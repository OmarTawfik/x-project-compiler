#pragma once

#include <iostream>
#include <math.h>
#include <stdio.h>
#include <string>
#include <fstream>
#include <vector>
#include <time.h>
#include <sys/timeb.h>
#include <ctime>

#include <windows.h>
#include <gl/glew.h>
#include <gl/gl.h>
#include <GL/GLUT.h>

using namespace std;

#pragma comment( lib, "glew32.lib" )
#pragma comment( lib, "glut32.lib" )
#pragma comment( lib, "glaux.lib")
#pragma comment( lib, "opengl32.lib")


const double PI = acos(-1.0);

//ERROR CODES
#define KEY_DOES_NOT_EXIST -10

