use winit::event::*;

#[derive(Debug, Default)]
pub struct MouseInput {
    pub delta_x: f32,
    pub delta_y: f32,
    pub right_button: bool,
    pub left_button: bool
}

impl MouseInput {
    pub fn process_mouse_input(&mut self, event: &DeviceEvent) -> bool {
        match event {
            DeviceEvent::MouseMotion { 
                delta
            } => {
                self.delta_x = delta.0 as f32;
                self.delta_y = delta.1 as f32;
            },
            DeviceEvent::Button { 
                button,
                state: ElementState::Pressed
            } => {
                if *button == 0 {
                    self.left_button = true;
                } else if *button == 1 {
                    self.right_button = true;
                }
            }
            _ => return false
        }

        return true;
    }
}