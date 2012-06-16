//
//  Shader.h
//  OpenGL Engine
//
//  Created by Muhammad Al-Adly on 8/9/11.
//  Copyright 2011 MAS-P. All rights reserved.
//

#ifndef SHADER_H_
#define SHADER_H_

#include "Headers.h"


class Shader
{
private:
	GLuint ShaderIDs[3];
	GLuint ProgramID;
	int index;
	string Name;
public:
    
	static void printShaderInfoLog(GLuint obj)
	{
	    int infologLength = 0;
	    int charsWritten  = 0;
	    char *infoLog;
        
		glGetShaderiv(obj, GL_INFO_LOG_LENGTH,&infologLength);
        
	    if (infologLength > 0)
	    {
	        infoLog = (char *)malloc(infologLength);
	        glGetShaderInfoLog(obj, infologLength, &charsWritten, infoLog);
			printf("%s\n",infoLog);
	        free(infoLog);
	    }
	}
    
	static void printProgramInfoLog(GLuint obj)
	{
	    int infologLength = 0;
	    int charsWritten  = 0;
	    char *infoLog;
        
		glGetProgramiv(obj, GL_INFO_LOG_LENGTH,&infologLength);
        
	    if (infologLength > 0)
	    {
	        infoLog = (char *)malloc(infologLength);
	        glGetProgramInfoLog(obj, infologLength, &charsWritten, infoLog);
			printf("%s\n",infoLog);
	        free(infoLog);
	    }
	}
    
	Shader(string name)
	{
		Name = name;
		index = 0;
		ProgramID = glCreateProgram();
	}
    
    GLuint GetID() { return ProgramID; }
    
	void AddShader(const char* prog, int Type)
	{
		ShaderIDs[index] = glCreateShader(Type);
		glShaderSource(ShaderIDs[index], 1, &prog, NULL);
		glCompileShader(ShaderIDs[index]);
		printShaderInfoLog( ShaderIDs[index] );
        
		glAttachShader(ProgramID, ShaderIDs[index]);
		index++;
	}
    
	void CompileShader()
	{
		glLinkProgram(ProgramID);
		printProgramInfoLog(ProgramID);
	}
    
	void ShaderBegin()
	{
		glUseProgram(ProgramID);
	}
    
	void ShaderEnd()
	{
		glUseProgram(NULL);
	}
    
	inline GLuint GetUniformAddress(char* name)
	{
		return glGetUniformLocation(ProgramID, (const char*)name);
	}
    
    GLuint GetAttributeAddress(char* name)
    {
        return glGetAttribLocation(ProgramID, (const char*) name);
    }
    
	~Shader()
	{
		for(int i=0; i<index; i++)
		{
			glDetachShader(ProgramID, ShaderIDs[i]);
			glDeleteShader(ShaderIDs[i]);
		}
		glDeleteProgram(ProgramID);
	}
};

#endif