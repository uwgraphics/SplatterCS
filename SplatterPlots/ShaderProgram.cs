using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace SplatterPlots
{
    public class ShaderProgram
    {
        Dictionary<string, int> locationMap = new Dictionary<string, int>();
        int ID;
        int vp;
        int fp;
        OpenTK.Graphics.IGraphicsContext m_context;
        
        public ShaderProgram(string vertexFile,string fragmentFile,OpenTK.Graphics.IGraphicsContext context)
        {
            m_context = context;
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            vp = AddShaderProgram(File.ReadAllText(vertexFile), ShaderType.VertexShader);
            fp = AddShaderProgram(File.ReadAllText(fragmentFile), ShaderType.FragmentShader);            
        }
        private static int AddShaderProgram(string source, ShaderType type)
        {
            int p = GL.CreateShader(type);
            GL.ShaderSource(p, source);

            GL.CompileShader(p);

            string info;
            GL.GetShaderInfoLog(p, out info);

            return p;
        }

        public void Link()
        {
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            ID = GL.CreateProgram();
            GL.AttachShader(ID, fp);
            GL.AttachShader(ID, vp);
            GL.LinkProgram(ID);

            string sInfo = GL.GetProgramInfoLog(ID);
        }
        public void Bind()
        {
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            GL.UseProgram(ID);
        }
        public void Release()
        {
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            GL.UseProgram(0);
        }
        public void SetUniform(string name, int val)
        {
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            int location;
            location = GetLocation(name);
            GL.Uniform1(location, val);
        }
        public void SetUniform(string name, float val)
        {
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            int location;
            location = GetLocation(name);
            GL.Uniform1(location, val);
        }
        public void SetUniform(string name, float v0, float v1)
        {
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            int location;
            location = GetLocation(name);
            GL.Uniform2(location, v0, v1);
        }
        public void SetUniform(string name, float v0, float v1, float v2)
        {
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            int location;
            location = GetLocation(name);
            GL.Uniform3(location, v0,v1,v2);
        }
        private int GetLocation(string name)
        {
            System.Diagnostics.Debug.Assert(m_context.IsCurrent);
            int location;
            if (!locationMap.TryGetValue(name, out location))
            {
                location = GL.GetUniformLocation(ID, name);
                if (location > 0)
                {
                    locationMap[name] = location;
                }
            }
            return location;
        }
    }
}
