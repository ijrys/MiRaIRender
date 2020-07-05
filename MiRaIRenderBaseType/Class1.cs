//using System.Numerics;

//using Vector3f = System.Numerics.Vector3;
using MiRaIRender.BaseType.SceneObject;
using System;
using Float = System.Single;
using Math = System.MathF;

namespace MiRaIRender.BaseType {


	public class PathTraceRender {
		private int width = 1280, height = 720;
		private Float fovHorizon = 135;
		private Scene scene;
		private int subSampleNumberPerPixel = 8;
		private int traceDeep = 4;
		private Random random = new Random();

		public int Width {
			get => width;
			set {
				if (value < 1) value = 1;
				width = value;
			}
		}

		public int Height {
			get => height;
			set {
				if (value < 1) value = 1;
				height = value;
			}
		}

		public Float FovHorizon {
			get => fovHorizon;
			set {
				if (value < 1) value = 1;
				else if (value > 170) value = 170;
				fovHorizon = 170;
			}
		}

		public Scene Scene {
			get => scene;
			set => scene = value;
		}

		/// <summary>
		/// 每像素子采样数量
		/// </summary>
		public int SubSampleNumberPerPixel {
			get => subSampleNumberPerPixel;
			set {
				if (value < 1) value = 1;
				subSampleNumberPerPixel = value;
			}
		}

		/// <summary>
		/// 追踪深度
		/// </summary>
		public int TraceDeep {
			get => traceDeep;
			set {
				if (value < 1) value = 1;
				traceDeep = value;
			}
		}

		public Color[,] RenderImg(Scene scene) {
			Float xmin = Math.Tan(Tools.GetRadianByAngle(fovHorizon / 2));
			Float pixelLength = (xmin * 2) / width;
			Float ymax = xmin / width * height;
			xmin = -xmin;
			Random random = new Random();

			Color[,] img = new Color[height, width];
			for (int j = 0; j < height; j++) {
				Float y = ymax - j * pixelLength;
				for (int i = 0; i < width; i++) {
					Float x = xmin + i * pixelLength;

					Color color = new Color();
					for (int k = 0; k < subSampleNumberPerPixel; k++) {
						Float xt = (Float)random.NextDouble();
						Float yt = (Float)random.NextDouble();

						Ray r = new Ray(new Vector3f(0, 0, 0), new Vector3f(x + xt, y + yt, -1).Normalize());
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

			double rvallue = random.NextDouble();
			{
				Float rrate = Tools.Schlick(rcr.material.Metallic.GetMetallic(mapCoords), rcr.normal & (-ray.Direction));
				Float rtrans = (1 - rrate) * rcr.material.Specular + rrate;

				if (rvallue < rrate) { // 镜面反射
					goto MetallicColor;
				}
				if (rvallue < rtrans) { // 折射
					goto SpecularColor;
				}
				goto DiffuseColor; // 漫反射
			}



		#region MetallicColor 镜面反射
		MetallicColor:
			{
				Vector3f traceDir = (rcr.normal + Tools.RandomPointInSphere() * rcr.material.Roughness).Normalize();
				Ray traceRay = new Ray(rcr.coords, traceDir);
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
				Ray traceRay = new Ray(rcr.coords, traceDir);
				color = PathTrace(traceRay, deepLast - 1) * baseColor;

				Vector3f vitureNormal = rcr.normal; // todo get normal by normal texture
				foreach (RenderObject item in scene.LightObjects) {
					Vector3f apoint = item.SelectALightPoint(rcr.coords);
					Vector3f dir = apoint - rcr.coords;
					if ((rcr.normal & dir) < 0.0f) { // 目标点在法平面下方
						continue;
					}

					Ray r = new Ray(rcr.coords, dir.Normalize());
					RayCastResult rayCastResult = Scene.Intersection(r);
					if (!rayCastResult.happened || rayCastResult.obj != apoint) { // 中间遮挡
						continue;
					}

					Vector2f xycoords = rayCastResult.obj.UV2XY(rayCastResult.uv);
					Color light = rayCastResult.material.Light.GetLight(xycoords);

					Float lightIntensity = (r.Direction & vitureNormal) / rayCastResult.distance;
					color += light * lightIntensity;
				}

				color /= Math.PI * 2;

				goto RTPoint;
			}

		#endregion
		RTPoint:
			return color;
		}
	}
}
