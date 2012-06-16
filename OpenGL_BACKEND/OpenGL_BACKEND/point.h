#ifndef Point_H
#define Point_H

class Point {
friend class Vector;
public:
	float x, y;

	Point()
	{
		x=y=0;	
	}
	
	Point(float x, float y)
	{
		this->x = x;
		this->y = y;
	}

	int operator==( Point);
	int operator!=( Point);

	Vector operator-( Point);       // Vector difference
	Point  operator+( Vector);      // +translate
	Point  operator-( Vector);      // -translate
	Point& operator+=( Vector);     // inc translate
	Point& operator-=( Vector);     // dec translate

	friend Point operator*( int, Point);
	friend Point operator*( double, Point);
	friend Point operator*( Point, int);
	friend Point operator*( Point, double);

	friend Point operator/( Point, int);
	friend Point operator/( Point, double);

	friend Point operator+( Point, Point);

	double DistanceTo(Point);         // Distance
};
#endif SS_Point_H
