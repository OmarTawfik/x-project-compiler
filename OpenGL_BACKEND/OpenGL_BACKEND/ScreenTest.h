#pragma once

#include "Screen.h"
#include "Sprite.h"
#include "Keyboard.h"

class ScreenTest : public Screen
{
public:
	shared_ptr<Sprite> test;

	ScreenTest()
	{
		Background.R = 0.5;
		test = shared_ptr<Sprite>(new Sprite("C:\\Users\\Muhammad\\Desktop\\746361explosion1.png"));
		test->Position.x = 100;
		test->Position.y = 100;
		AddChild(test);
	}

	virtual void UpdateScreen(UpdateArguments* e)
	{
		if (Keyboard::IsKeyPressed('a'))
		{
			RemoveChild(test);
			test = NULL;
		}
	}
};