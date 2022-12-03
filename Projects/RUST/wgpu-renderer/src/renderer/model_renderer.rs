#![allow(dead_code)]

use crate::model::{ModelUniform, Model};

/// Renderer for a specific model, can render that model at multiple places
/// TODO: Implement proper instanced rendering
pub struct ModelRenderer {
    /// The specific model
    model: Option<Model>,

    /// Dynamic data to upload
    model_uniform: ModelUniform,

    /// The instances of those models, with a fixed position
    instances: Vec<cgmath::Vector3<f32>>,
}

impl ModelRenderer {
    /// Create the model renderer, that renders a certain model
    pub fn new(model_uniform: ModelUniform, model: Model) -> Self {
        Self {
            model: Some(model),
            model_uniform,
            instances: vec![cgmath::Vector3::new(0.0, 0.0, 0.0)],
        }
    }

    /// Change the model for a new one
    pub fn replace_model(&mut self, new_model: Model) {
        self.model.replace(new_model);
    }

    /// Insert a new instance with a certain model transform
    pub fn add_instance(&mut self, position: cgmath::Vector3<f32>) {
        self.instances.push(position);
    }

    /// Render all the instanced models on a single pass
    pub fn render<'a>(
        &'a mut self,
        queue: &wgpu::Queue,
        render_pass: &mut wgpu::RenderPass<'a>
    ) {
        let model = self.model.as_ref().unwrap();
        for instance in &self.instances {
            self.model_uniform.update(queue, instance.clone());
            model.render(render_pass);
        }
    }
}