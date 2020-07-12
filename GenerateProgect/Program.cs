using MiRaIRender.BaseType;
using MiRaIRender.BaseType.LightSource;
using MiRaIRender.BaseType.Materials;
using MiRaIRender.Render.PathTrace;
using ModelLoader;
using System;
using System.IO;

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
			obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.3f, 0.3f, 0.3f));
			scene.Objects.Add(obj);

			PointLight pLight1 = new PointLight() {
				Position = new Vector3f(-5, 5, 0),
				R = 0.2f,
			};
			pLight1.Material.Light.Intensity = new Color(200.0f, 10.0f, 10.0f);
			PointLight pLight2 = new PointLight() {
				Position = new Vector3f(5, 5, -1),
				R = 0.2f,
			};
			pLight2.Material.Light.Intensity = new Color(10.0f, 10.0f, 200.0f);
			scene.Objects.Add(pLight1);
			scene.Objects.Add(pLight2);

			PointLight pLight;
			pLight = new PointLight() {
				Position = new Vector3f(0, 5, 2)
			};
			pLight.Material.Light.Intensity = new Color(30.0f);
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
			obj.Material.Metallic.Metallic = 0.9f;
			obj.Material.Roughness = 0.5f;
			obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.95f, 0.95f, 0.95f));
			scene.Objects.Add(obj);

			objs = OBJLoader.LoadModel("A:\\m3_2.obj");
			obj = objs[0];
			obj.Material.Metallic.Metallic = 0.9f;
			obj.Material.Roughness = 0.8f;
			obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.3f, 0.3f, 0.3f));
			scene.Objects.Add(obj);

			//obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.1f));

			PointLight pLight1 = new PointLight() {
				Position = new Vector3f(-5, 5, 0),
				R = 0.2f,
			};
			pLight1.Material.Light.Intensity = new Color(200.0f, 10.0f, 10.0f);
			PointLight pLight2 = new PointLight() {
				Position = new Vector3f(5, 5, -1),
				R = 0.2f,
			};
			pLight2.Material.Light.Intensity = new Color(10.0f, 10.0f, 200.0f);
			//scene.LightObjects.Add(pLight1);
			//scene.LightObjects.Add(pLight2);
			scene.Objects.Add(pLight1);
			scene.Objects.Add(pLight2);

			PointLight pLight;
			pLight = new PointLight() {
				Position = new Vector3f(0, 5, 2)
			};
			pLight.Material.Light.Intensity = new Color(30.0f);
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
			pLight1.Material.Light.Intensity = new Color(200.0f, 10.0f, 10.0f);
			PointLight pLight2 = new PointLight() {
				Position = new Vector3f(5, 5, -1),
				R = 0.2f,
			};
			pLight2.Material.Light.Intensity = new Color(10.0f, 10.0f, 200.0f);
			//scene.LightObjects.Add(pLight1);
			//scene.LightObjects.Add(pLight2);
			scene.Objects.Add(pLight1);
			scene.Objects.Add(pLight2);

			return scene;
		}

		static void Main(string[] args) {
			//MiRaIRanderProjectSaver.Save(Test1(), "A:\\scenes", "testS");
			//MiRaIRanderProjectSaver.Save(Test1(), "A:\\scenes", "testH");

			//Scene scene = MiRaIRanderProjectLoader.LoadScene("A:\\scenes\\testS\\testS.mrirpro");
			//Console.WriteLine(scene.Objects.Count);

			PathTraceRenderOptions options = new PathTraceRenderOptions() {
				RandonSeed = 20200708,
				CameraOrigin = new Vector3f(0.0f, 0.7f, 4.0f),
				Width = 1200,//100,
				Height = 900,//75,
				FovHorizon = 60,
				TraceDeep = 4,
				SubSampleNumberPerPixel = 160 //16
			};
			string[] optcont = PathTraceRenderOptions.SaveOptions(options);
			File.WriteAllLines("A:\\scenes\\output.rconf", optcont);
		}
	}
}
