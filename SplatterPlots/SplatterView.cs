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

namespace SplatterPlots
{
    public partial class SplatterView: GLControl
    {
        #region Private Fields
        SplatterModel splatPM;

        float offsetX;
        float offsetY;
        float scaleX;
        float scaleY;
        float totalScaleX { get { return scaleX * m_scaleFactorX; } }
        float totalScaleY { get { return scaleY * m_scaleFactorY; } }

        int screenOffsetX;
        int screenOffsetY;

        bool panEnabled;
        int panOrigX;
        int panOrigY;

        float m_gain;
        float m_lowerLimit;
        float m_bandwidth;
        float m_stripePeriod;
        float m_stripeWidth;
        float m_chromaF;
        float m_lightnessF;
        float m_scaleFactorX;
        float m_scaleFactorY;
        float m_clutterWindow = 100;

        bool dClickable;

        ShaderProgram blendProgram;
        ShaderProgram glyphProgram;
        Dictionary<string, DensityRenderer> densityMap = new Dictionary<string, DensityRenderer>();

        bool fade;
        bool contours=true;
        bool loaded;
        Matrix4 cameraMatrix;
        Matrix4 ortho;
        #endregion

        #region Public Properties
        public event EventHandler ModelChanged;
        public SplatterModel Model { get { return splatPM; } }
        public float Bandwidth
        {
            get { return m_bandwidth; }
            set { m_bandwidth = value; Refresh(); }
        }
        public float ChromaF
        {
            get { return m_chromaF; }
            set { m_chromaF = value; Refresh(); }
        }
        public float ClutterWindow
        {
            get { return m_clutterWindow; }
            set { m_clutterWindow = value; Refresh(); }
        }
        public float Gain
        {
            get { return m_gain; }
            set { m_gain = value; Refresh(); }
        }
        public float LightnessF
        {
            get { return m_lightnessF; }
            set { m_lightnessF = value; Refresh(); }
        }
        public float LowerLimit
        {
            get { return m_lowerLimit; }
            set { m_lowerLimit = value; Refresh(); }
        }
        public float ScaleFactorX
        {
            get { return m_scaleFactorX; }
            set { m_scaleFactorX = value; Refresh(); }
        }
        public float ScaleFactorY
        {
            get { return m_scaleFactorY; }
            set { m_scaleFactorY = value; Refresh(); }
        }
        public float StripePeriod
        {
            get { return m_stripePeriod; }
            set { m_stripePeriod = value; Refresh(); }
        }
        public float StripeWidth
        {
            get { return m_stripeWidth; }
            set { m_stripeWidth = value; Refresh(); }
        }
        #endregion

        #region Construction
        public SplatterView()
        {
            m_scaleFactorX = 1;
            m_scaleFactorY = 1;
            m_chromaF = .95f;
            m_lightnessF = .95f;
            m_clutterWindow = 30.0f;
            m_bandwidth = 10;
            m_gain = 1;
            m_lowerLimit = 0;
            m_stripePeriod = 50;
            m_stripeWidth = 1;

            InitializeComponent();
            offsetX = 0;
            offsetY = 0;

            scaleX = 1;
            scaleY = 1;

            panEnabled = false;
            fade = true;
            this.MouseWheel += new MouseEventHandler(SplatterView_MouseWheel);
        }
        #endregion

        #region Public Methods
        private void setBBox(float xmin, float ymin, float xmax, float ymax)
        {
            int www = Width;
            //rectf = QRectF(transformX(xmin),transformY(ymin),transformX(xmax)-transformX(xmin),transformY(ymax)-transformY(ymin));

            offsetX = -(xmax + xmin) / 2.0f;
            offsetY = -(ymax + ymin) / 2.0f;

            float rangeX = (xmax - xmin);
            float rangeY = (ymax - ymin);

            float range = Math.Max(rangeX, rangeY) * 1.2f;

            scaleX = Width / range;
            scaleY = scaleX;
            m_scaleFactorX = 1;
            m_scaleFactorY = 1;

            screenOffsetX = (int)(Width / 2.0f);
            screenOffsetY = (int)(Height / 2.0f);

            Refresh();
        }
        public void setGroupEnabled(string name, bool val)
        {
            splatPM.SetEnabled(name, val);
            Refresh();
        }
        public void setSplatPM(SplatterModel spm)
        {
            screenOffsetX = Width / 2;
            screenOffsetY = Height / 2;
            splatPM = spm;

            if (densityMap.Count <= 0)
            {
                foreach (var series in splatPM.seriesList.Values)
                {
                    densityMap[series.name] = new DensityRenderer();
                }
            }
            if (loaded)
            {
                MakeCurrent();
                foreach (var denisty in densityMap.Values)
                {
                    denisty.Init(Width, Height,Context);
                }
            }
            setBBox(spm.xmin, spm.ymin, spm.xmax, spm.ymax);
            if (ModelChanged!=null)
            {
                ModelChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Private methods

        #region Event Handlers
        private void SplatterView_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                glPaint();
            }
            catch (Exception ex)
            {
                int g = 0;

            }
        }
        private void SplatterView_Load(object sender, EventArgs e)
        {
            if (!Program.Runtime)
            {
                return;
            }            

            loaded = true;
            this.MakeCurrent();
            
            GL.Enable(EnableCap.Multisample);
            GL.Disable(EnableCap.CullFace);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            string nam = this.Name;
            blendProgram = new ShaderProgram("blur.vert", "blend.frag",Context);
            //blendProgram.->setParent(this);                
            blendProgram.Link();

            blendProgram.Bind();
            blendProgram.SetUniform("u_Texture0", 0);
            blendProgram.SetUniform("u_Texture1", 1);
            blendProgram.SetUniform("u_Texture2", 2);
            blendProgram.SetUniform("u_Texture3", 3);
            blendProgram.SetUniform("u_Texture4", 4);
            blendProgram.SetUniform("u_Texture5", 5);
            blendProgram.SetUniform("u_Texture6", 6);
            blendProgram.SetUniform("u_Texture7", 7);
            blendProgram.Release();

            //glyphProgram = new ShaderProgram(this->context());	
            //glyphProgram->setParent(this);
            //glyphProgram->addShaderFromSourceFile(QGLShader::Vertex,"glyph.vert");
            //glyphProgram->addShaderFromSourceFile(QGLShader::Fragment,"glyph.frag");
            //glyphProgram->link();	

            foreach (var denisty in densityMap.Values)
            {
                denisty.Init(Width, Height,Context);
            }
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
            ortho = Matrix4.CreateOrthographicOffCenter(ClientRectangle.X, ClientRectangle.Width, ClientRectangle.Y, ClientRectangle.Height, -100, 100);
            GL.LoadMatrix(ref ortho);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        private void SplatterView_MouseDown(object sender, MouseEventArgs e)
        {
            panEnabled = true;
            panOrigX = e.X;
            panOrigY = (Height - e.Y);
        }
        private void SplatterView_MouseMove(object sender, MouseEventArgs e)
        {
            if (panEnabled)
            {
                screenOffsetX += e.X - panOrigX;
                panOrigX = e.X;

                screenOffsetY += (Height - e.Y) - panOrigY;
                panOrigY = (Height - e.Y);
            }
            Refresh();
        }
        private void SplatterView_MouseUp(object sender, MouseEventArgs e)
        {
            panEnabled = false;
        }
        private void SplatterView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dClickable)
            {
                //emit DoubleClicked(splatPM);
            }
        }
        private void SplatterView_MouseWheel(object sender, MouseEventArgs e)
        {
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

            scaleX *= 1.0f / .9f;
            scaleY *= 1.0f / .9f;

            screenOffsetX = (int)x;
            screenOffsetY = (int)y;

            offsetX = -dx;
            offsetY = -dy;

            Refresh();
        }
        private void scrollOut(float x, float y)
        {
            float dx = unTransformX(x);
            float dy = unTransformY(y);

            scaleX *= .9f;
            scaleY *= .9f;

            screenOffsetX = (int)x;
            screenOffsetY = (int)y;

            offsetX = -dx;
            offsetY = -dy;

            Refresh();
        }
        #endregion

        #region Painting
        void glPaint()
        {
            if (splatPM == null)
                return;
            this.MakeCurrent();

            int N = 0;
            int I = 0;
                        
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            foreach (var series in splatPM.seriesList.Values)
            {
                if (series.enabled)
                {
                    renderSeries(series, (float)((Math.PI / splatPM.seriesList.Count) * I));
                    N++;
                    I++;
                }
            }

            I = 0;
            foreach (var series in splatPM.seriesList.Values)
            {
                if (series.enabled)
                {
                    densityMap[series.name].BindRGB(I);
                    I++;
                }
            }

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.Blend);
            bool val = GL.IsEnabled(EnableCap.Blend);

            if (contours)
            {
                blendProgram.Bind();
                blendProgram.SetUniform("N", N);
                blendProgram.SetUniform("lf", m_lightnessF);
                blendProgram.SetUniform("cf", m_chromaF);

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

                blendProgram.Release();
            }
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, 0);            
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
            setZoomPan();

            if (splatPM.showAllPoints)
            {
                foreach (var series in splatPM.seriesList.Values)
                {
                    if (series.enabled)
                    {
                        drawPoints(series);
                    }
                }
            }
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.LoadMatrix(ref Matrix4.Identity);
            drawGrid();
            this.SwapBuffers();
        }    
        void renderSeries(SeriesProjection series, float angle)
        {
            
            GL.MatrixMode(MatrixMode.Modelview);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            blurPoints(series);

            densityMap[series.name].Bind();
            densityMap[series.name].Shade(series.color.R / 255.0f, series.color.G / 255.0f, series.color.B / 255.0f, angle, m_stripePeriod, m_stripeWidth, m_lowerLimit, m_gain);
            densityMap[series.name].UnBind();
        }
        void blurPoints(SeriesProjection series)
        {
            densityMap[series.name].Bind();
            densityMap[series.name].Clear();
            paintPoints(series);
            densityMap[series.name].Blur(m_bandwidth, m_gain);
            densityMap[series.name].UnBind();
        }
        void paintPoints(SeriesProjection series)
        {
            GL.PushMatrix();
            GL.PushAttrib(AttribMask.EnableBit);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            setZoomPan();

            
            GL.PointSize(1);
            GL.Color4(1.0f, 0, 0,1.0f);
            GL.Begin(BeginMode.Points);
            foreach (var point in series.dataPoints)
            {
                GL.Vertex2(point.X,point.Y);
            }

            GL.End();
            GL.PopAttrib();
            GL.PopMatrix();
        }
        void setZoomPan()
        {
            GL.LoadMatrix(ref Matrix4.Identity);

            GL.Translate(screenOffsetX, screenOffsetY, 0);
            GL.Scale(totalScaleX, totalScaleY, 1);
            GL.Translate(offsetX, offsetY, 0);
            //glScalef(1, -1, 1);
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

            float alpha = (float)(1.0f - (150.0f - d * scaleY) / 150.0f);
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
            alpha = (float)(1.0f - (150.0 - d * scaleY) / 150.0);
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

            alpha = (float)(1.0 - (150.0 - d * totalScaleX) / 150.0);
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
            alpha = (float)(1.0 - (150.0 - d * totalScaleX) / 150.0);
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
        void drawPoints(SeriesProjection series)
        {
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.PointSmooth);
            double range = Math.Max(series.xmax - series.xmin, series.ymax - series.ymin);

            DensityRenderer renderer = densityMap[series.name];

            int num = (int)(Math.Ceiling(Width / m_clutterWindow));
            num = Math.Max(num, 1);

            int[] grid_1 = new int[(num - 1) * (num - 1)];
            int[] grid = new int[num * num];

            double cellsize = range / num;

            float offx = transformX(0) - (float)(Math.Floor(transformX(0) / m_clutterWindow) * m_clutterWindow);
            float offy = transformY(0) - (float)(Math.Floor(transformY(0) / m_clutterWindow) * m_clutterWindow);

            GL.PointSize(5);
            GL.Color3(series.color);            
            GL.Begin(BeginMode.Points);

            for (int i = 0; i < series.dataPoints.Count; i++)
            {
                float xgl = transformX(series.dataPoints[i].X);
                float ygl = transformY(series.dataPoints[i].Y);
                int ix = (int)Math.Floor((xgl - offx) / m_clutterWindow);
                int iy = (int)Math.Floor((ygl - offy) / m_clutterWindow);
                bool allow = !(ix < 0 || ix >= num || iy < 0 || iy >= num);

                if (allow)
                {
                    int count = grid[ix * num + iy]++;
                    allow = allow && count == 0;
                }

                float clutterRad = renderer.GetDist(xgl, ygl) * 2.0f;

                if (/*splatPM->showAllPoints ||*/clutterRad > m_clutterWindow && allow)
                //if (allow)
                {

                    //GL.Color3(series.color);
                    GL.Vertex3(series.dataPoints[i].X, series.dataPoints[i].Y, series.dataZval[i]);                    
                }
            }
            GL.End();
            GL.Disable(EnableCap.DepthTest);
            //glyphProgram->release();
        }
        #endregion

        #region Transforms
        private float unTransformX(float x)
        {
            return (x - screenOffsetX) / totalScaleX - offsetX;
        }
        private float unTransformY(float y)
        {
            return (y - screenOffsetY) / totalScaleY - offsetY;
        }
        //private float unTransformYGL(float y){
        //    return -((y - (Height - screenOffsetY)) / totalScaleY + offsetY);
        //}
        private float transformX(float x)
        {
            return (x + offsetX) * totalScaleX + screenOffsetX;
        }
        private float transformY(float y)
        {
            return (y + offsetY) * totalScaleY + screenOffsetY;
        }
        //private float transformYGL(float y)
        //{
        //    return (y - offsetY) * totalScaleY + (Height - screenOffsetY);
        //}        
        #endregion
        #endregion
    }
}
