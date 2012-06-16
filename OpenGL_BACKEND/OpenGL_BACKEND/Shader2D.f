//This is the primary 2D vertex shader.

#version 120

//GL3.2 ONLY
//uniform mat4 CameraMatrix;
//uniform mat4 ModelViewMatrix;
uniform sampler2D Sprite;
uniform vec2 ScreenSize;

varying vec2 texcoord;


void main()
{
	gl_FragColor = texture2D(Sprite, texcoord);
}