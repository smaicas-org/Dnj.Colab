use wgpu::util::DeviceExt;
use cgmath::{Vector3, Matrix4};

use crate::mesh::Mesh;
use crate::uniform::Uniform;

pub struct Model {
    mesh: Mesh,
    render_info: RenderInfo,
}

impl Model {
    pub fn new(
        device: &wgpu::Device,
        mesh: Mesh
    ) -> Self {
        Self {
            render_info: RenderInfo::new(device, &mesh),
            mesh,
        }
    }

    pub fn render<'a>(
        &'a self,
        render_pass: &mut wgpu::RenderPass<'a>
    ) {
        self.render_info.render(self.mesh.indices_count(), render_pass);
    }
}

pub struct RenderInfo {
    vertex_buffer: wgpu::Buffer,
    index_buffer: wgpu::Buffer,
}

impl RenderInfo {
    pub fn new(
        device: &wgpu::Device,
        mesh: &Mesh
    ) -> Self {
        Self {
            vertex_buffer: device.create_buffer_init(
                &wgpu::util::BufferInitDescriptor {
                    label: Some("Vertex Buffer"),
                    contents: mesh.vertex_data(),
                    usage: wgpu::BufferUsages::VERTEX,
                }
            ),
            index_buffer: device.create_buffer_init(
                &wgpu::util::BufferInitDescriptor {
                    label: Some("Index Buffer"),
                    contents: mesh.index_data(),
                    usage: wgpu::BufferUsages::INDEX,
                }
            ),
        }
    }

    pub fn render<'a>(
        &'a self,
        indices_count: u32,
        render_pass: &mut wgpu::RenderPass<'a>
    ) {
        render_pass.set_vertex_buffer(0, self.vertex_buffer.slice(..));
        render_pass.set_index_buffer(
            self.index_buffer.slice(..),
            wgpu::IndexFormat::Uint16
        );
        render_pass.draw_indexed(0..indices_count, 0, 0..1)
    }
}

pub struct ModelUniform {
    uniform: Uniform<cgmath::Matrix4<f32>>
}

impl From<Uniform<Matrix4<f32>>> for ModelUniform {
    fn from(uniform: Uniform<Matrix4<f32>>) -> Self {
        Self {
            uniform
        }
    }
}

impl ModelUniform {
    pub fn update(&self, queue: &wgpu::Queue, position: Vector3<f32>) {
        let transform = Matrix4::from_translation(position);
        self.uniform.update(queue, transform);
    }
}