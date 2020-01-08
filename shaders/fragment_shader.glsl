#version 100

precision mediump float;
uniform sampler2D in_fontTexture;
varying vec4 v_color;
varying vec2 v_texCoord;

void main()
{
	gl_FragColor = v_color * texture2D(in_fontTexture, v_texCoord);
}
