using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using System.Runtime.InteropServices;


namespace SplatterPlots
{
    public partial class SplatterView: GLControl
    {
        private static OpenTK.Graphics.GraphicsMode GetMode(){
            var def = OpenTK.Graphics.GraphicsMode.Default;
            var val = new OpenTK.Graphics.GraphicsMode(def.ColorFormat,def.Depth,def.Stencil,0);
            return val;
        }
        #region Private Fields
        SplatterModel m_Model;
        Timer m_TooltipTimer;
        ToolTip m_ToolTip;
        Point m_MousePosition;
        ShaderProgram m_BlendProgram;
        Matrix4 m_Ortho;

        MaxMode m_MaxMode;
        Dictionary<string, DensityRenderer> m_DensityMap = new Dictionary<string, DensityRenderer>();
        Dictionary<string, VBO> m_VboMap = new Dictionary<string, VBO>();

        float m_OffsetX;
        float m_OffsetY;
        float m_ScaleX;
        float m_ScaleY;
        int m_ScreenOffsetX;
        int m_ScreenOffsetY;
        float TotalScaleX { get { return m_ScaleX * m_ScaleFactorX; } }
        float TotalScaleY { get { return m_ScaleY * m_ScaleFactorY; } }        

        bool m_PanEnabled;
        int m_PanOrigX;
        int m_PanOrigY;

        bool m_SelectEnabled;
        int m_SelectOrigX;
        int m_SelectOrigY;
        int m_SelectNowX;
        int m_SelectNowY;

        float m_Bandwidth;
        float m_ClutterWindow;        
        float m_ContourThreshold;
        float m_DensityThreshold;        
        float m_StripePeriod;
        float m_StripeWidth;
        float m_ChromaF;
        float m_LightnessF;
        float m_ScaleFactorX;
        float m_ScaleFactorY;

        bool m_Contours = true;
        bool m_Loaded;
        bool m_ShowGrid = true;
        #endregion

        #region Public Properties
        public event EventHandler ModelChanged;
        public event EventHandler PointSelection;
        public SplatterModel Model { get { return m_Model; } }
        public float Bandwidth
        {
            get { return m_Bandwidth; }
            set { m_Bandwidth = value; Refresh(); }
        }        
        public float ClutterWindow
        {
            get { return m_ClutterWindow; }
            set { m_ClutterWindow = value; Refresh(); }
        }
        public float ContourThreshold
        {
            get { return m_ContourThreshold; }
            set { m_ContourThreshold = value; Refresh(); }
        }
        public float DensityThreshold
        {
            get { return m_DensityThreshold; }
            set { m_DensityThreshold = Math.Max(value, 0.001f); Refresh(); }
        }
        public float ChromaF
        {
            get { return m_ChromaF; }
            set { m_ChromaF = value; Refresh(); }
        }
        public float LightnessF
        {
            get { return m_LightnessF; }
            set { m_LightnessF = value; Refresh(); }
        }        
        public bool ShowGrid
        {
            get { return m_ShowGrid; }
            set { m_ShowGrid = value; Refresh(); }
        }
        public MaxMode MaxMode
        {
            get { return m_MaxMode; }
            set { m_MaxMode = value; Refresh(); }
        }        
        public float ScaleFactorX
        {
            get { return m_ScaleFactorX; }
            set { m_ScaleFactorX = value; Refresh(); }
        }
        public float ScaleFactorY
        {
            get { return m_ScaleFactorY; }
            set { m_ScaleFactorY = value; Refresh(); }
        }
        public float StripePeriod
        {
            get { return m_StripePeriod; }
            set { m_StripePeriod = value; Refresh(); }
        }
        public float StripeWidth
        {
            get { return m_StripeWidth; }
            set { m_StripeWidth = value; Refresh(); }
        }
        #endregion

        #region Construction
        public SplatterView()
            : base(GetMode())
        {
            m_ScaleFactorX = 1;
            m_ScaleFactorY = 1;
            m_ChromaF = .95f;
            m_LightnessF = .95f;
            m_ClutterWindow = 30.0f;
            m_Bandwidth = 10;
            m_ContourThreshold = 1;
            m_DensityThreshold = 1;
            m_StripePeriod = 50;
            m_StripeWidth = 1;
            m_MaxMode = MaxMode.Global;
            m_MousePosition = new Point(-100, -100);

            InitializeComponent();
            m_OffsetX = 0;
            m_OffsetY = 0;

            m_ScaleX = 1;
            m_ScaleY = 1;

            m_PanEnabled = false;
            m_SelectEnabled = false;            
            this.MouseWheel += new MouseEventHandler(SplatterView_MouseWheel);
            m_TooltipTimer = new Timer();
            m_TooltipTimer.Interval = 1000;
            m_TooltipTimer.Tick += new EventHandler(TimerTick);            
            m_ToolTip = new ToolTip();

            // Force the ToolTip text to be displayed whether or not the form is active.
            m_ToolTip.ShowAlways = true;
        }
        #endregion

        #region Public Methods
        private void setBBox(float xmin, float ymin, float xmax, float ymax)
        {
            m_OffsetX = -(xmax + xmin) / 2.0f;
            m_OffsetY = -(ymax + ymin) / 2.0f;

            float rangeX = (xmax - xmin);
            float rangeY = (ymax - ymin);

            float range = Math.Max(rangeX, rangeY) * 1.2f;

            m_ScaleX = Width / range;
            m_ScaleY = m_ScaleX;
            m_ScaleFactorX = 1;
            m_ScaleFactorY = 1;

            m_ScreenOffsetX = (int)(Width / 2.0f);
            m_ScreenOffsetY = (int)(Height / 2.0f);

            Refresh();
        }
        
        public void setSplatPM(SplatterModel spm)
        {
            m_ScreenOffsetX = Width / 2;
            m_ScreenOffsetY = Height / 2;
            m_Model = spm;
            m_Model.ModelChanged += new EventHandler(splatPM_ModelChanged);
            m_DensityMap.Clear();
            SetGroups();
            setBBox(spm.xmin, spm.ymin, spm.xmax, spm.ymax);
            if (ModelChanged!=null)
            {
                ModelChanged(this, EventArgs.Empty);
            }
        }

        void splatPM_ModelChanged(object sender, EventArgs e)
        {
            SetGroups();
            Refresh();
        }
        public void SetGroups()
        {            
            ClearVBOs();

            if (m_DensityMap.Count <= 0)
            {
                foreach (var series in m_Model.Groups.Values)
                {
                    m_DensityMap[series.Name] = new DensityRenderer();
                }
            }

            if (m_Loaded)
            {
                MakeCurrent();
                foreach (var denisty in m_DensityMap.Values)
                {
                    denisty.Init(Width, Height, Context);
                }
                InitVBO();

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
        }
        private struct Vertex
        {
            public Vertex(ProjectedPoint p, float count)
            {
                Position.X = p.X;
                Position.Y = p.Y;
                Position.Z = p.Index / count;
                Color.X = p.Index / count;
                Color.Y = p.Index / count;
                Color.Z = p.Index / count;
                Position2.X = p.X;
                Position2.Y = p.Y;
                Position2.Z = p.Z;
              
            }
            public Vector3 Position;
            public Vector3 Color;
            public Vector3 Position2;

            public static readonly int Stride = Marshal.SizeOf(default(Vertex));
        }

        private void InitVBO()
        {
            foreach (var series in m_Model.Groups.Values)
            {
                int vbo;                
                var vertices = series.dataPoints.Select(p => new Vertex(p,series.dataPoints.Count)).ToArray();
                GL.GenBuffers(1, out vbo);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(BufferTarget.ArrayBuffer,
                                       new IntPtr(vertices.Length * Vertex.Stride),
                                       vertices, BufferUsageHint.StaticDraw);
                m_VboMap[series.Name] = new VBO(vbo, vertices.Length);
            }
        }

        private void ClearVBOs()
        {
            foreach (var vbo in m_VboMap.Values)
            {                
                GL.DeleteBuffers(1, ref vbo.vbo);
            }
            m_VboMap.Clear();
        }
        #endregion

        #region Private methods

        #region Event Handlers
        private void SplatterView_Paint(object sender, PaintEventArgs e)
        {
#if DEBUG
            try
            {                
#endif
                glPaint();
#if DEBUG
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
#endif
        }
        
        private void SplatterView_Load(object sender, EventArgs e)
        {
            if (!Program.Runtime)
            {
                return;
            }

            m_Loaded = true;
            this.MakeCurrent();
            
            GL.Enable(EnableCap.Multisample);            
            int val;
            GL.GetInteger(GetPName.Samples,out val);
            GL.Disable(EnableCap.CullFace);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            string nam = this.Name;
            m_BlendProgram = new ShaderProgram("Opengl\\blur.vert", "Opengl\\blend.frag", Context);            
            m_BlendProgram.Link();

            m_BlendProgram.Bind();
            m_BlendProgram.SetUniform("u_Texture0", 0);
            m_BlendProgram.SetUniform("u_Texture1", 1);
            m_BlendProgram.SetUniform("u_Texture2", 2);
            m_BlendProgram.SetUniform("u_Texture3", 3);
            m_BlendProgram.SetUniform("u_Texture4", 4);
            m_BlendProgram.SetUniform("u_Texture5", 5);
            m_BlendProgram.SetUniform("u_Texture6", 6);
            m_BlendProgram.SetUniform("u_Texture7", 7);
            m_BlendProgram.Release();

            foreach (var denisty in m_DensityMap.Values)
            {
                denisty.Init(Width, Height,Context);
            }
            InitVBO();
        }
        
        private void SplatterView_Resize(object sender, EventArgs e)
        {
            if (!Program.Runtime)
            {
                return;
            }
            this.MakeCurrent();
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            GL.MatrixMode(MatrixMode.Projection);
            m_Ortho = Matrix4.CreateOrthographicOffCenter(ClientRectangle.X, ClientRectangle.Width, ClientRectangle.Y, ClientRectangle.Height, -100, 100);
            GL.LoadMatrix(ref m_Ortho);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        private void SplatterView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                m_PanEnabled = true;
                m_PanOrigX = e.X;
                m_PanOrigY = (Height - e.Y);
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                m_SelectEnabled = true;
                m_SelectOrigX = e.X;
                m_SelectOrigY = Height - e.Y;
            }
        }
        private void SplatterView_MouseMove(object sender, MouseEventArgs e)
        {            
            if (m_PanEnabled)
            {
                m_ScreenOffsetX += e.X - m_PanOrigX;
                m_PanOrigX = e.X;

                m_ScreenOffsetY += (Height - e.Y) - m_PanOrigY;
                m_PanOrigY = (Height - e.Y);
            }
            if (m_SelectEnabled)
            {
                m_SelectNowX = e.X;
                m_SelectNowY = Height - e.Y;
            }
            var positionDiff = Math.Sqrt((e.Location.X - m_MousePosition.X) * (e.Location.X - m_MousePosition.X) + (e.Location.Y - m_MousePosition.Y) * (e.Location.Y - m_MousePosition.Y));
            if (positionDiff > 5)
            {
                m_MousePosition = e.Location;
                m_TooltipTimer.Stop();
                m_ToolTip.Hide(this);
                m_TooltipTimer.Start();
            }            
            Refresh();
            
        }
        private void SplatterView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && m_PanEnabled)
            {
                m_PanEnabled = false;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right && m_SelectEnabled)
            {
                m_SelectEnabled = false;
                float xmin = unTransformX(Math.Min(m_SelectNowX, m_SelectOrigX));
                float ymin = unTransformY(Math.Min(m_SelectNowY, m_SelectOrigY));
                float xmax = unTransformX(Math.Max(m_SelectNowX, m_SelectOrigX));
                float ymax = unTransformY(Math.Max(m_SelectNowY, m_SelectOrigY));
                Model.Select(xmin, ymin, xmax, ymax);
                if (PointSelection!=null)
                {
                    PointSelection(this, EventArgs.Empty);
                }
            }
        }
        private void SplatterView_MouseWheel(object sender, MouseEventArgs e)
        {
            m_MousePosition.X = -100;
            m_MousePosition.Y = -100;
            m_TooltipTimer.Stop();
            m_ToolTip.Hide(this);

            if (e.Delta < 0)
            {
                scrollOut(e.X, Height - e.Y);
            }
            else
            {
                scrollIn(e.X, Height - e.Y);
            }
        }
        
        private void scrollIn(float x, float y)
        {
            float dx = unTransformX(x);
            float dy = unTransformY(y);

            m_ScaleX *= 1.0f / .9f;
            m_ScaleY *= 1.0f / .9f;

            m_ScreenOffsetX = (int)x;
            m_ScreenOffsetY = (int)y;

            m_OffsetX = -dx;
            m_OffsetY = -dy;

            Refresh();
        }
        private void scrollOut(float x, float y)
        {
            float dx = unTransformX(x);
            float dy = unTransformY(y);

            m_ScaleX *= .9f;
            m_ScaleY *= .9f;

            m_ScreenOffsetX = (int)x;
            m_ScreenOffsetY = (int)y;

            m_OffsetX = -dx;
            m_OffsetY = -dy;

            Refresh();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }
            var point = PointToClient(MousePosition);
            Vector2 center = new Vector2(point.X, Height - point.Y);
            float xmin = unTransformX(center.X - 5);
            float ymin = unTransformY(center.Y - 5);
            float xmax = unTransformX(center.X + 5);
            float ymax = unTransformY(center.Y + 5);

            var list = Model.GetSelectedList(xmin, ymin, xmax, ymax);
            if (list.Count > 0)
            {
                if (list.Count == 1)
                {
                    m_ToolTip.Show(list[0].ToString(), this, point, 10000);
                }
                else
                {
                    string format =
                        Model.dim0Name + ": {0}\n" +
                        Model.dim1Name + ": {1}\n" +
                        "About {2} data points.";
                    var x = unTransformX(center.X);
                    var y = unTransformY(center.Y);
                    string text = string.Format(format, x, y, list.Count);
                    m_ToolTip.Show(text, this, point, 10000);
                }
            }
        }
        #endregion

        #region Painting
        void glPaint()
        {
            if (m_Model == null)
                return;
            this.MakeCurrent();

            int N = 0;
            int I = 0;            

            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);            
            foreach (var series in m_Model.Groups.Values)
            {
                if (series.Enabled)
                {
                    blurPoints(series);
                }
            }
            if (MaxMode == MaxMode.Global)
            {
                float max = float.MinValue;
                foreach (var d in m_DensityMap.Values)
                {
                    max = Math.Max(d.MaxVal, max);
                }
                foreach (var d in m_DensityMap.Values)
                {
                    d.MaxVal = max;
                }
            }
            foreach (var series in m_Model.Groups.Values)
            {
                if (series.Enabled)
                {
                    doDistanceTransform(series);
                    renderSeries(series, (float)((Math.PI / m_Model.Groups.Count) * I));
                    N++;
                    I++;
                }
            }

            I = 0;
            foreach (var series in m_Model.Groups.Values)
            {
                if (series.Enabled)
                {
                    m_DensityMap[series.Name].BindRGB(I);
                    I++;
                }
            }

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.Blend);
            bool val = GL.IsEnabled(EnableCap.Blend);

            if (m_Contours)
            {
                m_BlendProgram.Bind();
                m_BlendProgram.SetUniform("N", N);
                m_BlendProgram.SetUniform("lf", m_LightnessF);
                m_BlendProgram.SetUniform("cf", m_ChromaF);

                GL.LoadMatrix(ref Matrix4.Identity);
                GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
                GL.Begin(BeginMode.Quads);
                GL.TexCoord2(0.0f, 0.0f);
                GL.Vertex2(0.0f, 0.0f);
                GL.TexCoord2(1.0f, 0.0f);
                GL.Vertex2(Width, 0.0f);
                GL.TexCoord2(1.0f, 1.0f);
                GL.Vertex2(Width, Height);
                GL.TexCoord2(0.0f, 1.0f);
                GL.Vertex2(0.0f, Height);
                GL.End();

                m_BlendProgram.Release();
            }
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            setZoomPan();

            if (m_Model.showAllPoints&&ClutterWindow<150)
            {
                foreach (var series in m_Model.Groups.Values)
                {
                    if (series.Enabled)
                    {
                        drawPoints(series);
                    }
                }
            }
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.LoadMatrix(ref Matrix4.Identity);
            drawSelect();
            if (ShowGrid)
            {
                drawGrid();
            }
            
            this.SwapBuffers();
        }
        void drawSelect()
        {
            if (m_SelectEnabled)
            {
                GL.Color4(.5f, .5f, .5f, .25f);
                GL.Begin(BeginMode.Quads);
                    GL.Vertex2(m_SelectOrigX,m_SelectOrigY);
                    GL.Vertex2(m_SelectNowX,m_SelectOrigY);
                    GL.Vertex2(m_SelectNowX, m_SelectNowY);
                    GL.Vertex2(m_SelectOrigX, m_SelectNowY);
                GL.End();
            }
        }
        void renderSeries(ISeriesProjection series, float angle)
        {
            
            GL.MatrixMode(MatrixMode.Modelview);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            m_DensityMap[series.Name].Bind();
            m_DensityMap[series.Name].Shade(series.Color.R / 255.0f, series.Color.G / 255.0f, series.Color.B / 255.0f, angle, m_StripePeriod, m_StripeWidth, m_DensityThreshold, m_ContourThreshold);
            m_DensityMap[series.Name].UnBind();
        }        
        void blurPoints(ISeriesProjection series)
        {
            m_DensityMap[series.Name].Bind();
            m_DensityMap[series.Name].Clear();
            paintPoints(series,false);
            m_DensityMap[series.Name].Blur(m_Bandwidth);
            m_DensityMap[series.Name].Filter(paintPoints, series);
            m_DensityMap[series.Name].UnBind();
            series.Histogram = m_DensityMap[series.Name].Histogram;
        }
        void doDistanceTransform(ISeriesProjection series)
        {
            m_DensityMap[series.Name].Bind();            
            m_DensityMap[series.Name].JumpFlooding(m_ContourThreshold);
            m_DensityMap[series.Name].UnBind();
        }
        void paintPoints(ISeriesProjection series,bool useColor)
        {            
            GL.PushMatrix();
            GL.PushAttrib(AttribMask.EnableBit);
            GL.DepthMask(false);
            GL.Disable(EnableCap.Texture2D);
            if (!useColor)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
            }
            else
            {
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Lequal);
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            setZoomPan();

            GL.EnableClientState(ArrayCap.VertexArray);
            if (useColor)
            {
                GL.EnableClientState(ArrayCap.ColorArray);
            }
            //GL.EnableVertexAttribArray(0);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VboMap[series.Name].vbo);            
            GL.VertexPointer(3, VertexPointerType.Float,Vertex.Stride, 0);
            if (useColor)
            {
                GL.ColorPointer(3, ColorPointerType.Float, Vertex.Stride, Vector3.SizeInBytes);
            }
            
            GL.PointSize(1);
            GL.Color4(1.0f, 1.0, 1.0,1.0f);
            //GL.Begin(BeginMode.Points);
            //foreach (var point in series.dataPoints)
            //{
            //    GL.Vertex2(point.X,point.Y);
            //}

            //GL.End();
            GL.DrawArrays(BeginMode.Points, 0, m_VboMap[series.Name].length);            
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);            
            //GL.DisableVertexAttribArray(0);
            GL.DisableClientState(ArrayCap.VertexArray);
            if (useColor)
            {
                GL.DisableClientState(ArrayCap.ColorArray);
                GL.Disable(EnableCap.DepthTest);
            }
            GL.PopAttrib();
            GL.PopMatrix();
            GL.DepthMask(true);
        }
        void setZoomPan()
        {
            GL.LoadMatrix(ref Matrix4.Identity);

            GL.Translate(m_ScreenOffsetX, m_ScreenOffsetY, 0);
            GL.Scale(TotalScaleX, TotalScaleY, 1);
            GL.Translate(m_OffsetX, m_OffsetY, 0);
            //glScalef(1, -1, 1);
        }
        public void saveScreenShot(string name)
        {
            MakeCurrent();
            Refresh();

            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            int pointCount = NumberOfPointsInView();
            bmp.Save(string.Format("{0}.png", name), System.Drawing.Imaging.ImageFormat.Png);
        }
        public int NumberOfPointsInView()
        {
            float xmin = unTransformX(0);
            float ymin = unTransformY(0);
            float xmax = unTransformX(ClientSize.Width);
            float ymax = unTransformY(ClientSize.Height);

            var count = Model.Groups.Values.Sum(
                (s => s.dataPoints.Count(
                    p => p.X >= xmin && p.X <= xmax &&
                         p.Y >= ymin && p.Y <= ymax)));
            return count;
        }
        public void ZoomIn()
        {
            m_ScaleX *= 1.0f / .9f;
            m_ScaleY *= 1.0f / .9f;
            Refresh();
        }
        public void ZoomOut()
        {
            m_ScaleX *= .9f;
            m_ScaleY *= .9f;
            Refresh();
        }
        void drawGrid()
        {
            QFontBuilderConfiguration vonfig = new QFontBuilderConfiguration();
            
            
            GL.PushAttrib(AttribMask.EnableBit);
            QFont font = new QFont(Font);
            font.Options.Colour = new OpenTK.Graphics.Color4(0, 0, 0, 1.0f);
            GL.PopAttrib();

            //do minor lines first
            float min = unTransformY(0);
            float max = unTransformY(Height);

            int exp = (int)(Math.Floor(Math.Log10(max - min)));
            double d = Math.Pow(10.0, exp);
            d *= .1;
            //if((max-min)/d < 3) d*=.1;

            double graphmin = Math.Floor(min / d) * d;
            double graphmax = Math.Ceiling(max / d) * d;

            float alpha = (float)(1.0f - (150.0f - d * m_ScaleY) / 150.0f);
            alpha = Math.Max(alpha, 0.0f);
            alpha = Math.Min(alpha, 1.0f);

            GL.Color4(0, 0, 0, alpha / 2.0f);            
            GL.LineWidth(1.5f);
            GL.Begin(BeginMode.Lines);
            for (double y = graphmin; y < graphmax + .5 * d; y += d)
            {
                int yi = (int)transformY((float)y);
                GL.Vertex2(0, yi);
                GL.Vertex2(Width, yi);
            }
            GL.End();
            //now major
            d *= 10;
            graphmin = Math.Floor(min / d) * d;
            graphmax = Math.Ceiling(max / d) * d;
            alpha = (float)(1.0f - (150.0 - d * m_ScaleY) / 150.0);
            alpha = Math.Max(alpha, 0.0f);
            alpha = Math.Min(alpha, 1.0f);
                        
            for (double y = graphmin; y < graphmax + .5 * d; y += d)
            {
                GL.Color4(0.0f, 0.0f, 0, alpha / 2.0f);
                GL.Begin(BeginMode.Lines);
                int yi = (int)transformY((float)y);
                GL.Vertex2(0, yi);
                GL.Vertex2(Width, yi);
                GL.End();
                GL.PushAttrib(AttribMask.EnableBit);
                QFontBegin();
                font.Print(string.Format("{0:G}", y), new Vector2(5, Height - yi));
                QFontEnd();
                GL.PopAttrib();
            }
            
            ///////////////////////////////////////////////////////////////

            ////do minor lines first
            min = unTransformX(0);
            max = unTransformX(Width);

            exp = (int)(Math.Floor(Math.Log10(max - min)));
            d = Math.Pow(10.0, exp);
            d *= .1;
            //if((max-min)/d < 3) d*=.1;

            graphmin = Math.Floor(min / d) * d;
            graphmax = Math.Ceiling(max / d) * d;

            alpha = (float)(1.0 - (150.0 - d * TotalScaleX) / 150.0);
            alpha = Math.Max(alpha, 0.0f);
            alpha = Math.Min(alpha, 1.0f);

            GL.Color4(0, 0, 0, alpha / 2.0f);
            GL.Begin(BeginMode.Lines);
            for (double x = graphmin; x < graphmax + .5 * d; x += d)
            {
                int xi = (int)transformX((float)x);
                GL.Vertex2(xi, 0);
                GL.Vertex2(xi, Height);
            }
            GL.End();
            //now major
            d *= 10;
            graphmin = Math.Floor(min / d) * d;
            graphmax = Math.Ceiling(max / d) * d;
            alpha = (float)(1.0 - (150.0 - d * TotalScaleX) / 150.0);
            alpha = Math.Max(alpha, 0.0f);
            alpha = Math.Min(alpha, 1.0f);
                        
            for (double x = graphmin; x < graphmax + .5 * d; x += d)
            {
                GL.Color4(0, 0, 0, alpha / 2.0f);
                GL.Begin(BeginMode.Lines);
                int xi = (int)transformX((float)x);
                GL.Vertex2(xi, 0);
                GL.Vertex2(xi, Height);
                GL.End();
                GL.PushAttrib(AttribMask.EnableBit);
                QFontBegin();
                font.Print(string.Format("{0:G}", x), new Vector2(xi + 15, 5));
                QFontEnd();
                GL.PopAttrib();
            }
            
        }
        private void QFontBegin()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix(); //push projection matrix            
            var M = Matrix4.CreateOrthographicOffCenter(ClientRectangle.X, ClientRectangle.Width, ClientRectangle.Height, ClientRectangle.Y, -1, 1);
            GL.LoadMatrix(ref M);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();  //push modelview matrix
            GL.LoadIdentity();
        }

        private void QFontEnd()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix(); //pop modelview

            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix(); //pop projection

            GL.MatrixMode(MatrixMode.Modelview);
        }
        void drawPoints(ISeriesProjection series)
        {            
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.PointSmooth);
            float range = Math.Max(series.Xmax - series.Xmin, series.Ymax - series.Ymin);

            DensityRenderer renderer = m_DensityMap[series.Name];

            int num = (int)(Math.Ceiling(Width / m_ClutterWindow));
            num = Math.Max(num, 1);
            
            int[] grid = new int[num * num];            

            float cellsize = range / num;

            float offx = transformX(0) - (float)(Math.Floor(transformX(0) / m_ClutterWindow) * m_ClutterWindow);
            float offy = transformY(0) - (float)(Math.Floor(transformY(0) / m_ClutterWindow) * m_ClutterWindow);

            var selectedList = new List<int>(grid.Length);
            var unselectedList = new List<int>(grid.Length);
            var allList = new List<int>(grid.Length);

            if (m_ClutterWindow == 0)
            {
                allList.AddRange(series.dataPoints.Select(d=>d.Index));
                unselectedList.AddRange(series.dataPoints.Select(d => d.Index));
            }
            else
            {
                foreach (var p in m_DensityMap[series.Name].Points)
                {
                    float xgl = transformX(p.X);
                    float ygl = transformY(p.Y);

                    int ix = (int)Math.Floor((xgl - offx) / m_ClutterWindow);
                    int iy = (int)Math.Floor((ygl - offy) / m_ClutterWindow);
                    bool allow = !(ix < 0 || ix >= num || iy < 0 || iy >= num);

                    if (allow)
                    {
                        int count = grid[ix * num + iy]++;
                        allow = allow && count == 0;
                    }

                    float clutterRad = renderer.GetDist(xgl, ygl) * 2.0f;

                    if (clutterRad > m_ClutterWindow && allow)
                    {

                        allList.Add(p.Index);
                        if (p.Selected)
                        {
                            selectedList.Add(p.Index);
                        }
                        else
                        {
                            unselectedList.Add(p.Index);
                        }
                    }
                }
            }
            //use vbo???//////////////////////////////////////////////
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_VboMap[series.Name].vbo);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 2 * Vector3.SizeInBytes);            

            var unselectedVertices = unselectedList.ToArray();
            var selectedVertices = selectedList.ToArray();
            var allVertices = allList.ToArray();
            GL.PointSize(5);
            Color modulated = ColorConv.Modulate(series.Color, .9f);
            GL.Color3(modulated);
            GL.DrawElements(BeginMode.Points, unselectedVertices.Length, DrawElementsType.UnsignedInt, unselectedVertices);
            GL.Color3(Color.FromArgb(0,255,0));
            GL.DrawElements(BeginMode.Points, selectedVertices.Length, DrawElementsType.UnsignedInt, selectedVertices);

            GL.PointSize(3);
            GL.Color3(series.Color);
            GL.DrawElements(BeginMode.Points, allVertices.Length, DrawElementsType.UnsignedInt, allVertices);
            GL.DisableClientState(ArrayCap.VertexArray);            
            GL.Disable(EnableCap.DepthTest);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);            
        }
        #endregion

        #region Transforms
        private float unTransformX(float x)
        {
            return (x - m_ScreenOffsetX) / TotalScaleX - m_OffsetX;
        }
        private float unTransformY(float y)
        {
            return (y - m_ScreenOffsetY) / TotalScaleY - m_OffsetY;
        }
        //private float unTransformYGL(float y){
        //    return -((y - (Height - screenOffsetY)) / totalScaleY + offsetY);
        //}
        private float transformX(float x)
        {
            return (x + m_OffsetX) * TotalScaleX + m_ScreenOffsetX;
        }
        private float transformY(float y)
        {
            return (y + m_OffsetY) * TotalScaleY + m_ScreenOffsetY;
        }
        //private float transformYGL(float y)
        //{
        //    return (y - offsetY) * totalScaleY + (Height - screenOffsetY);
        //}        
        #endregion
        #endregion
    }
    public class VBO
    {
        public int vbo;
        public int length;
        public VBO( int v, int num)
        {
            length = num;
            vbo = v;
        }
    }
    public enum MaxMode
    {
        Global,
        PerGroup
    }
    public class Stat
    {
        public float Bandwidth { get; set; }
        public float Threshold { get; set; }
        public float DensityThreshold { get; set; }
        public long Milliseconds { get; set; }
        public int GroupNum { get; set; }
        public int PointNum { get; set; }
        public float ClutterWindow { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Stat(SplatterView view, long time, int groupN, int pointN)
        {
            Bandwidth = view.Bandwidth;
            Threshold = view.ContourThreshold;
            DensityThreshold = view.DensityThreshold;
            Milliseconds = time;
            PointNum = pointN;
            GroupNum = groupN;
            ClutterWindow = view.ClutterWindow;
            Width = view.Width;
            Height = view.Height;

        }
    }
}
