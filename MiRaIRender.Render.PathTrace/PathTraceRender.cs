using MiRaIRender.BaseType;
using MiRaIRender.BaseType.SceneObject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Float = System.Single;
using Math = System.MathF;

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


		Color[,] img;
		RenderBlock[] RenderBlocks;
		int nextBlockId = 0;
		object renderBlocksLock = new object(), imgLock = new object();

		public void RenderABlock() {
			int height = Options.Height,
				width = Options.Width,
				subSampleNumberPerPixel = Options.SubSampleNumberPerPixel,
				traceDeep = Options.TraceDeep;
			Float xmin = Math.Tan(Tools.GetRadianByAngle(Options.FovHorizon / 2));
			Float pixelLength = (xmin * 2) / width;
			Float ymax = xmin / width * height;
			xmin = -xmin;

			Color[,] imgTmp = new Color[32, 32];

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
						Float x = xmin + i * pixelLength;

						Color color = new Color();
						Random random = new Random(Options.RandonSeed + j * width + i);
						for (int k = 0; k < subSampleNumberPerPixel; k++) {
							Float xt = (Float)random.NextDouble() * pixelLength;
							Float yt = (Float)random.NextDouble() * pixelLength;

							Vector3f dir = new Vector3f(x + xt, y + yt, -1.0f).Normalize();
							Ray r = new Ray(Options.CameraOrigin, dir);
							Color c= PathTrace(r, traceDeep, random);
							color += c;
						}
						color /= subSampleNumberPerPixel;
						imgTmp[j - renderBlock.t, i - renderBlock.l] = color;
					}
				}

				//lock (imgLock) {
					for (int j = renderBlock.t; j < renderBlock.b; j++) {
						for (int i = renderBlock.l; i < renderBlock.r; i++) {
							img[j, i] = imgTmp[j - renderBlock.t, i - renderBlock.l];
						}
					}
				//}
			}
		}

		public Color[,] RenderImg() {
			scene.PreRender();

			int height = Options.Height,
				width = Options.Width,
				subSampleNumberPerPixel = Options.SubSampleNumberPerPixel,
				traceDeep = Options.TraceDeep;

			img = new Color[height, width];

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

			Thread th1 = new Thread(RenderABlock);
			Thread th2 = new Thread(RenderABlock);
			Thread th3 = new Thread(RenderABlock);
			th1.Start();
			th2.Start();
			th3.Start();
			th1.Join();
			th2.Join();
			th3.Join();
			//RenderABlock();

			return img;
		}

		private Color PathTrace(Ray ray, int deepLast, Random random) {
			if (deepLast < 0) return Color.Dark;
			RayCastResult rcr = scene.Intersection(ray);

			if (rcr == null) { // 未追踪到任何对象
				return scene.SkyBox.SkyColor(ray);
			}

			Color color;
			//bool Reflection = false;
			Vector2f mapCoords = rcr.obj.UV2XY(rcr.uv);
			Color baseColor = rcr.material.BaseColor.Color(mapCoords);

			int rvallue = random.Next() & 0xFFFF;
			{
				Float rrate = 0.0f;
				Float rtrans = 0.0f;
				if (rcr.material.Refraction.Enable) { // 启用折射
													  //rrate = Tools.FresnelEquation(rcr.normal & (-ray.Direction), )

					//(rcr.material.Metallic.GetMetallic(mapCoords), rcr.normal & (-ray.Direction));
					rtrans = 0.0f;//(1.0f - rrate) * (1.0f - rcr.material.Refraction.GetRefraction(rcr.obj.UV2XY( rcr.uv)));
				}
				else { // 未启用折射
					rrate = Tools.Schlick(rcr.material.Metallic.GetMetallic(mapCoords), rcr.normal & (-ray.Direction));
					rtrans = 0.0f;//(1.0f - rrate) * (1.0f - rcr.material.Refraction.GetRefraction(rcr.obj.UV2XY( rcr.uv)));
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
				Vector3f traceDir = (Tools.Reflect(ray.Direction, rcr.normal) + Tools.RandomPointInSphere() * roughtness).Normalize();
				Ray traceRay = new Ray(rcr.coords, traceDir, rcr.obj);
				color = PathTrace(traceRay, deepLast - 1, random);
				color *= baseColor;
				goto RTPoint;
			}
		#endregion

		#region SpecularColor 折射
		SpecularColor:
			// todo
			{
				//(bool tir, Vector3f traceDir) = Tools.Refract(ray.Direction, rcr.normal, )
				//RayCastResult result = 
				color = Color.Dark;
			}
			goto RTPoint;
		#endregion

		#region DiffuseColor 漫反射
		DiffuseColor:
			{
				Vector3f traceDir = (rcr.normal + Tools.RandomPointInSphere() * rcr.material.Roughness).Normalize();
				Ray traceRay = new Ray(rcr.coords, traceDir, rcr.obj);
				color = PathTrace(traceRay, deepLast - 1, random);
				color *= baseColor;

				Vector3f vitureNormal = rcr.normal; // todo get normal by normal texture
				if (scene.r_LightObjects.Length == 0) {
					Console.WriteLine("LightError");
				}
				foreach (RenderObject item in scene.r_LightObjects) {
					Vector3f apoint = item.SelectALightPoint(rcr.coords);
					Vector3f dir = apoint - rcr.coords;
					if ((rcr.normal & dir) < 0.0f) { // 目标点在法平面下方
						continue;
					}
					dir = dir.Normalize();
					Ray r = new Ray(rcr.coords, dir, rcr.obj);
					RayCastResult rayCastResult = Scene.Intersection(r);
					if (rayCastResult == null || rayCastResult.obj != item) { // 中间遮挡
						continue;
					}

					Vector2f xycoords = rayCastResult.obj.UV2XY(rayCastResult.uv);
					Color light = rayCastResult.material.Light.GetLight(xycoords) * baseColor;

					Float lightIntensity = (r.Direction & vitureNormal) / (rayCastResult.distance * rayCastResult.distance);
					color += light * lightIntensity / (Math.PI * 2);
				}
				goto RTPoint;
			}

		#endregion
		RTPoint:
			return color;
		}
	}
}
