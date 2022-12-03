use std::mem;

pub struct Mesh {
    vertices: &'static [Vertex],
    indices: &'static [u16],
}

impl Mesh {
    pub const QUAD: Self = Mesh {
        vertices: &[
            Vertex::new([-0.5, 0.5, 0.0], [1.0, 0.0, 0.0]),
            Vertex::new([-0.5, -0.5, 0.0], [0.0, 1.0, 0.0]),
            Vertex::new([0.5, -0.5, 0.0], [0.0, 0.0, 1.0]),
            Vertex::new([0.5, 0.5, 0.0], [1.0, 1.0, 1.0]),
        ],
        indices: &[0, 1, 2, 0, 2, 3],
    };

    #[allow(dead_code)]
    pub const TRIANGLE: Self = Mesh {
        vertices: &[
            Vertex::new([0.5, -0.5, 0.0], [1.0, 0.0, 0.0]),
            Vertex::new([0.0, 0.5, 0.0], [0.0, 1.0, 0.0]),
            Vertex::new([-0.5, -0.5, 0.0], [0.0, 0.0, 1.0]),
        ],
        indices: &[0, 1, 2],
    };

    #[allow(dead_code)]
    pub const PENTAGON: Self = Mesh {
        vertices: &[
            Vertex::new([-0.0868241, 0.49240386, 0.0], [0.0, 0.0, 0.0]), 
            Vertex::new([-0.49513406, 0.06958647, 0.0], [0.0, 0.0, 0.0]),
            Vertex::new([-0.21918549, -0.44939706, 0.0], [0.0, 0.0, 0.0]), 
            Vertex::new([0.35966998, -0.3473291, 0.0], [0.0, 0.0, 0.0]),
            Vertex::new([0.44147372, 0.2347359, 0.0], [0.0, 0.0, 0.0]),
        ],
        indices:  &[0, 1, 4,
                    1, 2, 4,
                    2, 3, 4]
    };

    pub const WEIRD: Self = Mesh {
        vertices: &[
            Vertex::new([-0.5, -0.5, 0.0], [1.0, 0.0, 0.0]),
            Vertex::new([0.0,  -0.5, 0.0], [0.0, 1.0, 0.0]),
            Vertex::new([-0.5, 0.0, 0.0], [0.0, 0.0, 1.0]),
            Vertex::new([0.5, 0.5, 0.0], [1.0, 0.0, 0.0]),
            Vertex::new([0.0,  0.5, 0.0], [0.0, 1.0, 0.0]),
            Vertex::new([0.5, 0.0, 0.0], [0.0, 0.0, 1.0]),
        ],
        indices: &[0, 1, 2, 3, 4, 5]
    };

    pub fn indices_count(&self) -> u32 {
        self.indices.len() as u32
    }

    pub fn vertex_data<'a>(&'a self) -> &'a [u8] {
        unsafe {
            std::slice::from_raw_parts(
                self.vertices.as_ref().as_ptr() as *const u8,
                std::mem::size_of_val(self.vertices.as_ref()),
            )
        }
    }

    pub fn index_data<'a>(&'a self) -> &'a [u8] {
        unsafe {
            std::slice::from_raw_parts(
                self.indices.as_ref().as_ptr() as *const u8,
                std::mem::size_of_val(self.indices.as_ref()),
            )
        }
    }
}

pub const VERTEX_DESC: wgpu::VertexBufferLayout<'static> = 
    wgpu::VertexBufferLayout {
        array_stride: mem::size_of::<Vertex>() as wgpu::BufferAddress,
        step_mode: wgpu::VertexStepMode::Vertex,
        attributes: &[
            wgpu::VertexAttribute {
                offset: 0,
                shader_location: 0,
                format: wgpu::VertexFormat::Float32x3,
            },
            wgpu::VertexAttribute {
                offset: mem::size_of::<[f32; 3]>() as wgpu::BufferAddress,
                shader_location: 1,
                format: wgpu::VertexFormat::Float32x3,
            }
        ]
    };

#[repr(C)]
#[derive(Debug, Clone, Copy)]
struct Vertex {
    position: [f32; 3],
    color: [f32; 3],
}

impl Vertex {
    const fn new(position: [f32; 3], color: [f32; 3]) -> Self {
        Self { 
            position,
            color,
        }
    }
}