using MiRaIRender.BaseType;
using MiRaIRender.BaseType.LightSource;
using MiRaIRender.BaseType.Materials;
using MiRaIRender.Render.PathTrace;
using ModelLoader;
using System;
using System.IO;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace GenerateProgect {
	class Program {
		static Scene Test1() {
			Scene scene = new Scene();

			MashTrigle[] objs;
			MashTrigle obj;
			objs = OBJLoader.LoadModel("A:\\m3_1.obj");
			//objs = OBJLoaderWNormalSmooth.LoadModel("A:\\m3_1.obj");
			obj = objs[0];
			//obj.Material.Metallic.Metallic = 0.3f;
			obj.Material.Roughness = 0.8f;
			//obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.95f, 0.95f, 0.95f));
			scene.Objects.Add(obj);

			objs = OBJLoader.LoadModel("A:\\m3_2.obj");
			obj = objs[0];
			obj.Material.Metallic.Metallic = 0.9f;
			obj.Material.Roughness = 0.2f;
			obj.Material.BaseColor = new PureColorMaterialMap(new RGBSpectrum(0.3f, 0.3f, 0.3f));
			scene.Objects.Add(obj);

			PointLight pLight1 = new PointLight() {
				Position = new Vector3f(-5, 5, 0),
				R = 0.2f,
			};
			pLight1.Material.Light.Intensity = new RGBSpectrum(200.0f, 10.0f, 10.0f);
			PointLight pLight2 = new PointLight() {
				Position = new Vector3f(5, 5, -1),
				R = 0.2f,
			};
			pLight2.Material.Light.Intensity = new RGBSpectrum(10.0f, 10.0f, 200.0f);
			scene.Objects.Add(pLight1);
			scene.Objects.Add(pLight2);

			PointLight pLight;
			pLight = new PointLight() {
				Position = new Vector3f(0, 5, 2)
			};
			pLight.Material.Light.Intensity = new RGBSpectrum(30.0f);
			scene.Objects.Add(pLight);

			return scene;
		}

		static Scene Test3() {
			Scene scene = new Scene();

			MashTrigle[] objs;
			MashTrigle obj;
			//objs = OBJLoader.LoadModel("A:\\m3_1.obj");
			objs = OBJLoaderWNormalSmooth.LoadModel("A:\\m3_1.obj");
			obj = objs[0];
			obj.Material.Roughness = 0.8f;
			scene.Objects.Add(obj);

			objs = OBJLoader.LoadModel("A:\\m3_2.obj");
			obj = objs[0];
			obj.Material.Metallic.Metallic = 0.9f;
			obj.Material.Roughness = 0.8f;
			obj.Material.BaseColor = new PureColorMaterialMap(new RGBSpectrum(0.3f, 0.3f, 0.3f));
			scene.Objects.Add(obj);

			objs = OBJLoaderWNormalSmooth.LoadModel("A:\\m3_b1.obj");
			obj = objs[0];
			obj.Material.Metallic.Metallic = 0.0f;
			obj.Material.Refraction.Enable = true;
			obj.Material.Refraction.Refraction = 0.9f;
			obj.Material.Refraction.IOR = 1.4f;
			obj.Material.Roughness = 0.1f;
			obj.Material.BaseColor = new PureColorMaterialMap(new RGBSpectrum(0.9f, 0.95f, 0.9f));
			scene.Objects.Add(obj);

			//obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.1f));

			PointLight pLight1 = new PointLight() {
				Position = new Vector3f(-5, 5, 0),
				R = 0.2f,
			};
			pLight1.Material.Light.Intensity = new RGBSpectrum(200.0f, 10.0f, 10.0f);
			PointLight pLight2 = new PointLight() {
				Position = new Vector3f(5, 5, -1),
				R = 0.2f,
			};
			pLight2.Material.Light.Intensity = new RGBSpectrum(10.0f, 10.0f, 200.0f);
			//scene.LightObjects.Add(pLight1);
			//scene.LightObjects.Add(pLight2);
			scene.Objects.Add(pLight1);
			scene.Objects.Add(pLight2);

			PointLight pLight;
			pLight = new PointLight() {
				Position = new Vector3f(0, 5, 2)
			};
			pLight.Material.Light.Intensity = new RGBSpectrum(30.0f);
			scene.Objects.Add(pLight);
			//scene.LightObjects.Add(pLight);

			return scene;
		}
		static Scene Test2() {
			Scene scene = new Scene();
			MashTrigle[] objs;
			MashTrigle obj;
			objs = OBJLoader.LoadModel("A:\\t2.obj");
			obj = objs[0];
			//obj.Material.Metallic.Metallic = 0.3f;
			obj.Material.Roughness = 0.8f;
			//obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.95f, 0.95f, 0.95f));
			scene.Objects.Add(obj);

			PointLight pLight1 = new PointLight() {
				Position = new Vector3f(-5, 5, 0),
				R = 0.2f,
			};
			pLight1.Material.Light.Intensity = new RGBSpectrum(200.0f, 10.0f, 10.0f);
			PointLight pLight2 = new PointLight() {
				Position = new Vector3f(5, 5, -1),
				R = 0.2f,
			};
			pLight2.Material.Light.Intensity = new RGBSpectrum(10.0f, 10.0f, 200.0f);
			//scene.LightObjects.Add(pLight1);
			//scene.LightObjects.Add(pLight2);
			scene.Objects.Add(pLight1);
			scene.Objects.Add(pLight2);

			return scene;
		}

		static Scene CornellBox() {
			Material white = new Material() {
				BaseColor = new PureColorMaterialMap(new RGBSpectrum(0.725f, 0.71f, 0.68f)),
			};
			Material red = new Material() {
				BaseColor = new PureColorMaterialMap(new RGBSpectrum(0.63f, 0.065f, 0.05f)),
			};
			Material green = new Material() {
				BaseColor = new PureColorMaterialMap(new RGBSpectrum(0.14f, 0.45f, 0.091f)),
			};
			Material light = new Material() {
				BaseColor = new PureColorMaterialMap(new RGBSpectrum(0.65f)),
			};
			light.Light.Enable = true;
			light.Light.Intensity = 8.0f * new RGBSpectrum(0.747f + 0.058f, 0.747f + 0.258f, 0.747f) + 15.6f * new RGBSpectrum(0.740f + 0.287f, 0.740f + 0.160f, 0.740f) + 18.4f * new RGBSpectrum(0.737f + 0.642f, 0.737f + 0.159f, 0.737f);


			Scene scene = new Scene();
			MashTrigle obj;

			obj = OBJLoader.LoadModel("A:\\cornellbox\\floor.obj", true)[0];
			obj.Material = white;
			scene.Objects.Add(obj);

			obj = OBJLoader.LoadModel("A:\\cornellbox\\shortbox.obj", true)[0];
			obj.Material = white;
			scene.Objects.Add(obj);

			obj = OBJLoader.LoadModel("A:\\cornellbox\\tallbox.obj", true)[0];
			obj.Material = white;
			scene.Objects.Add(obj);

			obj = OBJLoader.LoadModel("A:\\cornellbox\\left.obj", true)[0];
			obj.Material = red;
			scene.Objects.Add(obj);

			obj = OBJLoader.LoadModel("A:\\cornellbox\\right.obj", true)[0];
			obj.Material = green;
			scene.Objects.Add(obj);

			obj = OBJLoader.LoadModel("A:\\cornellbox\\light.obj", true)[0];
			obj.Material = light ;
			scene.Objects.Add(obj);

			return scene;
		}

		static void Main(string[] args) {
			//MiRaIRanderProjectSaver.Save(Test1(), "A:\\scenes", "testS");
			MiRaIRanderProjectSaver.Save(CornellBox(), "A:\\scenes", "CornellBox");

			//Scene scene = MiRaIRanderProjectLoader.LoadScene("A:\\scenes\\testS\\testS.mrirpro");
			//Console.WriteLine(scene.Objects.Count);

			//PathTraceRenderOptions options = new PathTraceRenderOptions() {
			//	RandonSeed = 20200708,
			//	CameraOrigin = new Vector3f(0.0f, 0.7f, 4.0f),
			//	Width = 1200,//100,
			//	Height = 900,//75,
			//	FovHorizon = 60,
			//	TraceDeep = 4,
			//	SubSampleNumberPerPixel = 160 //16
			//};
			//string[] optcont = PathTraceRenderOptions.SaveOptions(options);
			//File.WriteAllLines("A:\\scenes\\output.rconf", optcont);
		}
	}
}
