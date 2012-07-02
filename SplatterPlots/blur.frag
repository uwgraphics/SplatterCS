
#version 150 compatibility

uniform sampler2D u_Texture0;
uniform int kWindow;
uniform float sigma;
//uniform float gain;
uniform vec2 u_Off;

void main()
{
	vec4 color = vec4(0.0,0.0,0.0,1.0);
	float sum = 0.0;
	float gW = 0.0;
	float xOff = 0.0;
	float yOff = 0.0;
	for( int i = -kWindow; i <= kWindow; i++ )
	{
		gW = exp(-(float(i*i))/(2.0*sigma*sigma));
		sum += gW;
		xOff = u_Off.x*float(i);	
		yOff = u_Off.y*float(i);
	
		color += texelFetch( u_Texture0, ivec2(gl_FragCoord.x + xOff, gl_FragCoord.y + yOff),0)*gW;
	}	
	color/=sum;
	color.a = 1.0;	
	gl_FragColor = color;
}
