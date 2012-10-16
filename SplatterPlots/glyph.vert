#version 150 compatibility
out vec3 pos;

void main()
{	
    gl_Position =  ftransform();	
	pos = vec4(gl_Position.x,gl_Position.y,gl_Position.z,gl_Position.w);
}