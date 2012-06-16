#include "GLObject.h"
#include "Headers.h"

GLObject::GLObject(void)
{
}


void GLObject::Render()
{
	glPushMatrix();
	this->InitDrawing();
	this->Draw();
	this->PostDrawing();
	glPopMatrix();
}