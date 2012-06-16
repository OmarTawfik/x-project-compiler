#include "GLObject.h"
#include "Headers.h"
#include "vector.h"
#include "point.h"
#include "Size.h"
#include "SOIL\SOIL.h"

class Sprite : public GLObject
{
private:
	GLuint textureId;
public:
	Point Position;
	Vector Velocity;
	Vector Acceleration;
	Size Size;
	float Rotation;

	Sprite(string filePath)
	{
		this->textureId = SOIL_load_OGL_texture(filePath.c_str(), SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, SOIL_FLAG_TEXTURE_REPEATS);
		Position.x = 0;
		Position.y = 0;
		Velocity.x = 0;
		Velocity.y = 0;
		Acceleration.x = 0;
		Acceleration.y = 0;
		Rotation = 0;

		if (this->textureId > 0)
		{
			this->Size.Width = SOILLoadedTextureWidth;
			this->Size.Height = SOILLoadedTextureHeight;
		}
	}

	virtual void InitDrawing()
	{
		glTranslatef(Position.x, Position.y, 0.0f);
		glRotatef(Rotation, 0, 0, 1);
	}

	virtual void Draw()
	{
		float vertecies[] = {
			-this->Size.Width/2, this->Size.Height/2,
			this->Size.Width/2, this->Size.Height/2,
			this->Size.Width/2, -this->Size.Height/2,
			-this->Size.Width/2, -this->Size.Height/2
		};

		float texCoords[] = {
			0, 0,
			1, 0,
			1, 1,
			0, 1
		};

		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_2D, this->textureId);

		GLuint uni = Screen::shader2D->GetUniformAddress("Sprite");
		glUniform1i(uni, 0);

		glEnableClientState(GL_VERTEX_ARRAY);
		glEnableClientState(GL_TEXTURE_COORD_ARRAY);

		glVertexPointer(2, GL_FLOAT, 0, vertecies);
        glTexCoordPointer(2, GL_FLOAT, 0, texCoords);
        
        glDrawArrays(GL_QUADS, 0, 4);
        
        glDisableClientState(GL_TEXTURE_COORD_ARRAY);
        glDisableClientState(GL_VERTEX_ARRAY);
	}

	~Sprite()
	{
		cout << "Sprite Destroyed!" << endl;
	}
};