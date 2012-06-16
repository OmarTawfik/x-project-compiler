#pragma once
#include "Headers.h"


class Keyboard
{
private:
	friend class ScreenManager;
	static bool Keys[384];

public:
	static void _init()
	{
		for (int i=0; i<384; ++i)
		{
			Keys[i] = false;
		}
	}

	static bool IsKeyPressed(int key)
	{
		if (key >= 384)
		{
			throw KEY_DOES_NOT_EXIST;
		}

		return Keys[key];
	}
};