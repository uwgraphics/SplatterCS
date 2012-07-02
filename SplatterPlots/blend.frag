#version 150 compatibility

uniform sampler2D u_Texture0;
uniform sampler2D u_Texture1;
uniform sampler2D u_Texture2;
uniform sampler2D u_Texture3;
uniform sampler2D u_Texture4;
uniform sampler2D u_Texture5;
uniform sampler2D u_Texture6;
uniform sampler2D u_Texture7;

uniform int N;
uniform float lf;
uniform float cf;

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

void main()
{
	if(N==0){
		//vec4 col = vec4(texelFetch(u_Texture0, ivec2(gl_FragCoord.x, gl_FragCoord.y),0).xyz,1.0);
        //gl_FragColor = col;
		gl_FragColor = texelFetch(u_Texture0, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
	}else{

		vec4 colors[8];
		colors[0] = texelFetch(u_Texture0, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
		colors[1] = texelFetch(u_Texture1, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
		colors[2] = texelFetch(u_Texture2, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
		colors[3] = texelFetch(u_Texture3, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
		colors[4] = texelFetch(u_Texture4, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
		colors[5] = texelFetch(u_Texture5, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
		colors[6] = texelFetch(u_Texture6, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
		colors[7] = texelFetch(u_Texture7, ivec2(gl_FragCoord.x, gl_FragCoord.y),0);
    
		float x =0;
		float y =0;
		float z =0;

		//float Cdec = cf;
		//float Ldec = lf;
		float Nf = 0;
		float Npf = 0;

		for(int i=0;i<N;i++){
			vec3 lab = RGBtoLAB(colors[i].xyz);
        
			x+= lab.x*colors[i].w;
			y+= lab.y*colors[i].w;
			z+= lab.z*colors[i].w;
			Nf += colors[i].w;
			Npf += min(1.0,colors[i].w);
			//Cdec*=Cdec;
			//Ldec*=Ldec;
		}
		float pf = max(Npf-1.0,0.0);
		float Cdec = pow(cf,pf);
		float Ldec = pow(lf,pf);
		if(Nf>N){
			Cdec = 1.0;
			Ldec = 1.0;
		}
		if(Nf<=0){
			gl_FragColor = vec4(1.0,1.0,1.0,1.0);		
		}else{
			vec3 newLab = vec3(x/Nf,y/Nf,z/Nf);
			vec3 newLch = LABtoLCH(newLab);
			newLch.y = newLch.y*Cdec;
			newLch.x = newLch.x*Ldec;

			vec3 ret = LABtoRGB(LCHtoLAB(newLch),true);
			gl_FragColor = vec4(ret.x,ret.y,ret.z,1);			
			//gl_FragColor = vec4(1,0,0,1);
		}
	}
}