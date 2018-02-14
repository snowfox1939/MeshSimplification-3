using System.Diagnostics;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Polynano.Graphics.Helpers
{
    /// <summary>
    /// Default implementation of a ShaderProgram for the MeshView.
    /// </summary>
    public class ShaderProgram : IShaderProgram
    {
        private readonly int _modelMatrixLocation;

        private readonly int _viewMatrixLocation;

        private readonly int _projectionMatrixLocation;

        private readonly int _diffuseColorLocation;

        private const string VertexShaderSource = @"
#version 440

layout (location = 0) in vec3 vPosition;
layout (location = 1) in vec3 vNormal;

uniform mat4 model_matrix;
uniform mat4 view_matrix;
uniform mat4 projection_matrix;

out vec3 normalInterp;
out vec3 vertPos;

void main(){
    gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vPosition, 1.0);
    vec4 vertPos4 = view_matrix * model_matrix * vec4(vPosition, 1.0);

    normalInterp = vec3(transpose(inverse(view_matrix * model_matrix))) * vNormal;
    vertPos = vec3(vertPos4) / vertPos4.w;
}";

        private const string FragmentShaderSource = @"
#version 440
precision mediump float;

in vec3 vertPos;
in vec3 normalInterp;

out vec4 fragColor;


uniform vec3 diffuseColor;

const vec3 lightPos  = vec3(200,60,250);
const vec3 specColor  = vec3(1.0, 1.0, 1.0);

void main() {

 vec3 ambientColor = diffuseColor * 0.4;
 vec3 normal = mix(normalize(normalInterp), normalize(cross(dFdx(vertPos), dFdy(vertPos))), 0.9999999f);
 vec3 lightDir = normalize(lightPos - vertPos);

 float lambertian = max(dot(lightDir,normal), 0.0);
 float specular = 0.0;

 if(lambertian > 0.0) {
  vec3 viewDir = normalize(-vertPos);
  vec3 halfDir = normalize(lightDir + viewDir);
  float specAngle = max(dot(halfDir, normal), 0.0);
  specular = pow(specAngle, 16.0);
 }

 fragColor = vec4(ambientColor + lambertian * diffuseColor + specular * specColor, 1.0);
}";

        public ShaderProgram()
        {
            // Create the program, attach and link the shaders
            ProgramId = GL.CreateProgram();

            // create the two shaders and remove them shortly after
            // since they are not needed after the program has been linked.

            using (var vs = new Shader(ShaderType.VertexShader, VertexShaderSource))
            {
                using (var fs = new Shader(ShaderType.FragmentShader, FragmentShaderSource))
                {
                    GL.AttachShader(ProgramId, vs.Id);
                    GL.AttachShader(ProgramId, fs.Id);

                    GL.LinkProgram(ProgramId);
                    Debug.WriteLine(GL.GetProgramInfoLog(ProgramId));

                    GL.DetachShader(ProgramId, vs.Id);
                    GL.DetachShader(ProgramId, fs.Id);
                }
            }

            // find the positions of the parameters
            _modelMatrixLocation = GL.GetUniformLocation(ProgramId, "model_matrix");
            _viewMatrixLocation = GL.GetUniformLocation(ProgramId, "view_matrix");
            _projectionMatrixLocation = GL.GetUniformLocation(ProgramId, "projection_matrix");
            _diffuseColorLocation = GL.GetUniformLocation(ProgramId, "diffuseColor");
            Debug.Assert(_modelMatrixLocation != -1 && _projectionMatrixLocation != -1 && _diffuseColorLocation != -1);
        }

        /// <summary>
        /// Get the index of the GL. ShaderProgram
        /// </summary>
        public int ProgramId { get; private set; }

        /// <summary>
        /// Set the model matrix
        /// </summary>
        public Matrix4 ModelMatrix
        {
            set => GL.UniformMatrix4(_modelMatrixLocation, false, ref value);
        }

        /// <summary>
        /// Set the view matrix
        /// </summary>
        public Matrix4 ViewMatrix
        {
            set => GL.UniformMatrix4(_viewMatrixLocation, false, ref value);
        }

        /// <summary>
        /// set the projection matrix
        /// </summary>
        public Matrix4 ProjectionMatrix
        {
            set => GL.UniformMatrix4(_projectionMatrixLocation, false, ref value);
        }

        /// <summary>
        /// set the mesh color 
        /// </summary>
        public Vector3 MeshColor
        {
            set => GL.Uniform3(_diffuseColorLocation, value);
        }

        ~ShaderProgram()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose the program.
        /// </summary>
        public void Dispose()
        {
            if (ProgramId != 0)
            {
                GL.DeleteProgram(ProgramId);
                ProgramId = 0;
            }
        }
    }
}