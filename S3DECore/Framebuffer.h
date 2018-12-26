#pragma once
#include "Textures.h"
#include <gl\glew.h>

class FrameBuffer {
public:
	FrameBuffer(void);
	uint GetIdentifier(void);
	void Bind(void);
	bool IsComplete(void);
private:
	uint identifier;
};