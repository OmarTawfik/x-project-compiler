#ifndef SCREENMANAGER_H_
#define SCREENMANAGER_H_

#include "Headers.h"
#include "Helpers.h"
#include "Screen.h"
#include "Keyboard.h"
#include "Mouse.h"


class ScreenManager : public Screen
{
protected:
	Screen* Screens[64];
	int ScreenIndex;
    
public:
	static ScreenManager* CurrentScreenManager;
    
	ScreenManager(Screen* mainMenu)
	{
		ScreenIndex=1;
		Screens[0] = mainMenu;

		char* vertexShader = MASReadFileToCharPointer("C:\\Users\\Muhammad\\Documents\\Visual Studio 2010\\Projects\\OpenGL_BACKEND\\OpenGL_BACKEND\\Shader2D.v");
		char* fragmentShader = MASReadFileToCharPointer("C:\\Users\\Muhammad\\Documents\\Visual Studio 2010\\Projects\\OpenGL_BACKEND\\OpenGL_BACKEND\\Shader2D.f");

		Screen::shader2D = new Shader("2D Shader");
		Screen::shader2D->AddShader(vertexShader, GL_VERTEX_SHADER);
		Screen::shader2D->AddShader(fragmentShader, GL_FRAGMENT_SHADER);
		Screen::shader2D->CompileShader();
	}
    
	void PushScreen(Screen* screen)
	{
		Screens[ScreenIndex] = screen;
		ScreenIndex++;
	}
    
	void PopScreen()
	{
		ScreenIndex--;
	}
    
    Screen* GetCurrentScreen()
    {
        return Screens[ScreenIndex-1];
    }
    
	virtual void RenderScreen()
	{
		Screens[ScreenIndex-1]->DrawScreen();
	}
    
    
	virtual void Update(int ticker, int tickerStep)
	{
		UpdateArguments e(ticker/1000.0, tickerStep/1000.0, 0);
		Screens[ScreenIndex-1]->UpdateScreen(&e);
	}
    
	virtual void KeyboardFunc(unsigned char key, int x, int y)
	{
        ConvertMouseCoordinates(x,y);
        if (Keyboard::Keys[key] == false)
        {
            Keyboard::Keys[key] = true;
            Screens[ScreenIndex-1]->KeyboardDown(key,x,y);
        }
        Keyboard::Keys[key] = true;
        Screens[ScreenIndex-1]->KeyboardPressed(key,x,y);
        
	}
    
	virtual void KeyboardUpFunc(unsigned char key, int x, int y)
	{
        Keyboard::Keys[key] = false;
		ConvertMouseCoordinates(x,y);
		Screens[ScreenIndex-1]->KeyboardUp(key,x,y);		
	}
    
	virtual void SpecialKeyFunc(int key, int x, int y)
	{
        ConvertMouseCoordinates(x,y);
        if (Keyboard::Keys[key+128] == false)
        {
            Keyboard::Keys[key+128] = true;
            Screens[ScreenIndex-1]->KeyboardDown(key+128,x,y);
        }
        Keyboard::Keys[key+128] = true;
		Screens[ScreenIndex-1]->KeyboardPressed(key+128,x,y);
	}
    
	virtual void SpecialKeyUpFunc(int key, int x, int y)
	{
        Keyboard::Keys[key+128] = false;
		ConvertMouseCoordinates(x,y);
		Screens[ScreenIndex-1]->KeyboardUp(key+128,x,y);
	}
    
	virtual void MouseDragFunc(int x, int y)
	{
		ConvertMouseCoordinates(x,y);
		Mouse::mouseX = x;
		Mouse::mouseY = y;


		Screens[ScreenIndex-1]->MouseDrag(x,y, Mouse::GetCurrentMouseButton());
	}
    
	virtual void MouseFunc(int button,int state,int x, int y)
	{
		ConvertMouseCoordinates(x,y);

		Mouse::mouseX = x;
		Mouse::mouseY = y;
        Mouse::currentMouseButton = button;

		switch(button)
		{
		case 0:
			Mouse::isLeftMouseButtonDown = state == GLUT_DOWN;
			break;
		case 1:
			Mouse::isMiddleMouseButtonDown = state == GLUT_DOWN;
			break;
		case 2:
			Mouse::isRightMouseButtonDown = state == GLUT_DOWN;
			break;
		}

        if(state == GLUT_UP)
		{
            Screens[ScreenIndex-1]->MouseUp(x,y,button);
		}
        else
        {
            Screens[ScreenIndex-1]->MouseDown(x,y,button);
        }
	}
    
	virtual void MouseMoveFunc(int x, int y)
	{
		ConvertMouseCoordinates(x,y);
		Mouse::mouseX = x;
		Mouse::mouseY = y;

		Screens[ScreenIndex-1]->MouseMove(x,y);
	}
    
    virtual void ScreenResize(int width, int height)
    {
		Screen::ScreenWidth = width;
		Screen::ScreenHeight = height;

		Screens[ScreenIndex-1]->ScreenResize(width, height);
    }
    
	void ConvertMouseCoordinates(int &x, int &y)
	{
		x -= Screen::ScreenWidth/2;
		y -= Screen::ScreenHeight/2;
		y *= -1;
	}
};

#endif