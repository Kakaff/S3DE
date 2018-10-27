#include "Entities.h"
#include "Components.h"

Transform::Transform(GameEntity* parentEntity) {
	entity = parentEntity;
}