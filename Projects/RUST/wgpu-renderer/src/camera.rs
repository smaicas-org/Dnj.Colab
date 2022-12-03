use cgmath::{
    Point3, Matrix4, Vector3, Rad, Deg, Bounded
};
use winit::event::*;

use crate::mouse_input::MouseInput;
use crate::uniform::Uniform;

const OPENGL_TO_WGPU_MATRIX: cgmath::Matrix4<f32> = cgmath::Matrix4::new(
    1.0, 0.0, 0.0, 0.0,
    0.0, 1.0, 0.0, 0.0,
    0.0, 0.0, 0.5, 0.0,
    0.0, 0.0, 0.5, 1.0,
);

const BASE_DIR: Vector3<f32> = Vector3::new(0.0, 0.0, -1.0);

#[derive(Debug)]
pub struct Camera {
    view: View,
    projection: Projection
}

impl Camera {
    pub fn new<
        P: Into<Point3<f32>>,
        A: Into<Rad<f32>>
    >(
        width: u32,
        height: u32,
        position: P,
        fovy: A,
    ) -> Self {
        Self {
            view: View::new(position, Deg(0.0), Deg(0.0)),
            projection: Projection::new(width, height, fovy, 0.1, 1000.0) 
        }
    }

    pub fn resize(&mut self, width: u32, height: u32) {
        self.projection.resize(width, height);
    }

    pub fn calc_matrix(&self) -> Matrix4<f32> {
        self.projection.calc_matrix() * self.view.calc_matrix()
    }
    
    fn calc_dirs(&self) -> (Vector3<f32>, Vector3<f32>) {
        self.view.calc_dirs()
    }

    fn update_position<F: Fn(&mut Point3<f32>)>(&mut self, f: F) {
        f(&mut self.view.position);
    }

    fn update_yaw<F: Fn(&mut Rad<f32>)>(&mut self, f: F) {
        f(&mut self.view.yaw);
    }
    
    fn update_pitch<F: Fn(&mut Rad<f32>)>(&mut self, f: F) {
        f(&mut self.view.pitch);
    }
}

#[derive(Debug)]
struct View {
    position: Point3<f32>,
    yaw: Rad<f32>,
    pitch: Rad<f32>
}

impl View {
    fn new<
        V: Into<Point3<f32>>,
        A: Into<Rad<f32>>
    >(
        position: V,
        yaw: A,
        pitch: A
    ) -> Self {
        Self {
            position: position.into(),
            yaw: yaw.into(),
            pitch: pitch.into()
        }
    }

    fn calc_dirs(&self) -> (Vector3<f32>, Vector3<f32>) {
        let forward = (
            Matrix4::from_angle_y(self.yaw) *
            Matrix4::from_angle_x(self.pitch) * BASE_DIR.extend(1.0)
        ).truncate();
        let right = Vector3::new(0.0, 1.0, 0.0).cross(forward);

        (forward, right)
    }

    fn calc_matrix(&self) -> Matrix4<f32> {
        let (forward, _) = self.calc_dirs();

        Matrix4::look_to_rh(
            self.position,
            forward,
            Vector3::unit_y()
        )
    }
}

#[derive(Debug)]
struct Projection {
    aspect: f32,
    fovy: Rad<f32>,
    znear: f32,
    zfar: f32
}

impl Projection {
    fn new<F: Into<Rad<f32>>>(
        width: u32,
        height: u32,
        fovy: F,
        znear: f32,
        zfar: f32
    ) -> Self {
        Self {
            aspect: width as f32 / height as f32,
            fovy: fovy.into(),
            znear,
            zfar
        }
    }

    fn resize(&mut self, width: u32, height: u32) {
        self.aspect = width as f32 / height as f32;
    }

    fn calc_matrix(&self) -> Matrix4<f32> {
        OPENGL_TO_WGPU_MATRIX * 
            cgmath::perspective(self.fovy, self.aspect, self.znear, self.zfar)
    }
}

pub struct CameraController {
    speed: f32,
    sensitivity: f32,
    mouse_input: Option<MouseInput>,
    grab_mouse: bool,
    forward: bool,
    backward: bool,
    left: bool,
    right: bool,
    up: bool,
    down: bool
}

impl CameraController {
    pub fn new(speed: f32, sensitivity: f32) -> Self {
        Self {
            speed,
            sensitivity,
            mouse_input: None,
            grab_mouse: false,
            forward: false,
            backward: false,
            left: false,
            right: false,
            up: false,
            down: false
        }
    }

    pub fn process_event(&mut self, event: &WindowEvent) -> bool {
        match event {
            WindowEvent::KeyboardInput {
                input: KeyboardInput {
                    state,
                    virtual_keycode: Some(keycode),
                    ..
                },
                ..
            } => {
                let is_pressed = *state == ElementState::Pressed;
                match keycode {
                    VirtualKeyCode::W => self.forward = is_pressed,
                    VirtualKeyCode::S => self.backward = is_pressed,
                    VirtualKeyCode::A => self.left = is_pressed,
                    VirtualKeyCode::D => self.right = is_pressed,
                    VirtualKeyCode::Q => self.up = is_pressed,
                    VirtualKeyCode::E => self.down = is_pressed,
                    VirtualKeyCode::L => {
                        if is_pressed {
                            self.grab_mouse = !self.grab_mouse;
                        }
                    },
                    VirtualKeyCode::Space => self.up = is_pressed,
                    VirtualKeyCode::LShift => self.down = is_pressed,
                    _ => {
                        return false;
                    }
                };

                return true;
            },
            _ => false
        }
    }

    pub fn process_device_event(
        &mut self,
        device_event: &DeviceEvent,
    ) {
        if let Some(mouse_input) = &mut self.mouse_input {
            mouse_input.process_mouse_input(device_event);
        } else {
            self.mouse_input = Some(MouseInput::default());
        }
    }

    pub fn update_camera(&mut self, camera: &mut Camera, dt: f32) {
        if !self.grab_mouse {
            return;
        }

        // Calculate the vector that points towards the scene and to the right
        let (forward, right) = camera.calc_dirs();
        let step = self.speed * dt;

        if self.forward {
            camera.update_position(|pos| *pos += forward * step);
        }
        if self.backward {
            camera.update_position(|pos| *pos -= forward * step);
        }
        if self.left {
            camera.update_position(|pos| *pos += right * step);
        }
        if self.right {
            camera.update_position(|pos| *pos -= right * step);
        }
        if self.up {
            camera.update_position(|pos| {
                *pos += Vector3::new(0.0, 1.0, 0.0) * step
            });
        }
        if self.down {
            camera.update_position(|pos| {
                *pos -= Vector3::new(0.0, 1.0, 0.0) * step
            });
        }

        if let Some(mouse_input) = self.mouse_input.take() {
            camera.update_pitch(|angle| {
                *angle = Rad(
                    f32::clamp(
                        (*angle - Rad(mouse_input.delta_y * self.sensitivity)).0,
                        <Rad<f32> as Bounded>::min_value().0 + 0.1,
                        <Rad<f32> as Bounded>::max_value().0 - 0.1
                    )
                )
            });
            camera.update_yaw(|angle| {
                *angle -= Rad(mouse_input.delta_x * self.sensitivity)
            });
        }
    }
}

pub struct CameraUniform {
    uniform: Uniform<Matrix4<f32>>
}

impl From<Uniform<Matrix4<f32>>> for CameraUniform {
    fn from(uniform: Uniform<Matrix4<f32>>) -> Self {
        Self {
            uniform
        }
    }
}

impl CameraUniform {
    pub fn update_view_proj(&mut self, queue: &wgpu::Queue, camera: &Camera) {
        let view_proj_data = camera.calc_matrix().into();
        self.uniform.update(queue, view_proj_data);
    }
}

/*
pub struct CameraUniform {
    view_proj: [[f32; 4]; 4],
    buffer: wgpu::Buffer,
    bind_group: wgpu::BindGroup,
    bind_group_layout: wgpu::BindGroupLayout,
}

impl CameraUniform {
    pub fn new(device: &wgpu::Device) -> Self {
        use cgmath::SquareMatrix;
        use wgpu::util::DeviceExt;

        let view_proj: [[f32; 4]; 4] = cgmath::Matrix4::identity().into();
        let buffer = device.create_buffer_init(&wgpu::util::BufferInitDescriptor {
            label: Some("Camera Buffer"),
            contents: unsafe {
                std::slice::from_raw_parts(
                    view_proj.as_ref().as_ptr() as *const u8,
                    std::mem::size_of_val(view_proj.as_ref()),
                )
            },
            usage: wgpu::BufferUsages::UNIFORM | wgpu::BufferUsages::COPY_DST,
        });
        let bind_group_layout = device.create_bind_group_layout(&wgpu::BindGroupLayoutDescriptor {
            entries: &[wgpu::BindGroupLayoutEntry {
                binding: 0,
                visibility: wgpu::ShaderStages::VERTEX,
                ty: wgpu::BindingType::Buffer {
                    ty: wgpu::BufferBindingType::Uniform,
                    has_dynamic_offset: false,
                    min_binding_size: None,
                },
                count: None,
            }],
            label: Some("camera_bind_group_layout"),
        });
        let bind_group = device.create_bind_group(&wgpu::BindGroupDescriptor {
            layout: &bind_group_layout,
            entries: &[wgpu::BindGroupEntry {
                binding: 0,
                resource: buffer.as_entire_binding(),
            }],
            label: Some("CameraUniform::bind_group"),
        });

        Self {
            view_proj,
            buffer,
            bind_group,
            bind_group_layout,
        }
    }

    pub fn bind_group_layout(&self) -> &wgpu::BindGroupLayout {
        &self.bind_group_layout
    }

    pub fn bind_group(&self) -> &wgpu::BindGroup {
        &self.bind_group
    }

    pub fn update_view_proj(&mut self, queue: &wgpu::Queue, camera: &Camera) {
        self.view_proj = camera.calc_matrix().into();
        queue.write_buffer(&self.buffer, 0, unsafe {
            std::slice::from_raw_parts(
                self.view_proj.as_ref().as_ptr() as *const u8,
                std::mem::size_of_val(self.view_proj.as_ref()),
            )
        });
    }
}
*/

