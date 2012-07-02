#version 150 compatibility
in vec3 pos;

uniform vec3 Brush;
uniform vec3 Pen;

void main()
{
	vec4 color = vec4(0.0,0.0,0.0,0.0);
	float dist = distance(gl_FragCoord.xy,pos.xy);
	if(dist<=4){
		color = vec4(Brush.x,Brush.y,Brush.z,1.0);
	}else if(dist<=5){
		color = vec4(Pen.x,Pen.y,Pen.z,1.0);
	}
	gl_FragDepth = pos.z;
	gl_FragColor = color;
}