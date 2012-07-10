using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using System.Diagnostics;
using System.Drawing;

namespace SplatterPlots
{
    public static class ColorConv
    {
        #region contruction
        static Random random;
        static ColorConv()
        {
            random = new Random();
        }
        #endregion

        #region public
        public static Vector3 XYZtoLRGB(Vector3 xyz, bool clamp)
        {
            Vector3 M0 = new Vector3(3.2404542f, -1.5371385f, -0.4985314f);
            Vector3 M1 = new Vector3(-0.9692660f, 1.8760108f, 0.0415560f);
            Vector3 M2 = new Vector3(0.0556434f, -0.2040259f, 1.0572252f);

            float r = Vector3.Dot(xyz, M0);
            float g = Vector3.Dot(xyz, M1);
            float b = Vector3.Dot(xyz, M2);

            if (clamp)
            {
                r = Math.Min(Math.Max(r, 0.0f), 1.0f);
                g = Math.Min(Math.Max(g, 0.0f), 1.0f);
                b = Math.Min(Math.Max(b, 0.0f), 1.0f);
            }

            return new Vector3(r, g, b);
        }
        public static Vector3 LRGBtoXYZ(Vector3 lrgb)
        {
            Vector3 M0 = new Vector3(0.4124564f, 0.3575761f, 0.1804375f);
            Vector3 M1 = new Vector3(0.2126729f, 0.7151522f, 0.0721750f);
            Vector3 M2 = new Vector3(0.0193339f, 0.1191920f, 0.9503041f);

            return new Vector3(Vector3.Dot(lrgb, M0), Vector3.Dot(lrgb, M1), Vector3.Dot(lrgb, M2));
        }
        public static Vector3 XYZtoLAB(Vector3 xyz)
        {
            float Xr = 0.95047f;
            float Yr = 1.0f;
            float Zr = 1.08883f;

            float eps = 216.0f / 24389.0f;
            float k = 24389.0f / 27.0f;

            float xr = (float)(xyz.X / Xr);
            float yr = (float)(xyz.Y / Yr);
            float zr = (float)(xyz.Z / Zr);

            xr = f(xr, eps, k);
            yr = f(yr, eps, k);
            zr = f(zr, eps, k);

            float L = 116 * yr - 16;
            float a = 500 * (xr - yr);
            float b = 200 * (yr - zr);

            return new Vector3(L, a, b);
        }
        public static Vector3 LABtoXYZ(Vector3 lab)
        {
            float Xr = 0.95047f;
            float Yr = 1.0f;
            float Zr = 1.08883f;

            float eps = 216.0f / 24389.0f;
            float k = 24389.0f / 27.0f;

            float L = (float)lab.X;
            float a = (float)lab.Y;
            float b = (float)lab.Z;

            float fy = (L + 16.0f) / 116.0f;
            float fx = a / 500.0f + fy;
            float fz = -b / 200.0f + fy;

            float xr = (float)((Math.Pow(fx, 3.0f) > eps) ? Math.Pow(fx, 3.0f) : (116.0f * fx - 16.0f) / k);
            float yr = (float)((L > (k * eps)) ? Math.Pow(((L + 16.0f) / 116.0f), 3.0f) : L / k);
            float zr = (float)((Math.Pow(fz, 3.0f) > eps) ? Math.Pow(fz, 3.0f) : (116.0f * fz - 16.0f) / k);

            float X = xr * Xr;
            float Y = yr * Yr;
            float Z = zr * Zr;

            return new Vector3(X, Y, Z);
        }
        public static Vector3 LABtoLCH(Vector3 lab)
        {
            float l = (float)lab.X;
            float a = (float)lab.Y;
            float b = (float)lab.Z;

            float C = (float)Math.Sqrt(a * a + b * b);
            float H = (float)Math.Atan2(b, a);

            return new Vector3(l, C, H);
        }
        public static Vector3 LCHtoLAB(Vector3 lch)
        {
            float l = (float)lch.X;
            float c = (float)lch.Y;
            float h = (float)lch.Z;

            return new Vector3(l, (float)(c * Math.Cos(h)), (float)(c * Math.Sin(h)));
        }
        public static Vector3 RGBtoLAB(Vector3 rgb)
        {
            return XYZtoLAB(LRGBtoXYZ(rgb));
        }
        public static Vector3 LABtoRGB(Vector3 lab, bool clamp)
        {
            return XYZtoLRGB(LABtoXYZ(lab), clamp);
        }

        public static Vector3 PerceptualLerpRGB(Vector3 rgb1, Vector3 rgb2, float alpha)
        {
            return LABtoRGB(lerp(RGBtoLAB(rgb1), RGBtoLAB(rgb2), alpha), true);
        }
        public static Vector3 PerceptualBlendRGB(List<Vector3> rgbs, float cf, float lf)
        {
            if (rgbs.Count == 1)
                return rgbs[0];

            float x = 0;
            float y = 0;
            float z = 0;

            /*for(unsigned int i=0;i<rgbs.size();i++){
                x+= rgbs[i].x();
                y+= rgbs[i].y();
                z+= rgbs[i].z();
            }
            Color rgb(x/rgbs.size(),y/rgbs.size(),z/rgbs.size());
            return rgb;
            */

            float Cdec = cf;
            float Ldec = lf;

            for (int i = 0; i < rgbs.Count; i++)
            {
                Vector3 lab = RGBtoLAB(rgbs[i]);

                x += (float)lab.X;
                y += (float)lab.Y;
                z += (float)lab.Z;
                Cdec *= Cdec;
                Ldec *= Ldec;
            }
            Vector3 newLab = new Vector3(x / rgbs.Count, y / rgbs.Count, z / rgbs.Count);
            Vector3 newLch = LABtoLCH(newLab);
            newLch.Y = newLch.Y * Cdec;
            newLch.X = newLch.X * Ldec;

            return LABtoRGB(LCHtoLAB(newLch), true);
        }
        public static List<Color> pickIsoCols(float L, int num, float angle, float angleOffset)
        {
            var resultRGB = new List<Color>();
            if (angle < 0)
                angle = uniform(0.0f, (float)Math.PI * 2.0f);
            while (angle < 0)
            {
                angle += (float)Math.PI * 2.0f;
            }
            while (angle > (float)Math.PI * 2.0f)
            {
                angle -= (float)Math.PI * 2.0f;
            }

            float spacing = (angleOffset * 2) / num;

            for (int i = 0; i < num; i++)
            {
                float H = spacing * i + angle - angleOffset;
                float C = findMaxC(L, H);

                float a = (float)(C * Math.Cos(H));
                float b = (float)(C * Math.Sin(H));

                Vector3 rgb = LABtoRGB(new Vector3(L, a, b), true);
                var color = Color.FromArgb((int)(rgb.X * 255), (int)(rgb.Y * 255), (int)(rgb.Z * 255));
                resultRGB.Add(color);
            }
            return resultRGB;
        }
        #endregion

        #region private
        private static Vector3 clamp(Vector3 col, float minV, float maxV)
        {
            col.X = Math.Min(Math.Max(col.X, minV), maxV);
            col.Y = Math.Min(Math.Max(col.Y, minV), maxV);
            col.Z = Math.Min(Math.Max(col.Z, minV), maxV);            

            return col;
        }
        private static Vector3 collapseCtoRGBRange(Vector3 lab)
        {
            Vector3 rgb = LABtoRGB(lab, true);
            rgb = clamp(rgb, 0.0f, 1.0f);
            return RGBtoLAB(rgb);
        }
        private static float clamp(float num, float minV, float maxV)
        {
            return Math.Min(Math.Max(num, minV), maxV);
        }
        private static float uniform(float min, float max)
        {
            float range = max - min;
            float r = (float)(random.NextDouble());
            return (r * range + min);//BUG fix the rand
        }
        private static Vector3 snapToRange(Vector3 lab, float angle, float angleOffset)
        {
            Vector3 rgb = LABtoRGB(lab, true);
            Vector3 val = RGBtoLAB(clamp(rgb, 0.0f, 1.0f));
            Vector3 lch = LABtoLCH(val);

            if (lch.Y < 20.0f) lch.Y = 20.0f;
            if (lch.X < 50.0f) lch.X = 50.0f;

            while (lch.Z < 0)
            {
                lch.Z += (float)Math.PI * 2.0f;
            }
            while (lch.Z > (float)Math.PI * 2.0f)
            {
                lch.Z -= (float)Math.PI * 2.0f;
            }

            float d0 = (float)Math.Abs(angle - (lch.Z));
            float dp = (float)Math.Abs(angle - (lch.Z + 2 * Math.PI));
            float dn = (float)Math.Abs(angle - (lch.Z - 2 * Math.PI));

            if (dp < d0 && dp < dn)
            {
                lch.Z += (float)Math.PI * 2.0f;
            }
            else if (dn < d0 && dn < dp)
            {
                lch.Z -= (float)Math.PI * 2.0f;
            }
            lch.Z = clamp((float)lch.Z, angle - angleOffset, angle + angleOffset);

            val = LCHtoLAB(lch);
            return val;
        }
        private static bool LCHInRange(Vector3 lch)
        {
            Vector3 rgb = LABtoRGB(LCHtoLAB(lch), false);
            return rgb.X >= 0.0f && rgb.X <= 1.0f &&
                    rgb.Y >= 0.0f && rgb.Y <= 1.0f &&
                    rgb.Z >= 0.0f && rgb.Z <= 1.0f;

        }
        private static float findMaxC(float L, float h)
        {
            Vector3 lch1 = new Vector3(L, 0, h);
            Vector3 lch2 = new Vector3(L, 150, h);

            Debug.Assert(LCHInRange(lch1));
            Debug.Assert(!LCHInRange(lch2));

            float inv = 0;
            float outv = 100;
            float mid = 0;

            for (int i = 0; i < 10; i++)
            {
                mid = (inv + outv) / 2;
                Vector3 temp = new Vector3(L, mid, h);
                if (LCHInRange(temp))
                {
                    inv = mid;
                }
                else
                {
                    outv = mid;
                }
            }
            return inv;
        }
        private static float lerp(float one, float two, float alpha)
        {
            return two * alpha + one * (1.0f - alpha);
        }
        private static Vector3 lerp(Vector3 one, Vector3 two, float alpha)
        {
            return new Vector3(lerp((float)one.X, (float)two.X, alpha), lerp((float)one.Y, (float)two.Y, alpha), lerp((float)one.Z, (float)two.Z, alpha));
        }
        private static float f(float n, float eps, float k)
        {
            if (n > eps)
            {
                return (float)Math.Pow(n, 1.0f / 3.0f);
            }
            else
            {
                return (k * n + 16.0f) / 116.0f;
            }
        }
        #endregion
    }
}
