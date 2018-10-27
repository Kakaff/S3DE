#include "Entities.h"
#include "EngineMacros.h"

uint GameEntity::count = 0;

GameEntity::GameEntity() {
	id = count + 1;
	count += 1;
	trans = new Transform(this);
}

DLL_Export GameEntity* Extern_CreateGameEntity(void) {
	return new GameEntity();
}

