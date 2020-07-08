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
			Scene scene = new Scene();

			MashTrigle[] objs = OBJLoader.LoadModel("A:\\m3_1.obj");
			MashTrigle obj = objs[0];
			scene.Objects.Add(obj);

			objs = OBJLoader.LoadModel("A:\\m3_2.obj");
			obj = objs[0];
			obj.Material.Metallic.Metallic = 1.0f;
			obj.Material.BaseColor = new PureColorMaterialMap(new Color( 0.95f));
			scene.Objects.Add(obj);

			//obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.1f));

			PointLight pLight1 = new PointLight() {
				Position = new Vector3f(-5, 5, 1)
			};
			pLight1.Material.Light.Intensity = new Color(600.0f, 20.0f, 20.0f);
			PointLight pLight2 = new PointLight() {
				Position = new Vector3f(5, 5, 1)
			};
			pLight2.Material.Light.Intensity = new Color(200.0f, 200.0f, 600.0f);
			scene.LightObjects.Add(pLight1);
			scene.LightObjects.Add(pLight2);
			scene.Objects.Add(pLight1);
			scene.Objects.Add(pLight2);

			PathTraceRenderOptions options = new PathTraceRenderOptions() {
				CameraOrigin = new Vector3f(0.0f, 0.7f, 4.0f),
				Width = 400,//100,
				Height = 300,//75,
				FovHorizon = 60,
				TraceDeep = 4,
				SubSampleNumberPerPixel = 128 //16
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
