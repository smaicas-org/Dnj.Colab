use anyhow::*;

use crate::pipeline::ModelPipeline;
use crate::model::Model;
use crate::mesh::Mesh;
use crate::camera::Camera;

mod model_renderer;

pub use model_renderer::ModelRenderer;

pub struct MasterRenderer {
    /// Color used to clear the screen
    clear_color: wgpu::Color,

    /// Attached to a render pass is the programable process the data to be
    /// drawn goes through
    m1_pipeline: ModelPipeline,
    m2_pipeline: ModelPipeline,

    m1: Model,
    m2: Model,
}

impl MasterRenderer {
    /// Create a `MasterRenderer` for a certain SurfaceTexture
    pub fn new(
        device: &wgpu::Device,
        format: wgpu::TextureFormat,
    ) -> Result<Self> {
        Ok(Self {
            clear_color: wgpu::Color {
                r: 0.1,
                g: 0.2,
                b: 0.4,
                a: 1.0
            },
            m1_pipeline: ModelPipeline::new(
                device,
                format,
            )?,
            m2_pipeline: ModelPipeline::new(
                device,
                format,
            )?,
            m1: Model::new(device, Mesh::QUAD),
            m2: Model::new(device, Mesh::WEIRD)
        })
    }

    /// Main rendering, creates the render pass and manages the order of 
    /// pipelines/models and subrenderers
    pub fn render<'a>(
        &'a mut self,
        encoder: &mut wgpu::CommandEncoder,
        view: &wgpu::TextureView,
        depth_view: &wgpu::TextureView
    ) {

        // Clear
        let mut render_pass = encoder.begin_render_pass(&wgpu::RenderPassDescriptor {
            label: Some("Render pass"),
            color_attachments: &[Some(wgpu::RenderPassColorAttachment {
                view,
                resolve_target: None,
                ops: wgpu::Operations {
                    load: wgpu::LoadOp::Clear(
                        self.clear_color()
                    ),
                    store: true,
                },
            })],
            depth_stencil_attachment: Some(
                wgpu::RenderPassDepthStencilAttachment {
                    view: depth_view,
                    depth_ops: Some(wgpu::Operations {
                        load: wgpu::LoadOp::Clear(1.0),
                        store: true
                    }),
                    stencil_ops: None
                }
            ),
        });

        // Draw
        self.m1_pipeline.set_current(&mut render_pass);
        self.m2.render(&mut render_pass);
        self.m2_pipeline.set_current(&mut render_pass);
        self.m1.render(&mut render_pass);
    }

    pub fn clear_color(&self) -> wgpu::Color {
        self.clear_color
    }
    
    /// Update all the uniforms with refiened input in order
    pub fn update_uniforms(&mut self, queue: &wgpu::Queue, camera: &Camera) {
        self.m1_pipeline.update_uniforms(queue, camera, cgmath::Vector3::new(0.0, 1.0, -1.0));
        self.m2_pipeline.update_uniforms(queue, camera, cgmath::Vector3::new(0.0, 0.0, -2.0));
    }
}