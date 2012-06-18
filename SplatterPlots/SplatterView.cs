using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace SplatterPlots
{
    public partial class SplatterView : GLControl
    {
        #region fields
        float offsetX;
        float offsetY;
        float scaleX;
        float scaleY;
        float scaleFactorX;
        float scaleFactorY;
        float totalScaleX { get { return scaleX * scaleFactorX; } }
        float totalScaleY { get { return scaleY * scaleFactorY; } }

        int screenOffsetX;
        int screenOffsetY;

        bool panEnabled;
        int panOrigX;
        int panOrigY;

        float gain;
        float lowerLimit;
        float bandwidth;
        float stripePeriod;
        float stripeWidth;

        float chromaF;
        float lightnessF;
        #endregion

        public float clutterWindow = 100;
        private bool fade;
        private bool contours;
        private bool loaded;
        private Matrix4 cameraMatrix;
        private Matrix4 ortho;

        public SplatterView()
        {
            InitializeComponent();
                   offsetX = 0;
            offsetY = 0;

            scaleX = 1;
            scaleY = 1;
            scaleFactorX = 1;
            scaleFactorY = 1;	

            chromaF = .95f;
            lightnessF = .95f;

            panEnabled = false;

            clutterWindow = 30.0f;
            fade = true;

            //QString filename = "map.svg";
            //usMap = new QSvgRenderer(filename);
            //bandwidth = 10;
            //gain = 1;
            //lowerLimit = 0;

        }

        void setBBox(float xmin, float ymin, float xmax, float ymax)
        {
            int www = Width;
            //rectf = QRectF(transformX(xmin),transformY(ymin),transformX(xmax)-transformX(xmin),transformY(ymax)-transformY(ymin));

            offsetX = -(xmax + xmin) / 2.0f;
            offsetY = (ymax + ymin) / 2.0f;

            float rangeX = (xmax - xmin);
            float rangeY = (ymax - ymin);

            float range = Math.Max(rangeX, rangeY) * 1.2f;

            scaleX = Width / range;
            scaleY = scaleX;
            scaleFactorX = 1;
            scaleFactorY = 1;

            screenOffsetX = (int)(Width / 2.0f);
            screenOffsetY = (int)(Height / 2.0f);

            Refresh();
        }

         void glPaint() {
            if(splatPM==shared_ptr<SplatGL>())
                return;
            this.MakeCurrent();

            //glBlendFunc(GL_ONE,GL_ONE);
            int N = 0;
            for(int i=0;i<splatPM->seriesList.size();i++){
                if(splatPM->seriesList[i]->enabled){
                    string temp = splatPM->seriesList[i]->name;
                    QString name = QString::fromStdString(temp);
                    renderSeries(splatPM->seriesList[i],(M_PI/splatPM->seriesList.size())*i);	
                    N++;
                }
            }

            int I = 0;
            for(int i=0;i<splatPM->seriesList.size();i++){
                if(splatPM->seriesList[i]->enabled){
                    string temp = splatPM->seriesList[i]->name;
                    QString name = QString::fromStdString(temp);		
                    densityMap[name]->BindRGB(I);
                    I++;
                }
            }
            glClearColor(1.0f,1.0f,1.0f,1.0f);
            glClear(GL_DEPTH_BUFFER_BIT|GL_COLOR_BUFFER_BIT);
            glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
            if(this->contours){
                //int N = splatPM->seriesList.size();
                blendProgram->bind();
                blendProgram->setUniformValue("N",N);
                blendProgram->setUniformValue("lf",lightnessF);
                blendProgram->setUniformValue("cf",chromaF);		

                QMatrix4x4 imat;
                glLoadMatrixd(imat.constData());
                glColor4f(1,1,1,1);
                glBegin(GL_QUADS);
                    glTexCoord2f(0, 0);
                    glVertex2f(0, 0);
                    glTexCoord2f(1, 0);
                    glVertex2f(width(), 0);
                    glTexCoord2f(1, 1);
                    glVertex2f(width(), height());
                    glTexCoord2f(0, 1);
                    glVertex2f(0, height());
                glEnd();	

                blendProgram->release();
            }
            glActiveTexture(GL_TEXTURE0);
            glBindTexture(GL_TEXTURE_2D,0);

            glMatrixMode(GL_MODELVIEW);	
            glDisable(GL_BLEND);
            glDisable(GL_TEXTURE_2D);
            this->setZoomPan();

            if(this->splatPM->showAllPoints){
                for(int i=0;i<splatPM->seriesList.size();i++){
                    if(splatPM->seriesList[i]->enabled){
                        drawPoints(splatPM->seriesList[i]);	
                    }
                }
            }
            glEnable(GL_BLEND);
            glEnable(GL_TEXTURE_2D);
        }


        private void SView_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                //Bitmap bmp = new Bitmap(Width,Height);
                //Graphics graphics = Graphics.FromImage(bmp);
                //paint(graphics);
                //e.Graphics.DrawImageUnscaled(bmp, 0, 0);
                glPaint();
            }
            catch (Exception ex)
            {
                int g = 0;

            }
        }
        private void SView_Load(object sender, EventArgs e)
        {
            loaded = true;

            GL.Enable(EnableCap.Multisample);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            
            GL.Enable(EnableCap.Texture2D);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            //    QString nam = this->objectName();
            //    blendProgram = new QGLShaderProgram(this->context());
            //    int ID = blendProgram->programId();
            //    blendProgram->setParent(this);
            //    blendProgram->addShaderFromSourceFile(QGLShader::Vertex,"blur.vert");
            //    blendProgram->addShaderFromSourceFile(QGLShader::Fragment,"blend.frag");
            //    blendProgram->link();	

            //    blendProgram->bind();
            //    blendProgram->setUniformValue("u_Texture0",0);
            //    blendProgram->setUniformValue("u_Texture1",1);
            //    blendProgram->setUniformValue("u_Texture2",2);
            //    blendProgram->setUniformValue("u_Texture3",3);
            //    blendProgram->setUniformValue("u_Texture4",4);
            //    blendProgram->setUniformValue("u_Texture5",5);
            //    blendProgram->setUniformValue("u_Texture6",6);
            //    blendProgram->setUniformValue("u_Texture7",7);
            //    blendProgram->release();

            //    glyphProgram = new QGLShaderProgram(this->context());	
            //    glyphProgram->setParent(this);
            //    glyphProgram->addShaderFromSourceFile(QGLShader::Vertex,"glyph.vert");
            //    glyphProgram->addShaderFromSourceFile(QGLShader::Fragment,"glyph.frag");
            //    glyphProgram->link();	

            //    QHash<QString,shared_ptr<DensityRenderer>>::Iterator iter = densityMap.begin();
            //    for(;iter!=densityMap.end();iter++){
            //        iter.value()->Init(context(),width(),height());
            //    }	

        }
        private void SView_Resize(object sender, EventArgs e)
        {
            this.MakeCurrent();
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            GL.MatrixMode(MatrixMode.Projection);
            ortho = Matrix4.CreateOrthographicOffCenter(ClientRectangle.X, ClientRectangle.Width, ClientRectangle.Y, ClientRectangle.Height, -100, 100);
            GL.LoadMatrix(ref ortho);
            GL.MatrixMode(MatrixMode.Modelview);
        }


        //void RenderArea1::setGroupEnabled(string name, bool val){
        //    splatPM->SetEnabled(name, val);
        //    update();
        //}
        //void RenderArea1::setSplatPM(shared_ptr<SplatGL> spm){
        //    int www = width();
        //    QPSIZE1 = width();
        //    screenOffsetX = QPSIZE1/2.0;
        //    screenOffsetY = QPSIZE1/2.0;
        //    path = QPainterPath();
        //    path.moveTo(0,0);
        //    path.lineTo(QPSIZE1, 0);
        //    path.lineTo(QPSIZE1,QPSIZE1);
        //    path.lineTo(0, QPSIZE1);
        //    path.lineTo(0, 0);
        //    path.closeSubpath();

        //    splatPM = spm;
        //    if(densityMap.size()<=0){
        //        for(unsigned int i=0;i<splatPM->seriesList.size();i++){
        //            string temp = splatPM->seriesList[i]->name;
        //            QString name = QString::fromStdString(temp);        
        //            densityMap[name] = make_shared<DensityRenderer>();		
        //        }
        //    }
        //}
        

        ////void RenderArea1::setBlendFactors(float Cf, float Lf){
        ////    chromaF = Cf;
        ////    lightnessF = Lf;
        ////    update();
        ////}


        ////void RenderArea1::setBackCol(float f){
        ////    backCol = QColor::fromRgbF(f,f,f);
        ////    update();
        ////}

        
        


        
        //void RenderArea1::renderSeries(shared_ptr<SeriesProjection> series, float angle) {
        //    QString name = QString::fromStdString(series->name);
        //    glMatrixMode(GL_MODELVIEW);
        //    glClearColor(0,0,0,0);
        //    glClear(GL_DEPTH_BUFFER_BIT|GL_COLOR_BUFFER_BIT);

        //    blurPoints(series);

        //    densityMap[name]->Bind();
        //    densityMap[name]->Shade(series->color.x(),series->color.y(),series->color.z(),angle,stripePeriod,stripeWidth,lowerLimit,gain);
        //    densityMap[name]->UnBind();
        //}
        //void RenderArea1::blurPoints(shared_ptr<SeriesProjection> series){
        //    QString name = QString::fromStdString(series->name);
        //    densityMap[name]->Bind();
        //    densityMap[name]->Clear();
        //    paintPoints(series);
        //    densityMap[name]->Blur(bandwidth,gain);
        //    densityMap[name]->UnBind();
        //}
        //void RenderArea1::paintPoints(shared_ptr<SeriesProjection> series){	
        //    glMatrixMode(GL_MODELVIEW);
        //    glClearColor(0,0,0,0);
        //    glClear(GL_DEPTH_BUFFER_BIT|GL_COLOR_BUFFER_BIT);
        //    this->setZoomPan();

        //    glPushMatrix();
        //    glPointSize(1);
        //    glColor3f(1,0,0);
        //    glBegin(GL_POINTS);
        //    for(int k=0;k<series->dataPoints.size();k++){
        //        const MathVec2D<float> &xy = series->dataPoints[k];

        //        float x = xy.x();
        //        float y = xy.y();
        //        glVertex2f(x,y);
        //    }

        //    glEnd();
        //    glPopMatrix();
        //}
        //void RenderArea1::setZoomPan(){
        //    QMatrix4x4 imat;
        //    glLoadMatrixd(imat.constData());

        //    glTranslatef(screenOffsetX, height()-screenOffsetY, 0);
        //    glScalef(totalScaleX(), totalScaleY(), 1);
        //    glTranslatef(offsetX, -offsetY, 0);
        //    //glScalef(1, -1, 1);
        //}
        //void RenderArea1::bandwidthChanged(int value){
        //    bandwidth = value;
        //    update();
        //}
        //void RenderArea1::gainChanged(int value){
        //    gain = value/100.0f;
        //    update();
        //}
        //void RenderArea1::lowerLimitChanged(int value){
        //    lowerLimit = value/100.f;
        //    update();
        //}
        //void RenderArea1::stripePeriodChanged(int value){
        //    stripePeriod = value;
        //    update();
        //}
        //void RenderArea1::stripeWidthChanged(int value){
        //    stripeWidth = value;
        //    update();
        //}
        //void RenderArea1::paintEvent(QPaintEvent * event){
        //    paintGL();//////////////////////////////////////////////


        //    float llon = transformX(-125.5f);
        //    float hlon = transformX(-66.5f);
        //    float llat = transformY(24.2f);
        //    float hlat = transformY(49.8f);

        //    QRectF rf(llon,hlat,hlon-llon,llat-hlat);
        //    QPainter painter(this);     
        //    painter.setRenderHint(QPainter::Antialiasing);
        //    //usMap->render(&painter,rf);
        //    //QGLWidget::paintEvent(event);
        //    //paintGL();//////////////////////////////////////////////
        //    //glEnable(GL_MULTISAMPLE);    




        //    paint(painter);
        //    painter.end();
        //}
        //void RenderArea1::paint(QPainter &painter){
        //    painter.setBackground(QBrush(backCol));
        //    penColor = QColor::fromRgb(0,0,0);
        //    painter.setPen(QPen(penColor, 3, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));     
        //    painter.setBrush(Qt::NoBrush);
        //    painter.drawPath(path);

        //    drawGrid(painter);

        //     // draw PM
        //    if(splatPM != shared_ptr<SplatGL>()){

        //        for(unsigned int i=0; i<splatPM->seriesList.size();i++){
        //            // draw Series
        //            shared_ptr<SeriesProjection> series = splatPM->seriesList[i];            
        //            //drawPoints(series,painter);
        //        }
        //    }

        //    //painter.drawRect(rectf);
        //}

        //void RenderArea1::drawGrid(QPainter &painter){
        //    QFont font = painter.font();
        //    font.setPointSize(12);
        //    painter.setFont(font);

        //    //do minor lines first
        //    float min = this->unTransformY(QPSIZE1);
        //    float max = this->unTransformY(0);

        //    int exp = floor(log10(max-min));
        //    double d = pow(10.0,exp);
        //    d*=.1;
        //    //if((max-min)/d < 3) d*=.1;

        //    double graphmin = floor(min/d)*d;
        //    double graphmax = ceil(max/d)*d;

        //    float alpha = 1.f - (150.0 - d*scaleY)/150.0;
        //    alpha = (std::max)(alpha,0.f);
        //    alpha = (std::min)(alpha,1.f);

        //    painter.setPen(QPen(QColor::fromRgbF(0,0,0,alpha/2.0), 1.5, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //    for(double y = graphmin;y<graphmax+.5*d;y+=d){
        //        int yi = transformY(y);
        //        painter.drawLine(0,yi,QPSIZE1,yi);
        //    }
        //    //now major
        //    d*=10;
        //    graphmin = floor(min/d)*d;
        //    graphmax = ceil(max/d)*d;
        //    alpha = 1.f - (150.0 - d*scaleY)/150.0;
        //    alpha = (std::max)(alpha,0.f);
        //    alpha = (std::min)(alpha,1.f);

        //    painter.setPen(QPen(QColor::fromRgbF(0,0,0,alpha/2.0), 1.5, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //    for(double y = graphmin;y<graphmax+.5*d;y+=d){
        //        int yi = transformY(y);
        //        painter.drawLine(0,yi,QPSIZE1,yi);
        //        QString text;
        //        text.setNum(y);

        //        painter.drawText(15,yi,text);
        //    }
        //    ///////////////////////////////////////////////////////////////

        //    ////do minor lines first
        //    min = this->unTransformX(0);
        //    max = this->unTransformX(QPSIZE1);

        //    exp = floor(log10(max-min));
        //    d = pow(10.0,exp);
        //    d*=.1;
        //    //if((max-min)/d < 3) d*=.1;

        //    graphmin = floor(min/d)*d;
        //    graphmax = ceil(max/d)*d;

        //    alpha = 1.f - (150.0 - d*totalScaleX())/150.0;
        //    alpha = (std::max)(alpha,0.f);
        //    alpha = (std::min)(alpha,1.f);

        //    painter.setPen(QPen(QColor::fromRgbF(0,0,0,alpha/2.0), 1.5, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //    for(double x = graphmin;x<graphmax+.5*d;x+=d){
        //        int xi = transformX(x);
        //        painter.drawLine(xi,0,xi,QPSIZE1);
        //    }
        //    //now major
        //    d*=10;
        //    graphmin = floor(min/d)*d;
        //    graphmax = ceil(max/d)*d;
        //    alpha = 1.f - (150.0 - d*totalScaleX())/150.0;
        //    alpha = (std::max)(alpha,0.f);
        //    alpha = (std::min)(alpha,1.f);

        //    painter.setPen(QPen(QColor::fromRgbF(0,0,0,alpha/2.0), 1.5, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //    for(double x = graphmin;x<graphmax+.5*d;x+=d){
        //        int xi = transformX(x);
        //        painter.drawLine(xi,0,xi,QPSIZE1);
        //        QString text;
        //        text.setNum(x);

        //        painter.drawText(xi+15,15,text);
        //    }

        //}
        //void RenderArea1::drawPoints(shared_ptr<SeriesProjection> series, QPainter &painter){
        //    double range = max(series->xmax - series->xmin, series->ymax - series->ymin);

        //    QString name = QString::fromStdString(series->name);
        //    shared_ptr<DensityRenderer> renderer = densityMap[name];

        //    int num = (int)(ceil(width()/clutterWindow));
        //    num = max(num,1);

        //    vector<int> grid_1((num-1)*(num-1),0);
        //    vector<int> grid(num*num,0);
        //    int count = 0;
        //    double cellsize = range/num;

        //    float offx = transformX(0) - floor(transformX(0)/clutterWindow)*clutterWindow;
        //    float offy = transformYGL(0) - floor(transformYGL(0)/clutterWindow)*clutterWindow;	

        //    for(unsigned int i=0;i<series->dataPoints.size();i++){  
        //        float xgl = transformX(series->dataPoints[i].x());
        //        float ygl = transformYGL(series->dataPoints[i].y());
        //        int ix = (int)floor((xgl-offx)/clutterWindow);
        //        int iy = (int)floor((ygl-offy)/clutterWindow);
        //        bool allow = !(ix<0||ix>=num||iy<0||iy>=num);

        //        if(allow){
        //            int count = grid[ix*num + iy]++;
        //            allow = allow && count == 0;
        //        }

        //        float clutterRad = renderer->GetDist(xgl, ygl)*2.f;

        //        if(/*splatPM->showAllPoints ||*/clutterRad > clutterWindow && allow){


        //            float x = transformX(series->dataPoints[i].x());
        //            float y = transformY(series->dataPoints[i].y());
        //            /*float x = ix*clutterWindow + clutterWindow/2 + offx;
        //            float y = height() - (iy*clutterWindow + clutterWindow/2 + offy);*/

        //            //if(splatPM->clutterRedux){        


        //                //if(series->dataZval[i]<=num){
        //                    QColor col = QColor::fromRgbF(series->color[0],series->color[1],series->color[2]);
        //                    painter.setPen(QPen(QColor::fromRgbF(0,0,0), 1, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //                    painter.setBrush(QBrush(col));
        //                    painter.drawEllipse(x-5,y-5,10,10);
        //                /*}else if(fade && series->dataZval[i]<=num*2){
        //                    float alpha = 1.f - (series->dataZval[i]-num)/(num);
        //                    alpha = max(alpha,0.f);
        //                    alpha = min(alpha,1.f);
        //                    QColor col = QColor::fromRgbF(series->color[0],series->color[1],series->color[2],alpha);
        //                    painter.setPen(QPen(QColor::fromRgbF(0,0,0,alpha), 1, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //                    painter.setBrush(QBrush(col));
        //                    float dia = (5+5*alpha);
        //                    painter.drawEllipse(x - dia/2.f,y - dia/2.f,dia,dia);
        //                }*/
        //            /*}else{
        //                QColor col = QColor::fromRgbF(series->color[0],series->color[1],series->color[2]);
        //                painter.setPen(QPen(QColor::fromRgbF(0,0,0), 1, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //                painter.setBrush(QBrush(col));
        //                painter.drawEllipse(x-5,y-5,10,10);
        //            }*/
        //        }
        //    }
        //    glEnd();	
        //}
        //void RenderArea1::drawPoints(shared_ptr<SeriesProjection> series){
        //    glEnable(GL_DEPTH_TEST);
        //    double range = max(series->xmax - series->xmin, series->ymax - series->ymin);

        //    QString name = QString::fromStdString(series->name);
        //    shared_ptr<DensityRenderer> renderer = densityMap[name];

        //    int num = (int)(ceil(width()/clutterWindow));
        //    num = max(num,1);

        //    vector<int> grid_1((num-1)*(num-1),0);
        //    vector<int> grid(num*num,0);
        //    int count = 0;
        //    double cellsize = range/num;

        //    float offx = transformX(0) - floor(transformX(0)/clutterWindow)*clutterWindow;
        //    float offy = transformYGL(0) - floor(transformYGL(0)/clutterWindow)*clutterWindow;	

        //    /*glyphProgram->bind();
        //    glyphProgram->setUniformValue("Brush",series->color.x(),series->color.y(),series->color.z());
        //    glyphProgram->setUniformValue("Pen",0.0,0.0,0.0);*/

        //    glPointSize(3);
        //    glColor3f(series->color.x(),series->color.y(),series->color.z());
        //    glBegin(GL_POINTS);

        //    for(unsigned int i=0;i<series->dataPoints.size();i++){  
        //        float xgl = transformX(series->dataPoints[i].x());
        //        float ygl = transformYGL(series->dataPoints[i].y());
        //        int ix = (int)floor((xgl-offx)/clutterWindow);
        //        int iy = (int)floor((ygl-offy)/clutterWindow);
        //        bool allow = !(ix<0||ix>=num||iy<0||iy>=num);

        //        if(allow){
        //            int count = grid[ix*num + iy]++;
        //            allow = allow && count == 0;
        //        }

        //        float clutterRad = renderer->GetDist(xgl, ygl)*2.f;

        //        if(/*splatPM->showAllPoints ||*/clutterRad > clutterWindow && allow){

        //            glColor3f(series->color.x(),series->color.y(),series->color.z());
        //            glVertex3f(series->dataPoints[i].x(),series->dataPoints[i].y(),series->dataZval[i]);
        //            //float x = transformX(series->dataPoints[i].x());
        //            //float y = transformY(series->dataPoints[i].y());
        //            //float x = ix*clutterWindow + clutterWindow/2 + offx;
        //            //float y = height() - (iy*clutterWindow + clutterWindow/2 + offy);

        //            //if(splatPM->clutterRedux){        


        //                //if(series->dataZval[i]<=num){
        //                    /*QColor col = QColor::fromRgbF(series->color[0],series->color[1],series->color[2]);
        //                    painter.setPen(QPen(QColor::fromRgbF(0,0,0), 1, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //                    painter.setBrush(QBrush(col));
        //                    painter.drawEllipse(x-5,y-5,10,10);*/
        //                /*}else if(fade && series->dataZval[i]<=num*2){
        //                    float alpha = 1.f - (series->dataZval[i]-num)/(num);
        //                    alpha = max(alpha,0.f);
        //                    alpha = min(alpha,1.f);
        //                    QColor col = QColor::fromRgbF(series->color[0],series->color[1],series->color[2],alpha);
        //                    painter.setPen(QPen(QColor::fromRgbF(0,0,0,alpha), 1, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //                    painter.setBrush(QBrush(col));
        //                    float dia = (5+5*alpha);
        //                    painter.drawEllipse(x - dia/2.f,y - dia/2.f,dia,dia);
        //                }*/
        //            /*}else{
        //                QColor col = QColor::fromRgbF(series->color[0],series->color[1],series->color[2]);
        //                painter.setPen(QPen(QColor::fromRgbF(0,0,0), 1, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
        //                painter.setBrush(QBrush(col));
        //                painter.drawEllipse(x-5,y-5,10,10);
        //            }*/
        //        }
        //    }
        //    glEnd();
        //    glDisable(GL_DEPTH_TEST);
        //    //glyphProgram->release();
        //}
        //void RenderArea1::mousePressEvent(QMouseEvent *event){
        //    panEnabled = true;
        //    panOrigX = event->x();
        //    panOrigY = event->y();

        //    event->accept();
        //}
        //void RenderArea1::mouseReleaseEvent(QMouseEvent *event){
        //    panEnabled = false;    
        //    event->accept();
        //}
        //void RenderArea1::mouseDoubleClickEvent(QMouseEvent *event){
        //    if(dClickable){
        //        emit DoubleClicked(splatPM);
        //    }
        //}
        //void RenderArea1::mouseMoveEvent(QMouseEvent *event){
        //    if(panEnabled){        
        //        screenOffsetX +=  event->x()-panOrigX;
        //        panOrigX = event->x();

        //        screenOffsetY +=  event->y()-panOrigY;		
        //        panOrigY = event->y();
        //    }
        //    update();
        //    event->accept();
        //}
        //float RenderArea1::unTransformX(float x){
        //    return (x - screenOffsetX)/totalScaleX() - offsetX;
        //}
        //float RenderArea1::unTransformY(float y){
        //    return -((y - screenOffsetY)/totalScaleY() - offsetY);
        //}
        ///*float RenderArea1::unTransformYGL(float y){
        //    return -((y - (height()-screenOffsetY))/totalScaleY() + offsetY);
        //}*/
        //float RenderArea1::transformX(float x){
        //    return (x+offsetX)*totalScaleX()+screenOffsetX;
        //}
        //float RenderArea1::transformYGL(float y){
        //    return (y-offsetY)*totalScaleY()+(height()-screenOffsetY);	
        //}
        //float RenderArea1::transformY(float y){
        //    return (-y+offsetY)*totalScaleY()+screenOffsetY;    
        //}
        //void RenderArea1::scaleFactorXChanged(int value){
        //    float v = value/10.f;
        //    scaleFactorX = pow((1.0f/.9f),v);
        //    update();
        //} 
        //void RenderArea1::scaleFactorYChanged(int value){
        //    float v = value/10.f;
        //    scaleFactorY = pow((1.0f/.9f),v);
        //    update();
        //}
        //void RenderArea1::chromaFactorChanged(int value){
        //    float v = value/100.0f;
        //    chromaF = v;
        //    update();
        //}
        //void RenderArea1::lightnessFactorChanged(int value){
        //    float v = value/100.0f;
        //    lightnessF = v;
        //    update();
        //}
        //void RenderArea1::scrollIn(float x, float y){
        //    float dx = unTransformX(x);
        //    float dy = unTransformY(y);

        //    scaleX *= 1.f/.9f;
        //    scaleY *= 1.f/.9f;

        //    screenOffsetX = x;
        //    screenOffsetY = y;

        //    offsetX = -dx;
        //    offsetY = dy;

        //    update();
        //}
        //void RenderArea1::scrollOut(float x, float y){
        //    float dx = unTransformX(x);
        //    float dy = unTransformY(y);

        //    scaleX *= .9f;
        //    scaleY *= .9f;

        //    screenOffsetX = x;
        //    screenOffsetY = y;

        //    offsetX = -dx;
        //    offsetY = dy;

        //    update();
        //}

        //void RenderArea1::wheelEvent(QWheelEvent *event){
        //    int numDegrees = event->delta() / 8;
        //    int numSteps = numDegrees / 15;

        //    if (event->orientation() == Qt::Vertical) {        
        //        if(numDegrees < 0){
        //            scrollOut(event->x(),event->y());			
        //        }else{
        //            scrollIn(event->x(),event->y());			
        //        }
        //    } 
        //    event->accept();

        //}


        //    }
    }
}
