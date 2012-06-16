#pragma once

class Color
{
public:
	//Color data
	float R, G, B, A;
	////Constructors
	Color()
	{
		R = G = B = 0;
		A = 1;
	}
	
	Color(float R, float G, float B)
	{
		this->R = R;
		this->G = G;
		this->B = B;
		this->A = 1;
	}

	Color(float R, float G, float B, float A)
	{
		this->R = R;
		this->G = G;
		this->B = B;
		this->A = A;
	}
	////
};