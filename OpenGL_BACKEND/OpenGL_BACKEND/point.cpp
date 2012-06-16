#include "point.h"
#include "vector.h"
#include "Headers.h"


int Point::operator==( Point Q)
{
	return (x==Q.x && y==Q.y);
}

int Point::operator!=( Point Q)
{
	return (x!=Q.x || y!=Q.y);
}

Vector Point::operator-( Point Q)
{
	Vector v;
	v.x = x - Q.x;
	v.y = y - Q.y;
	return v;
}

Point Point::operator+( Vector v)
{
	Point P;
	P.x = x + v.x;
	P.y = y + v.y;
	return P;
}

Point Point::operator-( Vector v)
{
	Point P;
	P.x = x - v.x;
	P.y = y - v.y;
	return P;
}

Point& Point::operator+=( Vector v)
{
	x += v.x;
	y += v.y;
	return *this;
}

Point& Point::operator-=( Vector v)
{
	x -= v.x;
	y -= v.y;
	return *this;
}

Point operator*( int c, Point Q) {
	Point P;
	P.x = c * Q.x;
	P.y = c * Q.y;
	return P;
}

Point operator*( double c, Point Q) {
	Point P;
	P.x = c * Q.x;
	P.y = c * Q.y;
	return P;
}

Point operator*( Point Q, int c) {
	Point P;
	P.x = c * Q.x;
	P.y = c * Q.y;
	return P;
}

Point operator*( Point Q, double c) {
	Point P;
	P.x = c * Q.x;
	P.y = c * Q.y;
	return P;
}

Point operator/( Point Q, int c) {
	Point P;
	P.x = Q.x / c;
	P.y = Q.y / c;
	return P;
}

Point operator/( Point Q, double c) {
	Point P;
	P.x = Q.x / c;
	P.y = Q.y / c;
	return P;
}

Point operator+( Point Q, Point R)
{
	Point P;
	P.x = Q.x + R.x;
	P.y = Q.y + R.y;
	return P;
}

double Point::DistanceTo(Point Q)
{
	double dx = this->x - Q.x;
	double dy = this->y - Q.y;
	return sqrt(dx*dx + dy*dy);
}
