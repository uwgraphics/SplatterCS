#version 150 compatibility

uniform sampler2D u_Texture0;
uniform int kStep;
uniform int init;
uniform float upperLimit;
uniform float maxVal;

vec3 calcMin(vec3 val, ivec2 coord){
    vec2 v = texelFetch( u_Texture0, coord, 0).yz;	
	float dist = distance(gl_FragCoord.xy, v);
	if(dist<val.x){
		return vec3(dist,v.x,v.y);
	}else{
		return val;
	}
}
void main()
{
	if(init==1){
		float val = texelFetch( u_Texture0, ivec2(gl_FragCoord.x, gl_FragCoord.y),0).r;
		vec4 color = vec4(0,gl_FragCoord.x,gl_FragCoord.y,1);
		val = val/maxVal;
		if(val<upperLimit){
			color.r = 1000000.0;
			color.g = 1000000.0;
			color.b = 1000000.0;
		}
		gl_FragColor = color;
	}else{
		vec3 minVal = texelFetch( u_Texture0, ivec2(gl_FragCoord.x, gl_FragCoord.y),0).xyz;

		minVal = calcMin(minVal,ivec2(gl_FragCoord.x + kStep, gl_FragCoord.y + kStep));
		minVal = calcMin(minVal,ivec2(gl_FragCoord.x + kStep, gl_FragCoord.y - kStep));
		minVal = calcMin(minVal,ivec2(gl_FragCoord.x - kStep, gl_FragCoord.y + kStep));
		minVal = calcMin(minVal,ivec2(gl_FragCoord.x - kStep, gl_FragCoord.y - kStep));
		minVal = calcMin(minVal,ivec2(gl_FragCoord.x + 0, gl_FragCoord.y + kStep));
		minVal = calcMin(minVal,ivec2(gl_FragCoord.x + 0, gl_FragCoord.y - kStep));
		minVal = calcMin(minVal,ivec2(gl_FragCoord.x + kStep, gl_FragCoord.y + 0));
		minVal = calcMin(minVal,ivec2(gl_FragCoord.x - kStep, gl_FragCoord.y + 0));

		vec4 color = vec4(minVal.x,minVal.y,minVal.z,1);		
		gl_FragColor = color;
	}
}