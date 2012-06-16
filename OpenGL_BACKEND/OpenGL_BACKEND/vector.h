//==================================================================
// Copyright 2002, softSurfer (www.softsurfer.com)
// This code may be freely used and modified for any purpose
// providing that this copyright notice is included with it.
// SoftSurfer makes no warranty for this code, and cannot be held
// liable for any real or imagined damage resulting from it's use.
// Users of this code must verify correctness for their application.
//==================================================================

#ifndef Vector_H
#define Vector_H

#include "Headers.h"
#include "point.h"

class Vector : public Point {
public:
	// Constructors same as Point class
	Vector() : Point() {};
	Vector( float a, float b) : Point(a,b) {};
	~Vector() {};

	Vector operator-();
	Vector operator~();

	friend Vector operator*( int, Vector);
	friend Vector operator*( double, Vector);
	friend Vector operator*( Vector, int);
	friend Vector operator*( Vector, double);

	friend Vector operator/( Vector, int);
	friend Vector operator/( Vector, double);

	Vector operator+( Vector);        // vector add
	Vector operator-( Vector);        // vector subtract
	double operator*( Vector);        // inner dot product
	double operator|( Vector);        // 2D exterior perp product

	Vector& operator*=( double);      // vector scalar mult
	Vector& operator/=( double);      // vector scalar div
	Vector& operator+=( Vector);      // vector increment
	Vector& operator-=( Vector);      // vector decrement
	Vector& operator^=( Vector);      // 3D exterior cross product

	double len() {                    // vector length
		return sqrt(x*x + y*y);
	}
	double len2() {                   // vector length squared (faster)
		return (x*x + y*y);
	}

	void normalize();                 // convert vector to unit length
};
#endif SS_Vector_H
