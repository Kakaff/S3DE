#include "Renderer.h"
#include "Textures.h"
#include "EngineWindow.h"

namespace S3DECore {
	namespace Graphics {

		void Renderer::Init(Vector2 displayRes, Vector2 renderRes) {
			if (glewInit() != GLEW_OK)
				throw gcnew System::Exception("Error initializing glew!");

			currRendRes = renderRes;
			currDispRes = displayRes;
			SetViewportSize(0, 0, displayRes.x, displayRes.y);
			Textures::TextureUnits::InitTextureUnits();

			S3DECore::Window::DisplayResolution = displayRes;

		}

		void Renderer::UpdateEvents() {
			if (renderResChanged) {
				Vector2 oldRes = currRendRes;
				currRendRes = newRendRes;
				if (OnRenderResolutionChanged != nullptr)
					OnRenderResolutionChanged(oldRes, currRendRes);
				renderResChanged = false;
			}

			if (displayReyChanged) {
				Vector2 oldRes = currDispRes;
				currDispRes = newDispRes;
				if (OnDisplayResolutionChanged != nullptr)
					OnDisplayResolutionChanged(oldRes, currDispRes);
				displayReyChanged = false;

				S3DECore::Window::DisplayResolution = newDispRes;
			}
		}

		int Renderer::GetParami(int param) {
			int val;
			glGetIntegerv(param, &val);
			return val;
		}

		void Renderer::Enable(RendererCapability cap) {
			glEnable((int)cap);
			CheckErrors(String::Format("Tried enabling cap {0}",cap));
		}

		void Renderer::Disable(RendererCapability cap) {
			glDisable((int)cap);
			CheckErrors(String::Format("Tried disabling cap {0}", cap));
		}

		void Renderer::Clear(ClearBufferBit cbb) {
			glClear((GLbitfield)cbb);
			CheckErrors(String::Format("Tried clearing clearbit {0}", cbb));
		}

		void Renderer::SetViewportSize(int x, int y, int width, int height) {
			glViewport(x, y, width, height);
			CheckErrors(gcnew String("Tried changing viewport size"));
		}

		void Renderer::SetViewportSize(int x, int y, Vector2 resolution) {
			glViewport(x, y, (int)resolution.x, (int)resolution.y);
			CheckErrors(gcnew String("Tried changing viewport size"));
		}
	}
}