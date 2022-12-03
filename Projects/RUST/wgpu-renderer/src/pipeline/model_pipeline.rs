use anyhow::*;

use super::Pipeline;
use crate::uniform::UniformGroupBuilder;
use crate::camera::{Camera, CameraUniform};
use crate::model::ModelUniform;

pub struct ModelPipeline {
    pipeline: Pipeline,
    camera_uniform: CameraUniform,
    model_uniform: ModelUniform,
}

impl ModelPipeline {
    pub fn new(
        device: &wgpu::Device,
        format: wgpu::TextureFormat,
    ) -> Result<Self> {
        // Create the shader module
        let shader = device.create_shader_module(
            wgpu::include_wgsl!("../shader.wgsl")
        );

        // Create the uniform group and the uniforms
        let mut builder = UniformGroupBuilder::new(&device);
        let camera_uniform = CameraUniform::from(
            builder.create_uniform(wgpu::ShaderStages::VERTEX)
        );
        let model_uniform = ModelUniform::from(
            builder.create_uniform(wgpu::ShaderStages::VERTEX)
        );
        let uniform_group = builder.build();

        Ok(Self {
            pipeline: Pipeline::new(device, format, uniform_group, shader)?,
            camera_uniform,
            model_uniform
        })
    }

    pub fn set_current<'a>(&'a self, render_pass: &mut wgpu::RenderPass<'a>) {
        self.pipeline.set_current(render_pass);
    }

    pub fn update_uniforms(
        &mut self,
        queue: &wgpu::Queue,
        camera: &Camera,
        position: cgmath::Vector3<f32>
    ) {
        self.camera_uniform.update_view_proj(queue, camera);
        self.model_uniform.update(queue, position);
    }
}