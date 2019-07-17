#pragma once
#include "GL\glew.h"
#include "Vectors.h"

using namespace S3DECore::Math;

namespace S3DECore {
	namespace Graphics {
		public enum class RendererCapability {
			AlphaTest = GL_ALPHA_TEST,
			CullFace = GL_CULL_FACE,
			DepthTest = GL_DEPTH_TEST,
		};

		public enum class ClearBufferBit {
			Color = GL_COLOR_BUFFER_BIT,
			Depth = GL_DEPTH_BUFFER_BIT,
			Stencil = GL_STENCIL_BUFFER_BIT,
			Accum = GL_ACCUM_BUFFER_BIT
		};

		public delegate void ResolutionChangedCallback(Vector2 oldRes, Vector2 newRes);

		public ref class Renderer abstract sealed {
		public:
			static void Init(Vector2 displayRes, Vector2 renderRes);
			static void SetViewportSize(int x, int y, int width, int height);
			static void SetViewportSize(int x, int y, Vector2 resolution);
			static int GetParami(int param);
			static void Enable(RendererCapability cap);
			static void Disable(RendererCapability cap);
			static void Clear(ClearBufferBit cbb);
			static void UpdateEvents();
			static ResolutionChangedCallback ^OnRenderResolutionChanged,^OnDisplayResolutionChanged;

			static property Vector2 DisplayResolution {
				Vector2 get() { return currDispRes; }
				void set(Vector2 v) { displayReyChanged = true; newDispRes = v; }
			}
			static property Vector2 RenderResolution {
				Vector2 get() { return currRendRes; }
				void set(Vector2 v) { renderResChanged = true; newRendRes = v; }
			}

			static property bool VSyncEnabled {
				bool get() { return vSyncEnabled; }
				void set(bool b) { vSyncEnabled = b; }
			}

			static bool CheckErrors() {
				prevErr = glGetError();
				return prevErr != GL_NO_ERROR;
			}

			static property int LatestError {
				int get() { return prevErr; }
			}

			static void CheckErrors(String^ errorMsg) {
				if (CheckErrors()) {
					throw gcnew System::Exception(String::Format("Renderer Error {0}, Error message: {1}",prevErr,errorMsg));
				}
			}

		private:
			static GLenum prevErr;
			static int dWidth, dHeight, rWidth, rHeight;
			static bool vSyncEnabled,displayReyChanged,renderResChanged;
			static Vector2 currDispRes, currRendRes,newDispRes,newRendRes;
		};
	}
}