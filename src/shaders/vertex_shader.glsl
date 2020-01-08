#version 100

uniform mat4 projection_matrix;
attribute vec2 in_position;
attribute vec2 in_texCoord;
attribute vec4 in_color;
varying vec4 v_color;
varying vec2 v_texCoord;

void main()
{
	gl_Position = projection_matrix * vec4(in_position, 0, 1);
	v_color = in_color;
	v_texCoord = in_texCoord;
}
