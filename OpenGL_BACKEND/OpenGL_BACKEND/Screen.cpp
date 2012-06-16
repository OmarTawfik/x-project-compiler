#include "Screen.h"
#include "Helpers.h"

Screen::Screen()
{
}

void Screen::DrawScreen()
{
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	glOrtho(0, ScreenWidth, 0, ScreenHeight, -100, 100);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	glViewport(0, 0, ScreenWidth, ScreenHeight);
	
    //glDepthMask(false);

	this->shader2D->ShaderBegin();
	glUniform2f(Screen::shader2D->GetUniformAddress("ScreenSize"), ScreenWidth, ScreenHeight);

	for (int i = 0; i < this->ScreenObjects.size(); ++i)
	{
		this->ScreenObjects[i]->Render();
	}

	this->shader2D->ShaderEnd();
}

void Screen::UpdateScreen(UpdateArguments* e)
{
	for (int i=0; i<this->ScreenObjects.size(); ++i)
	{
		this->ScreenObjects[i]->Update(e);
	}
}

void Screen::AddChild(shared_ptr<GLObject> child)
{
	bool isFound = false;
	long size = this->ScreenObjects.size();
	for	(int i=0; i<size; ++i)
	{
		if (this->ScreenObjects[i] == child)
		{
			isFound = true;
			break;
		}
	}

	if (!isFound)
	{
		this->ScreenObjects.push_back(child);
	}
}

void Screen::RemoveChild(shared_ptr<GLObject> child)
{
	vector<shared_ptr<GLObject> >::iterator i = this->ScreenObjects.begin();

	while(this->ScreenObjects.end() != i)
	{
		if (*i == child)
		{
			this->ScreenObjects.erase(i);
			return;
		}
	}
}

Shader* Screen::shader2D;
int Screen::ScreenWidth;
int Screen::ScreenHeight;