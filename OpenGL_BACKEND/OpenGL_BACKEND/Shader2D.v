//This is the primary 2D vertex shader.

#version 120

//GL3.2 ONLY
//uniform mat4 CameraMatrix;
//uniform mat4 ModelViewMatrix;
uniform sampler2D Sprite;

varying vec2 texcoord;


void main()
{
	texcoord = gl_MultiTexCoord0.xy; 
	gl_Position = gl_ProjectionMatrix * gl_ModelViewMatrix * gl_Vertex;
}