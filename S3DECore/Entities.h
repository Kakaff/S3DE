#pragma once
#include "EngineTypes.h"
#include "Components.h"

class GameEntity {
public:
	GameEntity(void);
	uint GetID(void);
private:
	uint id;
	class Transform* trans;
	static uint count;
};
