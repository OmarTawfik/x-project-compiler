#pragma once

#include "Headers.h"
#include "point.h"

class Mouse
{
private:
	friend class ScreenManager;
	static int mouseX;
	static int mouseY;
	static bool isLeftMouseButtonDown;
	static bool isRightMouseButtonDown;
	static bool isMiddleMouseButtonDown;
	static int currentMouseButton;

public:
	static void _init()
	{
		mouseX = 0;
		mouseY = 0;
		isLeftMouseButtonDown = false;
		isRightMouseButtonDown = false;
		isMiddleMouseButtonDown = false;
		currentMouseButton = 1;
	}

	static Point GetMousePosition()
	{
		Point mouse(mouseX, mouseY);
		return mouse;
	}
	
	static bool IsLeftMouseButtonDown()
	{
		return isLeftMouseButtonDown;
	}

	static bool IsRightMouseButtonDown()
	{
		return isRightMouseButtonDown;
	}

	static bool IsMiddleMouseButtonDown()
	{
		return isMiddleMouseButtonDown;
	}

	static int GetCurrentMouseButton()
	{
		return currentMouseButton;
	}
};