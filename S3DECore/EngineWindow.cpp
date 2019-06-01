#include "EngineWindow.h"
#include <GLFW\glfw3.h>

bool S3DECore::GLFW::Init() {
	return glfwInit();
}

bool S3DECore::Window::CreateWindow(int width, int height) {

	SetWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	SetWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	SetWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	S3DECore::Window::window_ptr = glfwCreateWindow(width, height, "", NULL, NULL);
	if (S3DECore::Window::window_ptr == NULL)
		return false;

	glfwMakeContextCurrent(S3DECore::Window::window_ptr);
	
	return true;
}


int S3DECore::Window::GetAttribute(int attr) {
	return glfwGetWindowAttrib(S3DECore::Window::window_ptr,attr);
}



void S3DECore::Window::SetWindowHint(int hint, int value) {
	glfwWindowHint(hint, value);
}


void S3DECore::Window::DestroyWindow()
{
	glfwDestroyWindow(S3DECore::Window::window_ptr);
}

void S3DECore::Window::SetResolution(int width, int height)
{
	glfwSetWindowSize(S3DECore::Window::window_ptr, width, height);
}


void S3DECore::Window::SwapBuffers()
{
	glfwSwapBuffers(S3DECore::Window::window_ptr);
}

void S3DECore::Window::SwapInterval(int v)
{
	glfwSwapInterval(v);
}

bool S3DECore::Window::IsCloseRequested() {
	return glfwWindowShouldClose(S3DECore::Window::window_ptr);
}


