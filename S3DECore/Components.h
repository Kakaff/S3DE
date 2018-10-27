#pragma once
#include "Entities.h"

class Transform {
public:
	Transform(class GameEntity* parentEntity);

private:
	Transform(void);
	class GameEntity* entity;
};
