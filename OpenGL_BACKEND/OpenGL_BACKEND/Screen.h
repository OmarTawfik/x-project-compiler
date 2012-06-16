#pragma once

#include "Headers.h"
#include "GLObject.h"
#include "Color.h"
#include "UpdateArguments.h"
#include "Shader.h"


class Screen
{
private:
	static int ScreenWidth;
	static int ScreenHeight;
	friend class ScreenManager;
    //The primary update objects
    vector<shared_ptr<GLObject> > ScreenObjects;
public:
	//The primary shader
	static Shader* shader2D;
    Screen();
    //Background Color
    Color Background;
    //True rendering virtual function here
    void DrawScreen();
    //The Screen Update function
	void virtual UpdateScreen(UpdateArguments* e);
	//Add/Remove children
	void AddChild(shared_ptr<GLObject> child);
	void RemoveChild(shared_ptr<GLObject> child);
    //User input overrides
    virtual void KeyboardDown(int key, int x, int y) {}
    virtual void KeyboardUp(int key, int x, int y) {}
    virtual void KeyboardPressed(int key, int x, int y) {}
    virtual void MouseDrag(int x, int y, int button) {}
    virtual void MouseMove(int x, int y) {}
    virtual void MouseDown(int x, int y, int button) {}
    virtual void MouseUp(int x, int y, int button) {}
    virtual void ScreenResize(int width, int height) {}
    
	~Screen() {}
};