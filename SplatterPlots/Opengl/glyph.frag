#version 150 compatibility
in vec4 pos;

void main()
{
	vec4 color = gl_FragColor;

	
	gl_FragColor = color;
}