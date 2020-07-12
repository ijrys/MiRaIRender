using ImageTools;
using MiRaIRender.BaseType;
using MiRaIRender.BaseType.LightSource;
using MiRaIRender.BaseType.Materials;
using MiRaIRender.Render.PathTrace;
using ModelLoader;
using System;
using System.IO;
using System.Numerics;
namespace MiRaIRender {
	class Program {
		static void Main(string[] args) {
			string projectPath = null;
			string configPath = null;
			string outputPath = null;

			foreach (string item in args) {
				string iteml = item.ToLower();

				if (iteml.EndsWith(".mrirpro")) {
					projectPath = item;
				}
				else if (iteml.EndsWith(".rconf")) {
					configPath = item;
				}
				else {
					outputPath = item;
				}
			}
			if (string.IsNullOrEmpty(projectPath)) {
				Console.WriteLine("Error Need Project File");
				return;
			}
			if (string.IsNullOrEmpty(configPath)) {
				Console.WriteLine("Error Need Render Config File");
				return;
			}
			if (string.IsNullOrEmpty(outputPath)) {
				Console.WriteLine("Need Output File Name");
				return;
			}
			Console.WriteLine($"Project: {projectPath}");
			Console.WriteLine($"Config : {configPath}");
			Console.WriteLine($"Output : {outputPath}");
			if (File.Exists(outputPath)) {
				string uinp;
				do {
					Console.WriteLine("Output file already exist, overwrite it?(y/n)");
					uinp = Console.ReadLine();
					if (uinp.ToLower() == "n") {
						return;
					}
					if (uinp.ToLower() == "y") {
						break;
					}
				}
				while (true);
			}

			(Scene scene, bool ok) = MiRaIRanderProjectLoader.LoadScene(projectPath);
			if (!ok) {
				Console.WriteLine("project load error");
				return;
			}

			PathTraceRenderOptions options;

			string[] optCont = File.ReadAllLines(configPath);
			(options, ok) = PathTraceRenderOptions.LoadOptions(optCont);
			if (!ok) {
				Console.WriteLine("config load error");
				return;
			}

			PathTraceRender render = new PathTraceRender() {
				Scene = scene,
				Options = options
			};
			DateTime begin = DateTime.Now;
			Color[,] img = render.RenderImg();
			DateTime end = DateTime.Now;
			Console.WriteLine($"Render end, use time {(end - begin)}");

			ColorRGB8[,] simg = ImageTools.ColorConverter.ConvertToRGB8Image(img);
			//string fname = $"A:\\img\\{FileName()}";

			Console.WriteLine("save to " + outputPath);
			ImageSave.ImageSave_PPM(simg, outputPath);
		}
		static string FileName() {
			DateTime time = DateTime.Now;
			string re = time.ToString("yyyyMMdd_HHmmss");
			return re;
		}
	}
}
