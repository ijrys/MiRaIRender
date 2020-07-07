using ImageTools;
using MiRaIRender.BaseType;
using MiRaIRender.BaseType.LightSource;
using MiRaIRender.BaseType.Materials;
using MiRaIRender.Render.PathTrace;
using ModelLoader;
using System;
using System.Numerics;
namespace MiRaIRender {
	class Program {
		static void Main(string[] args) {
			MashTrigle[] objs = OBJLoader.LoadModel("A:\\t2.obj");
			
			MashTrigle obj = objs[0];
			//obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.1f));
			Scene scene = new Scene();
			scene.Objects = new BaseType.SceneObject.RenderObject[] { obj };

			PointLight pLight1 = new PointLight() {
				Color = new Color(30.0f),
				Position = new Vector3f(20, 20, 20)
			};
			PointLight pLight2 = new PointLight() {
				Color = new Color(30.0f),
				Position = new Vector3f(20, 20, -20)
			};
			scene.LightObjects = new BaseType.SceneObject.RenderObject[] { pLight1, pLight2 };

			PathTraceRenderOptions options = new PathTraceRenderOptions() {
				CameraOrigin = new Vector3f(0.0f, 0.0f, 4.0f),
				Width = 800,
				Height = 600,
				FovHorizon = 90,
				TraceDeep = 3,
				SubSampleNumberPerPixel = 512
			};
			PathTraceRender render = new PathTraceRender() {
				Scene = scene,
				Options = options
			};
			Color[,] img = render.RenderImg();
			ColorRGB8[,] simg = ImageTools.ColorConverter.ConvertToRGB8Image(img);
			string fname = $"A:\\img\\{FileName()}";

			Console.WriteLine("save to " + fname + ".ppm");
			ImageSave.ImageSave_PPM(simg, fname + ".ppm");
		}
		static string FileName() {
			DateTime time = DateTime.Now;
			string re = time.ToString("yyyyMMdd_HHmmss");
			return re;
		}
	}
}
