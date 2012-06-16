#include "ScreenManager.h"
#include "Headers.h"
#include "Helpers.h"
#include "Screen.h"
#include "ScreenTest.h"


void Draw();
void Update(int);
void SetEnvironment();
void Reshape(int w, int h);
void Keyboard(unsigned char key, int x, int y);
void KeyboardUp(unsigned char key, int x, int y);
void SpecialKeyboard(int key, int x, int y);
void SpecialKeyboardUp(int key, int x, int y);
void MouseDragFunc(int x, int y);
void MouseFunc(int button,int state,int x, int y);
void MouseMoveFunc(int x, int y);
void InitKeys();

ScreenManager* mng;

int main(int argc, char** argv)
{
	glutInit(&argc, argv);
    glutInitDisplayMode(GLUT_RGB | GLUT_DOUBLE | GLUT_DEPTH);
    
	glutInitWindowSize(800, 500);
	glutCreateWindow("OpenGL Engine");
	glutPositionWindow(20, 30);

	glewInit();
    
	char* po = (char*)glGetString(GL_VERSION);
    printf("You are working with OpenGL, version %s", po);
    
    
	mng = new ScreenManager(new ScreenTest());
	ScreenManager::CurrentScreenManager  = mng;
    
	glutDisplayFunc(Draw); 
	glutTimerFunc(0, Update, 0);
	glutReshapeFunc(Reshape);
	glutKeyboardFunc(Keyboard);
	glutKeyboardUpFunc(KeyboardUp);
	glutSpecialFunc(SpecialKeyboard);
	glutSpecialUpFunc(SpecialKeyboardUp);
	glutMouseFunc(MouseFunc);
	glutMotionFunc(MouseDragFunc);
	glutPassiveMotionFunc(MouseMoveFunc);
    
    //PrimaryShaders::InitShaders();
    
	glutMainLoop();
	return 0;
}

void Draw()
{
	Screen* tmp = mng->GetCurrentScreen();
	glClearColor(tmp->Background.R, tmp->Background.G, tmp->Background.B, tmp->Background.A);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	SetEnvironment();	
    
	mng->RenderScreen();
    
	glutSwapBuffers();
}

int prevTick = GetMilliTickCount();
void Update(int COUNT)
{
    static float Tick = GetMilliTickCount();
    Tick = GetMilliTickCount();
    
    mng->Update(Tick, 16);
    
    prevTick = Tick;
    
    glutPostRedisplay();

	glutTimerFunc(16, Update, ++COUNT);
}

void Keyboard(unsigned char key, int x, int y)
{
	mng->KeyboardFunc(key,x,y);
}

void KeyboardUp(unsigned char key, int x, int y)
{
	mng->KeyboardUpFunc(key, x,y);
}

void SpecialKeyboard(int key, int x, int y)
{
	mng->SpecialKeyFunc(key,x,y);
}

void SpecialKeyboardUp(int key, int x, int y)
{
	mng->SpecialKeyUpFunc(key,x,y);
}

void MouseDragFunc(int x, int y)
{
	mng->MouseDragFunc(x,y);
}

void MouseFunc(int button,int state,int x, int y)
{
	mng->MouseFunc(button, state, x,y);
}

void MouseMoveFunc(int x, int y)
{
	mng->MouseMoveFunc(x,y);
}

void SetEnvironment()
{
	glEnable(GL_DEPTH_TEST);		//Depth test
	glEnable(GL_COLOR_MATERIAL);	//Enable Materials
	glEnable(GL_LIGHTING);			//Enable lighting
	glEnable(GL_NORMALIZE);			//Automatically normalize normals
	glEnable(GL_BLEND);				//For Alpha Blending
    glEnable(GL_LINE_SMOOTH);
	glBlendFunc(GL_SRC_ALPHA,GL_ONE_MINUS_SRC_ALPHA);// same
    
	glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);
}

void Reshape(int w, int h)
{
	glViewport(0, 0, w, h);
	mng->ScreenResize(w, h);
}