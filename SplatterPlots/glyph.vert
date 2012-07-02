#version 150 compatibility
out vec3 pos;

float rand(vec2 co){
    return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

void main()
{	
    gl_Position =  ftransform();
	float r = rand(gl_Position.xy);
	pos = vec3(gl_Position.x,gl_Position.y,r);
}