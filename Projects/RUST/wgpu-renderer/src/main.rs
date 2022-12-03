use std::time::Instant;

use anyhow::*;
use camera::CameraController;
use winit::dpi::PhysicalSize;
use winit::event::*;
use winit::event_loop::{ControlFlow, EventLoop};
use winit::window::{Window, WindowBuilder};

mod camera;
mod renderer;
mod mesh;
mod model;
mod uniform;
mod texture;
mod mouse_input;
mod pipeline;

use crate::texture::Texture;
use crate::camera::Camera;
use crate::renderer::MasterRenderer;

/// Contains all the wgpu primitives and state
pub struct WgpuContext {
    /// A connection to a logical rendering device, can interact with resources
    /// on the gpu like buffer, textures, setup the pipeline (shader modules,
    /// samplers, bind groups) and create an encoder that translated each draw
    /// command too something each associated physical device can understand
    device: wgpu::Device,

    /// Groups the translated commands to a single one and send it to the
    /// physical device
    queue: wgpu::Queue,

    /// Container and manager for all the renderers, controls their order and
    /// facilitates their usage
    master_renderer: MasterRenderer,

    /// Depth buffer
    depth_texture: Texture,
}

impl WgpuContext {
    /// Instantiate a wgpu rendering context
    pub fn new(
        device: wgpu::Device,
        queue: wgpu::Queue,
        config: &wgpu::SurfaceConfiguration,
    ) -> Result<Self> {
        // Create the master renderer that will control all the renderers, its
        // order and its relations
        let master_renderer = 
            MasterRenderer::new(&device, config.format)?;

        // Depth bitmap, to avoid overlapping models
        let depth_texture = Texture::create_depth(&device, config);

        Ok(Self {
            device,
            queue,
            master_renderer,
            depth_texture
        })
    }

    /// Apply the configuration to the surface
    pub fn configure_surface(
        &self,
        surface: &wgpu::Surface,
        config: &wgpu::SurfaceConfiguration
    ) {
        surface.configure(&self.device, config);
    }
    
    /// Issue a render to a view (reference of a surface texture)
    pub fn render<'a>(&'a mut self, view: &'a wgpu::TextureView) -> Result<()> {
        // Get the command encoder that will, let the master renderer and its
        // inner renderers push all its commands in order and submit them to
        // the gpu
        let mut encoder = self
            .device
            .create_command_encoder(&wgpu::CommandEncoderDescriptor {
                label: Some("Render Encoder"),
            });

        self.master_renderer.render(
            &mut encoder,
            view, 
            &self.depth_texture.view
        );

        self.queue.submit(Some(encoder.finish()));

        Ok(())
    }

    /// Update all the uniforms owned by the master renderer / his child 
    /// renderers with refined input
    pub fn update(&mut self, camera: &Camera) {
        self.master_renderer.update_uniforms(&self.queue, camera);
    }
}

/// The target of a rendering context, in charge of processing its raw events
struct Display {
    /// A surface is a target where to draw
    surface: wgpu::Surface,

    /// The details on how to draw a `self.surface`, contains things like the
    /// format, the size, and when the changes are propagated from the gpu to
    /// the render target (PresentMode)
    config: wgpu::SurfaceConfiguration,

    /// The context that will render
    context: WgpuContext,

    /// Camera and the controller of the camera used by the context
    camera: Camera,
    camera_controller: CameraController,
}

impl Display {
    async fn new(window: &Window) -> Result<Self> {
        // Extract the size of the window
        let PhysicalSize { width, height } = window.inner_size();

        // Initialize wgpu and get a physical device compatible to the
        // surface created by the window
        let instance = wgpu::Instance::new(wgpu::Backends::VULKAN);
        let surface = unsafe { instance.create_surface(window) };
        let adapter = instance
            .request_adapter(&wgpu::RequestAdapterOptions {
                power_preference: wgpu::PowerPreference::default(),
                compatible_surface: Some(&surface),
                force_fallback_adapter: false,
            })
            .await
            .unwrap();

        // Get a logical device (default limits/features)
        let (device, queue) = adapter
            .request_device(
                &wgpu::DeviceDescriptor {
                    features: wgpu::Features::empty(),
                    limits: wgpu::Limits::default(),
                    label: Some("My device"),
                },
                None,
            )
            .await
            .unwrap();

        // Create the surface configuration
        let config = wgpu::SurfaceConfiguration {
            usage: wgpu::TextureUsages::RENDER_ATTACHMENT,
            format: *surface
                .get_supported_formats(&adapter)
                .get(0)
                .context("No supported format available for surface")
                .unwrap(),
            width,
            height,
            present_mode: wgpu::PresentMode::Fifo,
            alpha_mode: wgpu::CompositeAlphaMode::Auto,
        };

        // Create the wgpu rendering context and configure the surface with that
        // config using it
        let context = WgpuContext::new(device, queue, &config)?;
        context.configure_surface(&surface, &config);

        // Create the camera
        let camera = 
            Camera::new(width, height, (0.0, 0.0, 0.0), cgmath::Deg(90.0));

        Ok(Self {
            surface,
            config,
            context,
            camera,
            camera_controller: CameraController::new(1.0, 0.01),
        })
    }

    /// Handle resizing, reconfigure the surface if possible
    fn resize(&mut self, PhysicalSize { width, height }: PhysicalSize<u32>) {
        if width > 0 && height > 0 {
            self.config.width = width;
            self.config.height = height;
            self.context.configure_surface(&self.surface, &self.config);
        }
    }

    /// Handle window input
    fn input(&mut self, event: &WindowEvent) -> bool {
        self.camera_controller.process_event(event)
    }

    /// Handle general input, needed for mouse 3d camera input, as we need the
    /// raw movements
    fn process_device_event(&mut self, event: &DeviceEvent) {
        self.camera_controller.process_device_event(event);
    }

    /// Update loop, transformation from refined input, to refined state
    fn update(&mut self, dt: f32) {
        self.context.update(&self.camera);
        self.camera_controller.update_camera(&mut self.camera, dt);
    }

    /// Draw to the display
    fn render(&mut self) -> Result<()> {
        // Get the inner texture of the display
        let output = self.surface.get_current_texture()?;
        let view = output
            .texture
            .create_view(&wgpu::TextureViewDescriptor::default());

        // Render to it
        self.context.render(&view)?;

        // Issue propagation of that rendering from the GPU to the OS surface
        output.present();

        Ok(())
    }

    pub fn width(&self) -> u32 {
        self.config.width
    }
    
    pub fn height(&self) -> u32 {
        self.config.height
    }
}

fn main() -> Result<()> {
    // Initialize the logging backend
    env_logger::init();

    // Create the event loop and the window
    let event_loop = EventLoop::new();
    let window = WindowBuilder::new()
        .build(&event_loop)
        .context("Failed to create window")?;

    // Initialize wgpu rendering context
    let mut state = pollster::block_on(Display::new(&window))?;

    // Main loop
    let mut dt = 0.0;
    let mut it = Instant::now();
    event_loop.run(move |event, _, control_flow| {
        *control_flow = ControlFlow::Poll;

        match event {
            Event::WindowEvent { ref event, .. } if !state.input(event) => match event {
                WindowEvent::CloseRequested
                | WindowEvent::KeyboardInput {
                    input:
                        KeyboardInput {
                            state: ElementState::Pressed,
                            virtual_keycode: Some(VirtualKeyCode::Escape),
                            ..
                        },
                    ..
                } => {
                    *control_flow = ControlFlow::Exit;
                }
                WindowEvent::KeyboardInput {
                    input:
                        KeyboardInput {
                            state: ElementState::Pressed,
                            virtual_keycode: Some(VirtualKeyCode::R),
                            ..
                        },
                    ..
                } => {
                    window.request_redraw();
                }
                WindowEvent::Resized(physical_size) => {
                    state.resize(*physical_size);
                }
                WindowEvent::ScaleFactorChanged { new_inner_size, .. } => {
                    state.resize(**new_inner_size);
                }
                _ => {}
            },
            Event::RedrawRequested(_) => {
                state.update(dt);
                if let Err(e) = state.render() {
                    if let Some(e) = e.downcast_ref::<wgpu::SurfaceError>() {
                        match e {
                            wgpu::SurfaceError::Lost => {
                                state.resize(PhysicalSize::new(state.width(), state.height()));
                            }
                            wgpu::SurfaceError::OutOfMemory => {
                                *control_flow = ControlFlow::Exit;
                            }
                            _ => {
                                panic!("Unknown render error: {}", e);
                            }
                        }
                    }
                }
            },
            Event::DeviceEvent { 
                ref event,
                .. 
            } => {
                state.process_device_event(event);
            }
            Event::MainEventsCleared => {
                dt = it.elapsed().as_secs_f32();
                it = Instant::now();
                window.request_redraw();
            }
            _ => {}
        }
    })
}