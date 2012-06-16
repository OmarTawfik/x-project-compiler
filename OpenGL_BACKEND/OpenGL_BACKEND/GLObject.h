#pragma once
#include "UpdateArguments.h"

class GLObject
{
public:
    //Constructor
    GLObject();
    //Render function.
	virtual void Draw() {}
    //Pre-rendering operations
	virtual void InitDrawing() {}
    //Post-rendering operations
	virtual void PostDrawing() {}
    //These functions are the ones to call for rendering
    void Render();
	//Update function
	virtual void Update(UpdateArguments* e) {}
};