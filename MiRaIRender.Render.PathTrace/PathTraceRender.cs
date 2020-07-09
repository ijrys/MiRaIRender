using MiRaIRender.BaseType;
using MiRaIRender.BaseType.SceneObject;
using System;
using Float = System.Single;
using Math = System.MathF;

namespace MiRaIRender.Render.PathTrace {
	public class PathTraceRender {

		private Scene scene;

		public Scene Scene {
			get => scene;
			set => scene = value;
		}

		public PathTraceRenderOptions Options;

		public Color[,] RenderImg() {
			scene.PreRender();

			int height = Options.Height,
				width = Options.Width,
				subSampleNumberPerPixel = Options.SubSampleNumberPerPixel,
				traceDeep = Options.TraceDeep;

			Float xmin = Math.Tan(Tools.GetRadianByAngle(Options.FovHorizon / 2));
			Float pixelLength = (xmin * 2) / width;
			Float ymax = xmin / width * height;
			xmin = -xmin;
			Random random = Options.Random;

			Color[,] img = new Color[height, width];

			for (int j = 0; j < height; j++) {
				Console.WriteLine("rendering " + j);
				Float y = ymax - j * pixelLength;
				for (int i = 0; i < width; i++) {
					Float x = xmin + i * pixelLength;

					Color color = new Color();
					for (int k = 0; k < subSampleNumberPerPixel; k++) {
						Float xt = (Float)random.NextDouble() * pixelLength;
						Float yt = (Float)random.NextDouble() * pixelLength;

						Vector3f dir = new Vector3f(x + xt, y + yt, -1.0f).Normalize();
						Ray r = new Ray(Options.CameraOrigin, dir);
						color += PathTrace(r, traceDeep) / subSampleNumberPerPixel;
					}

					img[j, i] = color;
				}
			}

			return img;
		}

		private Color PathTrace(Ray ray, int deepLast) {
			if (deepLast < 0) return Color.Dark;
			RayCastResult rcr = scene.Intersection(ray);

			if (!rcr.happened) { // 未追踪到任何对象
				return scene.SkyBox.SkyColor(ray);
			}

			Color color;
			Vector2f mapCoords = rcr.obj.UV2XY(rcr.uv);
			Color baseColor = rcr.material.BaseColor.Color(mapCoords);

			int rvallue = Options.Random.Next() & 0xFFFF;
			{
				Float rrate = Tools.Schlick(rcr.material.Metallic.GetMetallic(mapCoords), rcr.normal & (-ray.Direction));
				Float rtrans = (1 - rrate) * rcr.material.Specular;
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
				Ray traceRay = new Ray(rcr.coords + traceDir * 0.0002f, traceDir);
				color = PathTrace(traceRay, deepLast - 1) * baseColor;

				goto RTPoint;
			}
		#endregion

		#region SpecularColor 折射
		SpecularColor:
			// todo
			color = Color.Dark;
			goto RTPoint;
		#endregion

		#region DiffuseColor 漫反射
		DiffuseColor:
			{
				Vector3f traceDir = (rcr.normal + Tools.RandomPointInSphere() * rcr.material.Roughness).Normalize();
				Ray traceRay = new Ray(rcr.coords + traceDir * 0.0002f, traceDir);
				color = PathTrace(traceRay, deepLast - 1) * baseColor;

				Vector3f vitureNormal = rcr.normal; // todo get normal by normal texture
				foreach (RenderObject item in scene.r_LightObjects) {
					Vector3f apoint = item.SelectALightPoint(rcr.coords);
					Vector3f dir = apoint - rcr.coords;
					if ((rcr.normal & dir) < 0.0f) { // 目标点在法平面下方
						continue;
					}
					dir = dir.Normalize();
					Ray r = new Ray(rcr.coords + dir * 0.0002f, dir.Normalize());
					RayCastResult rayCastResult = Scene.Intersection(r);
					if (!rayCastResult.happened || rayCastResult.obj != item) { // 中间遮挡
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
