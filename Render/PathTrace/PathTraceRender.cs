#define ReflactDebug

using MiRaIRender.BaseType;
using MiRaIRender.BaseType.SceneObject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Float = System.Single;
using Math = System.MathF;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;
using MiRaIRender.BaseType.Spectrum;

namespace MiRaIRender.Render.PathTrace {
	public class PathTraceRender {

		private struct RenderBlock {
			public int l;
			public int r;
			public int t;
			public int b;
		}

		private Scene scene;

		public Scene Scene {
			get => scene;
			set => scene = value;
		}

		public PathTraceRenderOptions Options;


		RGBSpectrum[,] img;
#if ReflactDebug
		public Float[,] debugImg;
#endif
		RenderBlock[] RenderBlocks;
		int nextBlockId = 0;
		object renderBlocksLock = new object();


		public RGBSpectrum[,] RenderImg() {
			scene.PreRender();

			int height = Options.Height,
				width = Options.Width,
				subSampleNumberPerPixel = Options.SubSampleNumberPerPixel,
				traceDeep = Options.TraceDeep;

			img = new RGBSpectrum[height, width];
			debugImg = new Float[height, width];

			{ // 准备渲染块
				List<RenderBlock> renderBlocks = new List<RenderBlock>();
				const int blocksize = 32;
				for (int blockj = 0; blockj * blocksize < height; blockj++) {
					for (int blocki = 0; blocki * blocksize < width; blocki++) {
						RenderBlock block = new RenderBlock();
						block.l = blocki * blocksize;
						block.t = blockj * blocksize;
						block.r = block.l + blocksize;
						if (block.r > width) { block.r = width; }
						block.b = block.t + blocksize;
						if (block.b > height) { block.b = height; }
						renderBlocks.Add(block);
					}
				}
				RenderBlocks = renderBlocks.ToArray();
				nextBlockId = 0;
			}

			//Thread th1 = new Thread(RenderABlock);
			//th1.Start();
			//Thread th2 = new Thread(RenderABlock);
			//th2.Start();
			//Thread th3 = new Thread(RenderABlock);
			//th3.Start();
			//th1.Join();
			//th2.Join();
			//th3.Join();
			RenderABlock();

			return img;
		}

		private void RenderABlock() {
			int height = Options.Height,
				width = Options.Width,
				subSampleNumberPerPixel = Options.SubSampleNumberPerPixel,
				traceDeep = Options.TraceDeep;
			Float xmin = Math.Tan(Tools.GetRadianByAngle(Options.FovHorizon / 2));
			Float pixelLength = (xmin * 2) / width;
			Float ymax = xmin / width * height;
			xmin = -xmin;

			RGBSpectrum[,] imgTmp = new RGBSpectrum[1, 32];

			while (RenderBlocks != null) {
				int blockId;
				lock (renderBlocksLock) {
					blockId = nextBlockId;
					nextBlockId++;
				}
				if (blockId >= RenderBlocks.Length) {
					break;
				}
				RenderBlock renderBlock = RenderBlocks[blockId];
				Console.WriteLine($"rending block {blockId} : {renderBlock.l}, {renderBlock.t} => {renderBlock.r}, {renderBlock.b}");
				for (int j = renderBlock.t; j < renderBlock.b; j++) {
					Float y = ymax - j * pixelLength;
					for (int i = renderBlock.l; i < renderBlock.r; i++) {
						//if (i == 83 && j == 54) {
						//	Console.WriteLine("debug");
						//}
						Float x = xmin + i * pixelLength;

						RGBSpectrum color = new RGBSpectrum();
						Float reflactRate = 0.0f;
						Random random = new Random(Options.RandonSeed + j * width + i);
						for (int k = 0; k < subSampleNumberPerPixel; k++) {
							Float xt = (Float)random.NextDouble() * pixelLength;
							Float yt = (Float)random.NextDouble() * pixelLength;

							Vector3f dir = Vector3f.Normalize(new Vector3f(x + xt, y + yt, -1.0f));
							Ray r = new Ray(Options.CameraOrigin, dir);
#if ReflactDebug
							(RGBSpectrum c, Float rrate) = PathTrace(r, traceDeep, random);
#else
							Color c = PathTrace(r, traceDeep, random);
#endif
							color += c;
							reflactRate += rrate;
						}
						color /= subSampleNumberPerPixel;
						imgTmp[0, i - renderBlock.l] = color;
						debugImg[j, i] = reflactRate / subSampleNumberPerPixel;
					}
					int imgbegin = width * j + renderBlock.l;
					Array.Copy(imgTmp, 0, img, imgbegin, renderBlock.r - renderBlock.l);
				}
			}
		}

#if ReflactDebug
		private (RGBSpectrum, Float) PathTrace(Ray ray, int deepLast, Random random) {
			Float reflactRate = 0.0f;
#else
		private Color PathTrace(Ray ray, int deepLast, Random random) {
#endif
			if (deepLast < 0) {
#if ReflactDebug
				return (RGBSpectrum.Dark, reflactRate);
#else
				return Color.Dark;
#endif
			}
			RayCastResult rcr = scene.Intersection(ray);

			if (rcr == null) { // 未追踪到任何对象
#if ReflactDebug
				return (scene.SkyBox.SkyColor(ray), reflactRate);
#else
				return scene.SkyBox.SkyColor(ray);
#endif

			}

			RGBSpectrum color;
			//bool Reflection = false;
			Vector2f mapCoords = rcr.obj.UV2XY(rcr.uv);
			RGBSpectrum baseColor = rcr.material.BaseColor.Color(mapCoords);

			int rvallue = random.Next() & 0xFFFF;
			{
				Float rrate = 0.0f;
				Float rtrans = 0.0f;
				if (rcr.material.Refraction.Enable) { // 启用折射
					Float niOverNt = ray.IOR / rcr.IOR;
					rrate = Tools.FresnelEquation(Vector3f.Dot(rcr.normal, (-ray.Direction)), niOverNt);

					rtrans = (1.0f - rrate) * (rcr.material.Refraction.GetRefraction(mapCoords));
#if ReflactDebug
					reflactRate = rrate;
#endif
				}
				else { // 未启用折射
					rrate = Tools.Schlick(rcr.material.Metallic.GetMetallic(mapCoords), Vector3f.Dot(rcr.normal, -ray.Direction));
					rtrans = 0.0f;
#if ReflactDebug
					reflactRate = rrate;
#endif
				}
				int rmaxv = (int)(0xFFFF * rrate);
				int transmaxv = (int)(0xFFFF * rtrans) + rmaxv;

				if (rvallue < rmaxv) { // 镜面反射
					goto MetallicColor;
				}
				if (rvallue < transmaxv) { // 折射
					goto SpecularColor;
				}
				goto DiffuseColor; // 漫反射
			}

		#region MetallicColor 镜面反射
		MetallicColor:
			{
				Float roughtness = rcr.material.Roughness;
				Vector3f traceDir = Vector3f.Normalize(Tools.Reflect(ray.Direction, rcr.normal) + Tools.RandomPointInSphere() * roughtness);
				Ray traceRay = new Ray(rcr.coords, traceDir, rcr.obj, rcr.material.Refraction.IOR);
				(color, _) = PathTrace(traceRay, deepLast - 1, random);
				color *= baseColor;
				goto RTPoint;
			}
		#endregion

		#region SpecularColor 折射
		SpecularColor:
			{
				Float niOverNt = ray.IOR / rcr.IOR;
				Vector3f refdir = Tools.Refract(ray.Direction, rcr.normal, niOverNt);
				Ray traceRay = new Ray(rcr.coords, refdir, rcr.obj, rcr.IOR);
				(color, _) = PathTrace(traceRay, deepLast - 1, random);
				color *= baseColor;
			}
			goto RTPoint;
		#endregion

		#region DiffuseColor 漫反射
		DiffuseColor:
			{
				Vector3f traceDir = Vector3f.Normalize(rcr.normal + Tools.RandomPointInSphere() * rcr.material.Roughness);
				Ray traceRay = new Ray(rcr.coords, traceDir, rcr.obj, rcr.material.Refraction.IOR);
				(color, _) = PathTrace(traceRay, deepLast - 1, random);
				color *= baseColor;

				Vector3f vitureNormal = rcr.normal; // todo get normal by normal texture

				foreach (RenderObject item in scene.r_LightObjects) {
					Vector3f apoint = item.SelectALightPoint(rcr.coords);
					Vector3f dir = apoint - rcr.coords;
					if (Vector3f.Dot(rcr.normal, dir) < 0.0f) { // 目标点在法平面下方
						continue;
					}
					dir = Vector3f.Normalize(dir);
					Ray r = new Ray(rcr.coords, dir, rcr.obj, 1.0f);
					RayCastResult rayCastResult = Scene.Intersection(r);
					if (rayCastResult == null || rayCastResult.obj != item) { // 中间遮挡
						continue;
					}

					Vector2f xycoords = rayCastResult.obj.UV2XY(rayCastResult.uv);
					RGBSpectrum light = rayCastResult.material.Light.GetLight(xycoords) * baseColor;

					Float lightIntensity = Vector3f.Dot(r.Direction, vitureNormal) / (rayCastResult.distance * rayCastResult.distance);
					color += light * lightIntensity / (Math.PI * 2);
				}
				goto RTPoint;
			}

		#endregion
		RTPoint:
			if (rcr.material.Light.Enable) {
				Float lightIntensity = Vector3f.Dot(-ray.Direction, rcr.normal) / (rcr.distance * rcr.distance) / (Math.PI * 2.0f);
				color += rcr.material.Light.GetLight(mapCoords) * lightIntensity;
			}
#if ReflactDebug
			return (color, reflactRate);
#else
			return color;
#endif
		}
	}
}
