using System;
using System.Collections.Generic;
using OpenTK.Graphics.ES20;

namespace SharpPi.Graphics
{
    public struct UniformFieldInfo
    {
        public int Location;
        public string Name;
        public int Size;
        public ActiveUniformType Type;
    }

    public class Shader : IDisposable
    {
        public int Program { get; private set; }
        private readonly Dictionary<string, int> UniformCache = new Dictionary<string, int>();
        private readonly Dictionary<string, int> AttribCache = new Dictionary<string, int>();

        public Shader(string vertexShader, string fragmentShader)
        {
            GLException.CheckError("Start of Shader");
            int vertShader = CompileShader(ShaderType.VertexShader, vertexShader);
            GLException.CheckError("After vertex shader");
            int fragShader = CompileShader(ShaderType.FragmentShader, fragmentShader);
            GLException.CheckError("After frag shader");
            Program = GL.CreateProgram();
            GLException.CheckError("CreateProgram");
            GL.AttachShader(Program, vertShader);
            GL.AttachShader(Program, fragShader);
            GL.LinkProgram(Program);
            GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out int Success);
            if (Success == 0)
                throw new GLException(ErrorCode.InvalidOperation, "GL.LinkProgram had info log: " + GL.GetProgramInfoLog(Program));

            GL.DeleteShader(vertShader);
            GL.DeleteShader(fragShader);
        }

        public void UseShader()
        {
            GL.UseProgram(Program);
        }

        public void Dispose()
        {
            GL.DeleteProgram(Program);
        }

        public UniformFieldInfo[] GetUniforms()
        {
            GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out int UnifromCount);
            UniformFieldInfo[] Uniforms = new UniformFieldInfo[UnifromCount];

            for (int i = 0; i < UnifromCount; i++)
            {
                string Name = GL.GetActiveUniform(Program, i, out int Size, out ActiveUniformType Type);

                UniformFieldInfo FieldInfo;
                FieldInfo.Location = GetUniformLocation(Name);
                FieldInfo.Name = Name;
                FieldInfo.Size = Size;
                FieldInfo.Type = Type;

                Uniforms[i] = FieldInfo;
            }

            return Uniforms;
        }

        public int GetUniformLocation(string uniform)
        {
            if (!UniformCache.TryGetValue(uniform, out int location))
            {
                location = GL.GetUniformLocation(Program, uniform);
                UniformCache.Add(uniform, location);

                if (location == -1)
                    throw new GLException(ErrorCode.InvalidValue, $"The uniform '{uniform}' does not exist in this shader!");
            }

            return location;
        }

        public int GetAttributeLocation(string attribute)
        {
            if (!AttribCache.TryGetValue(attribute, out int location))
            {
                location = GL.GetAttribLocation(Program, attribute);
                AttribCache.Add(attribute, location);

                if (location == -1)
                    throw new GLException(ErrorCode.InvalidValue, $"The uniform '{attribute}' does not exist in this shader!");
            }

            return location;
        }

        private int CompileShader(ShaderType type, string source)
        {
            GLException.CheckError("Before CreateShader(" + type + ")");
            int Shader = GL.CreateShader(type);
            GL.ShaderSource(Shader, 1, new string[] { source }, new int[] { source.Length });
            GL.CompileShader(Shader);
            GL.GetShader(Shader, ShaderParameter.CompileStatus, out int success);
            if (success != 1)
            {
                GL.GetShaderInfoLog(Shader, out string infoLog);
                throw new GLException(ErrorCode.InvalidOperation, $"GL.CompileShader for shader [{type}] had info log: {infoLog}");
            }

            return Shader;
        }
    }
}