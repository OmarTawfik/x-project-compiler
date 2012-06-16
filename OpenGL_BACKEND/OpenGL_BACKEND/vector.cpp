#include "point.h"
#include "vector.h"
#include "Headers.h"

Vector Vector::operator-() {
	Vector v;
	v.x = -x; v.y = -y;
	return v;
}

// Unary 2D perp operator
Vector Vector::operator~()
{
	Vector v;
	v.x = -y; v.y = x;
	return v;
}

// Scalar multiplication
Vector operator*( int c, Vector w ) {
	Vector v;
	v.x = c * w.x;
	v.y = c * w.y;
	return v;
}

Vector operator*( double c, Vector w ) {
	Vector v;
	v.x = c * w.x;
	v.y = c * w.y;
	return v;
}

Vector operator*( Vector w, int c ) {
	Vector v;
	v.x = c * w.x;
	v.y = c * w.y;
	return v;
}

Vector operator*( Vector w, double c ) {
	Vector v;
	v.x = c * w.x;
	v.y = c * w.y;
	return v;
}

// Scalar division
Vector operator/( Vector w, int c ) {
	Vector v;
	v.x = w.x / c;
	return v;
}

Vector operator/( Vector w, double c ) {
	Vector v;
	v.x = w.x / c;
	v.y = w.y / c;
	return v;
}

//------------------------------------------------------------------
//  Arithmetic Ops
//------------------------------------------------------------------

Vector Vector::operator+( Vector w ) {
	Vector v;
	v.x = x + w.x;
	v.y = y + w.y;
	return v;
}

Vector Vector::operator-( Vector w ) {
	Vector v;
	v.x = x - w.x;
	v.y = y - w.y;
	return v;
}

//------------------------------------------------------------------
//  Products
//------------------------------------------------------------------

// Inner Dot Product
double Vector::operator*( Vector w ) {
	return (x * w.x + y * w.y);
}

// 2D Exterior Perp Product
double Vector::operator|( Vector w ) {
	return (x * w.y - y * w.x);
}

//------------------------------------------------------------------
//  Shorthand Ops
//------------------------------------------------------------------

Vector& Vector::operator*=( double c ) {        // vector scalar mult
	x *= c;
	y *= c;
	return *this;
}

Vector& Vector::operator/=( double c ) {        // vector scalar div
	x /= c;
	y /= c;
	return *this;
}

Vector& Vector::operator+=( Vector w ) {        // vector increment
	x += w.x;
	y += w.y;
	return *this;
}

Vector& Vector::operator-=( Vector w ) {        // vector decrement
	x -= w.x;
	y -= w.y;
	return *this;
}


//------------------------------------------------------------------
//  Special Operations
//------------------------------------------------------------------

void Vector::normalize() {                      // convert to unit length
	double ln = sqrt( x*x + y*y);
	if (ln == 0) return;                    // do nothing for nothing
	x /= ln;
	y /= ln;
}
