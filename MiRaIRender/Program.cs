using MiRaIRender.BaseType;
using MiRaIRender.BaseType.Materials;
using MiRaIRender.Render.PathTrace;
using ModelLoader;
using System;
using System.Numerics;
namespace MiRaIRender {
	class Program {
		static void Main(string[] args) {
			MashTrigle[] objs = OBJLoader.LoadModel("A:\\m.obj");
			MashTrigle obj = objs[0];
			Scene scene = new Scene();
			scene.Objects = new BaseType.SceneObject.RenderObject[] { obj };

			PathTraceRenderOptions options = new PathTraceRenderOptions();
			PathTraceRender render = new PathTraceRender() {
				Scene = scene,
				Options = options
			};
			Color[,] img = render.RenderImg();

		}
	}
}
