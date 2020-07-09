﻿using ImageTools;
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

			MashTrigle[] objs;
			MashTrigle obj;
			objs = OBJLoader.LoadModel("A:\\m3_1.obj");
			obj = objs[0];
			//obj.Material.Metallic.Metallic = 0.8f;
			//obj.Material.Roughness = 0.0f;
			//obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.95f, 0.95f, 0.95f));
			scene.Objects.Add(obj);

			objs = OBJLoader.LoadModel("A:\\m3_2.obj");
			obj = objs[0];
			obj.Material.Metallic.Metallic = 0.9f;
			obj.Material.Roughness = 0.2f;
			obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.3f, 0.3f, 0.3f));
			scene.Objects.Add(obj);

			//obj.Material.BaseColor = new PureColorMaterialMap(new Color(0.1f));

			PointLight pLight1 = new PointLight() {
				Position = new Vector3f(-5, 5, 1),
				R = 0.2f,
			};
			pLight1.Material.Light.Intensity = new Color(180.0f, 60.0f, 60.0f);
			PointLight pLight2 = new PointLight() {
				Position = new Vector3f(5, 5, 1),
				R = 0.2f,
			};
			pLight2.Material.Light.Intensity = new Color(60.0f, 60.0f, 180.0f);
			scene.LightObjects.Add(pLight1);
			scene.LightObjects.Add(pLight2);
			scene.Objects.Add(pLight1);
			scene.Objects.Add(pLight2);

			//PointLight pLight;
			//pLight = new PointLight() {
			//	Position = new Vector3f(5, 5, 1)
			//};
			//pLight.Material.Light.Intensity = new Color(120.0f, 120.0f, 120.0f);
			//scene.Objects.Add(pLight);
			//scene.LightObjects.Add(pLight);

			//pLight = new PointLight() {
			//	Position = new Vector3f(-5, 5, 1)
			//};
			//pLight.Material.Light.Intensity = new Color(120.0f, 120.0f, 120.0f);
			//scene.Objects.Add(pLight);
			//scene.LightObjects.Add(pLight);

			PathTraceRenderOptions options = new PathTraceRenderOptions() {
				RandonSeed = 20200708,
				CameraOrigin = new Vector3f(0.0f, 0.7f, 4.0f),
				Width = 1200,//100,
				Height = 900,//75,
				FovHorizon = 60,
				TraceDeep = 4,
				SubSampleNumberPerPixel = 96 //16
			};
			PathTraceRender render = new PathTraceRender() {
				Scene = scene,
				Options = options
			};
			DateTime begin = DateTime.Now;
			Color[,] img = render.RenderImg();
			DateTime end = DateTime.Now;
			Console.WriteLine($"Render end, use time {(end - begin)}");

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
