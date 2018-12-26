#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <GL\glew.h>
#include <GLFW\glfw3.h>

#include "EngineMacros.h"
#include "EngineWindow.h"
#include "EngineClock.h"

#include "Graphics.h"


DLL_Export void InitGLFW() {
	if (!glfwInit()) {
		
	}
	else {

	}
}

DLL_Export void CreateWindow() {
	if (!S3DECore::Window::CreateWindow()) {

	}
	else {
		
	}
}

static void(*func)();
static void(*errFunc)();

DLL_Export void Extern_SetViewPortSize(uint width, uint height) {
	glViewport(0, 0, width, height);
}

DLL_Export void RegisterErrorFunc(void(*fun)()) {
	errFunc = fun;
}

DLL_Export void RegisterUpdateFunc(void(*fun)()) {
	func = fun;
}

DLL_Export void InitGlew() {
	glewExperimental = true;
	if (glewInit() != GLEW_OK) {
		
	}
	else {

	}
}

DLL_Export void Extern_GLGeti(GLint param, int& i) {
	glGetIntegerv(param, &i);
}

DLL_Export uint Extern_CheckGLErrors(void) {
	return glGetError();
}

DLL_Export void Extern_Enable(uint v) {
	glEnable(v);
}

DLL_Export void Extern_Disable(uint v) {
	glDisable(v);
}

DLL_Export void Extern_Clear(int clearval) {
	glClear(clearval);
}

void GLAPIENTRY MessageCallBack(GLenum src, GLenum type, GLuint id, GLenum severity, GLsizei length, const GLchar* message, const void* userparam) {
	printf("GL CALLBACK: %s type = 0x%x, severity = 0x%x, message = %s\n",
		(type == GL_DEBUG_TYPE_ERROR ? "** GL ERROR **" : ""),
		type, severity, message);
}

DLL_Export void RunEngine() {
	glViewport(0, 0, 1280, 720);
	glEnable(GL_DEPTH_TEST);
	glEnable(GL_CULL_FACE);
	glCullFace(GL_BACK);
	glFrontFace(GL_CCW);

	while (!S3DECore::Window::IsCloseRequested()) {
		glfwPollEvents();
		func();
		S3DECore::Window::SwapBuffers();
		WaitForNextFrame();
	}
}
