using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace SplatterPlots
{
    public class DensityRenderer
    {        
        public DensityRenderer()
        {
            Points = new List<ProjectedPoint>();
        }
        public int[] Histogram { get { return m_Histogram; } }
        public List<ProjectedPoint> Points { get; private set; }
        public void Init(int w, int h, OpenTK.Graphics.IGraphicsContext context)
        {
            Width = w;
            Height = h;

            //	initializeGLFunctions(context);

            int temp = -99;
            GL.GetInteger(GetPName.DrawBuffer, out temp);

            //setup
            GL.GenTextures(1, out textureColor);
            GL.BindTexture(TextureTarget.Texture2D, textureColor);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R32f, Width, Height, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.GenTextures(1, out textureHandle0);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R32f, Width, Height, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.GenTextures(1, out textureHandle1);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle1);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R32f, Width, Height, 0, PixelFormat.Red, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.GenTextures(1, out textureHandleRGB);
            GL.BindTexture(TextureTarget.Texture2D, textureHandleRGB);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.GenTextures(1, out textureDist0);
            GL.BindTexture(TextureTarget.Texture2D, textureDist0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.GenTextures(1, out textureDist1);
            GL.BindTexture(TextureTarget.Texture2D, textureDist1);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, Width, Height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            string version = GL.GetString(StringName.Version);

            GL.GenFramebuffers(1, out fboHandle);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle);
                        
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandle0, 0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.ActiveTexture(TextureUnit.Texture0);

            temp = -99;
            GL.GetInteger(GetPName.DrawBuffer, out temp);

            blurProgram = new ShaderProgram("blur.vert","blur.frag",context);
            blurProgram.Link();

            coloring = new ShaderProgram("blur.vert","color.frag",context);            
            coloring.Link();	

            JFA = new ShaderProgram("blur.vert","jfa.frag",context);
            JFA.Link();

            BlurData = new float[Width * Height];
            ColorData = new float[Width * Height];
            DistData = new float[Width * Height];
        }
        public void Filter(Action<SeriesProjection,bool> paintPoints,SeriesProjection series)
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureColor, 0);

            paintPoints(series, true);

            //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, 0, 0);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureColor);  

            Points.Clear();
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Red, PixelType.Float, ColorData);
            for (int i = 0; i < ColorData.Length; i++)
            {
                if (ColorData[i] != 0)
                {
                    int val = (int)(ColorData[i]*series.dataPoints.Length);
                    //int y = i / Height;
                    //int x = i % Height;
                    //Points.Add(new System.Drawing.Point(x, y));
                    Points.Add(series.dataPoints[val]);
                }
            }

        }
        public void Blur(float sigma)
        {
            int kw = (int)(Math.Ceiling(sigma * 3));
            blurProgram.Bind();
            
            float[] vec = { 1.0f, 0.0f };
            SetShaderUniforms(kw, sigma, vec);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandle1, 0);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle0);           

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.PushMatrix();
            GL.LoadMatrix(ref Matrix4.Identity);

            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(Width, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(Width, Height);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, Height);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, textureHandle1);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandle0, 0);

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            vec[0] = 0; vec[1] = 1;
            SetShaderUniforms(kw, sigma, vec);
            GL.Begin(BeginMode.Quads);
            GL.Vertex2(0, 0);
            GL.Vertex2(Width, 0);
            GL.Vertex2(Width, Height);
            GL.Vertex2(0, Height);
            GL.End();

            GL.PopMatrix();
            GL.BindTexture(TextureTarget.Texture2D, textureHandle0);
            blurProgram.Release();

            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Red, PixelType.Float, BlurData);

            MaxVal = float.MinValue;
            for (int i = 0; i < Width * Height; i++)
            {
                MaxVal = Math.Max(BlurData[i], MaxVal);
            }

            float step = MaxVal / 100.0f;
            for (int i = 0; i < m_Histogram.Length; i++)
            {
                m_Histogram[i] = 0;
            }
            if (MaxVal != 0)
            {
                for (int i = 0; i < Width * Height; i++)
                {
                    int index = Math.Min(Convert.ToInt32(Math.Floor(BlurData[i] / step)), m_Histogram.Length - 1);

                    m_Histogram[index]++;
                }
            }
            else
            {
                m_Histogram[0] = Width * Height;
            }            
        }
        public void Shade(float r, float g, float b, float angle, float stripePeriod, float stripeWidth, float lowerLimit, float upperLimit)
        {
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandleRGB, 0);
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, textureDist1);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle0);

            coloring.Bind();
            coloring.SetUniform("u_Texture0", 0);
            coloring.SetUniform("DistText1", 1);
            coloring.SetUniform("rgbCol", r, g, b);
            coloring.SetUniform("angle", angle);
            coloring.SetUniform("stripePeriod", stripePeriod);
            coloring.SetUniform("stripeWidth", stripeWidth);
            coloring.SetUniform("lowerLimit", lowerLimit);
            coloring.SetUniform("upperLimit", upperLimit);
            coloring.SetUniform("maxVal", MaxVal);

            GL.LoadMatrix(ref Matrix4.Identity);
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0, 0);
                GL.Vertex2(0, 0);
                GL.TexCoord2(1, 0);
                GL.Vertex2(Width, 0);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Width, Height);
                GL.TexCoord2(0, 1);
                GL.Vertex2(0, Height);
            GL.End();

            coloring.Release();
        }
        public void JumpFlooding(float upperLimit)
        {
            //init jfa
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureDist0, 0);
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle0);

            JFA.Bind();
            JFA.SetUniform("init", 1);
            JFA.SetUniform("upperLimit", upperLimit);
            JFA.SetUniform("maxVal", MaxVal);

            GL.LoadMatrix(ref Matrix4.Identity);

            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0, 0);
                GL.Vertex2(0, 0);
                GL.TexCoord2(1, 0);
                GL.Vertex2(Width, 0);
                GL.TexCoord2(1, 1);
                GL.Vertex2(Width, Height);
                GL.TexCoord2(0, 1);
                GL.Vertex2(0, Height);
            GL.End();

            JFA.SetUniform("init", 0);

            float Size = Math.Max(Width, Height);
            float log2 = (float)(Math.Log(Size) / Math.Log(2.0));
            int n = (int)(Math.Ceiling(log2));
            int k = (int)(Math.Pow(2.0f, n - 1));
            n = (n - 1) / 2 + 1;
            for (int i = 0; i < n; i++)
            {
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureDist1, 0);
                
                GL.BindTexture(TextureTarget.Texture2D, textureDist0);
                JFA.SetUniform("kStep", k);

                GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
                GL.Begin(BeginMode.Quads);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(0, 0);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(Width, 0);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(Width, Height);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(0, Height);
                GL.End();

                k = Math.Max(1, k / 2);

                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureDist0, 0);
                
                GL.BindTexture(TextureTarget.Texture2D, textureDist1);

                JFA.SetUniform("kStep", k);
                GL.Begin(BeginMode.Quads);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(0, 0);
                    GL.TexCoord2(1, 0);
                    GL.Vertex2(Width, 0);
                    GL.TexCoord2(1, 1);
                    GL.Vertex2(Width, Height);
                    GL.TexCoord2(0, 1);
                    GL.Vertex2(0, Height);
                GL.End();
                k = Math.Max(1, k / 2);
            }
            GL.BindTexture(TextureTarget.Texture2D, textureDist1);
            GL.GetTexImage(TextureTarget.Texture2D, 0,  PixelFormat.Red, PixelType.Float, DistData);
            JFA.Release();
        }
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            int temp = -99;
            GL.GetInteger(GetPName.DrawBuffer, out temp);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
        }
        public void UnBind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            int temp = -99;
            GL.GetInteger(GetPName.DrawBuffer, out temp);
           // GL.BindTexture(TextureTarget.Texture2D, textureHandle0);
        }
        public void CleanUp()
        {
            UnBind();

            GL.DeleteFramebuffers(1, ref fboHandle);
            GL.DeleteTextures(1, ref textureHandle0);
            GL.DeleteTextures(1, ref textureHandle1);
        }
        public void Clear()
        {
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandle1, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandle0, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);            
        }
        public void BindRGB(int unit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            GL.BindTexture(TextureTarget.Texture2D, textureHandleRGB);
        }
        public float GetDist(float x, float y)
        {
            int ix = (int)(x);
            int iy = (int)(y);
            if (ix < 0 || ix >= Width || iy < 0 || iy >= Height)
            {
                return -1000;
            }
            return DistData[iy * Height + ix];
        }
        
        void SetShaderUniforms(int kw, float sigma, float[] vec)
        {
            blurProgram.SetUniform("kWindow", kw);
            blurProgram.SetUniform("sigma", sigma);
            blurProgram.SetUniform("u_Off", vec[0], vec[1]);
            blurProgram.SetUniform("u_Texture0", 0);
        }
        int fboHandle;
        int textureHandle0;
        int textureHandle1;
        int textureHandleRGB;
        int textureColor;
        int textureDist0;
        int textureDist1;
		
        int Width;
        int Height;
        float[] BlurData;
        float[] DistData;
        float[] ColorData;
        int[] m_Histogram = new int[100];
        float maxVal;
        ShaderProgram blurProgram;
        ShaderProgram coloring;
        ShaderProgram JFA;

        public float MaxVal { get { return maxVal; } set { maxVal = value; } }
    }
}
