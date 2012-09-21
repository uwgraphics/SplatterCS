#version 150 compatibility

uniform sampler2D u_Texture0;
uniform sampler2D DistText1;
uniform vec3 rgbCol;
uniform float angle;
uniform float maxVal;
uniform float stripePeriod;
uniform float stripeWidth;
uniform float lowerLimit;
uniform float upperLimit;

float f(float n, float eps, float k){
    if(n > eps){
		return pow(n,1.0f/3.0f);
	}else{
	    return (k*n+16.f)/116.f;
	}    
}
vec3 XYZtoLRGB(vec3 xyz, bool clamp){
	vec3 M0 = vec3(3.2404542f, -1.5371385f, -0.4985314f);
	vec3 M1 = vec3(-0.9692660f,  1.8760108f, 0.0415560f);
	vec3 M2 = vec3(0.0556434f, -0.2040259f,  1.0572252f);

    float r = dot(xyz,M0);
    float g = dot(xyz,M1);
    float b = dot(xyz,M2);

    if(clamp){
        r = min(max(r,0.f),1.f);
        g = min(max(g,0.f),1.f);
        b = min(max(b,0.f),1.f);
    }
		
	return  vec3(r,g,b);
}
vec3 LRGBtoXYZ(vec3 lrgb){
	vec3 M0 = vec3(0.4124564f,  0.3575761f,  0.1804375f);
	vec3 M1 = vec3(0.2126729f,  0.7151522f,  0.0721750f);
	vec3 M2 = vec3(0.0193339f,  0.1191920f,  0.9503041f);
		  
	return  vec3(dot(lrgb,M0),dot(lrgb,M1),dot(lrgb,M2));
}
vec3 XYZtoLAB(vec3 xyz){
	float Xr = 0.95047f;
    float Yr = 1.0f;
	float Zr = 1.08883f;

	float eps = 216.f/24389.f;
	float k = 24389.f/27.f;
		  
	float xr = xyz.x/Xr;
	float yr = xyz.y/Yr;
	float zr = xyz.z/Zr;

	xr = f(xr,eps,k);
	yr = f(yr,eps,k);
	zr = f(zr,eps,k);

	float L = 116*yr-16;
	float a = 500*(xr-yr);
	float b = 200*(yr-zr);

	return vec3(L,a,b);
}
vec3 LABtoXYZ(vec3 lab){
	float Xr = 0.95047f;
	float Yr = 1.0f;
	float Zr = 1.08883f;
		
	float eps = 216.f/24389.f;
	float k = 24389.f/27.f;

	float L = lab.x;
	float a = lab.y;
	float b = lab.z;

	float fy  = (L+16.f)/116.f;
	float fx  = a/500.f + fy;
	float fz  = -b/200.f + fy;

	float xr = ((pow(fx,3.0f)>eps) ? pow(fx,3.0f) : (116.f*fx - 16.f)/k);
	float yr = ((L > (k*eps)) ? pow(((L+16.f)/116.f),3.0f): L/k);
	float zr = ((pow(fz,3.0f)>eps) ? pow(fz,3.0f) : (116.f*fz - 16.f)/k);

	float X = xr*Xr;
	float Y = yr*Yr;
	float Z = zr*Zr;

	return vec3(X,Y,Z);
}
vec3 LABtoLCH(vec3 lab){
	float l = lab.x;
	float a = lab.y;
	float b = lab.z;
		
	float C = sqrt(a*a+b*b);
	float H = atan(b,a);

    return vec3(l,C,H);
}
vec3 LCHtoLAB(vec3 lch){
	float l = lch.x;
	float c = lch.y;
	float h = lch.z;
		
	return vec3(l,c*cos(h),c*sin(h));
}
vec3 RGBtoLAB(vec3 rgb){
	return  XYZtoLAB(LRGBtoXYZ(rgb));
}
vec3 LABtoRGB(vec3 lab,bool clamp ){
	return  XYZtoLRGB(LABtoXYZ(lab),clamp);
}
float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}
void main()
{
	float PI = 3.14159265358979323846264;	
	float w;
	w = texelFetch(u_Texture0, ivec2(gl_FragCoord.x, gl_FragCoord.y),0).r;
	float dist = texelFetch(DistText1, ivec2(gl_FragCoord.x, gl_FragCoord.y),0).r;
	float wf = w/maxVal;
	//w = min(w,1.0);
	//wf=w;	
	float a = wf>lowerLimit?wf:0.0;

	vec3 lab = RGBtoLAB(rgbCol);
	vec3 lch = LABtoLCH(lab);
	vec2 dir = vec2(cos(angle),sin(angle));
	dir = normalize(dir);

	float c = dot(dir,gl_FragCoord.xy);
	c = mod(c,stripePeriod);
	//float cosv = abs(cos(((2*PI)/stripeWidth)*c));
	//float d = .5*cosv + .5;
	
	if(c <= stripeWidth && wf>=upperLimit ||( dist>0.0 && dist<3.0)){
		lch.x *= .90;
		lch.y *= .90;
		a=1000.0;
	}else{
		if(wf>=upperLimit){
			wf = 1.0;
			a = 1.0;
		}else{
			wf = wf/upperLimit;
		}

		lch.x = lch.x*wf + (1.0-wf)*100.0;
		lch.y = lch.y*wf;
	}
	
	vec3 ret = LABtoRGB(LCHtoLAB(lch),true);
	gl_FragColor = vec4(ret.x,ret.y,ret.z,a);
}