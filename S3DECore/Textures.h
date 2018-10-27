#pragma once
#include "GL\glew.h"
#include "EngineTypes.h"
class Texture {
public:
	Texture(int target);
	uint GetIdentifier();
	int GetTarget();
private:
	int targ;
	uint identifier;
};