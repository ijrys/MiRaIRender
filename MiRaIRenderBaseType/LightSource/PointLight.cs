using MiRaIRender.BaseType.Materials;
using MiRaIRender.BaseType.SceneObject;
using System;
using System.Collections.Generic;
using System.Text;
using Math = System.MathF;
using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.BaseType.LightSource {
	public class PointLight : SceneObject.SceneObjectA {

		/// <summary>
		/// 光源位置
		/// </summary>
		public Vector3f Position;

		public Float R = 0.1f;

		public PointLight() {
			Material = new Material();
			Material.Light.Enable = true;
			Material.Light.Intensity = new Color(10.0f);
		}

		public override RayCastResult Intersection(Ray ray, float nowbest) {
			{
				float distance = (Position - ray.Origin).Length();
				if (nowbest + R < distance) {
					return null;
				}
			}
			Vector3f A = ray.Origin, B = ray.Direction, C = Position;
			Float a = Vector3f.Dot(B, B);
			Float b = Vector3f.Dot(B, (A - C)) * 2.0f;
			Float c = (A - C).LengthSquared() - R * R;

			float drt = b * b - 4 * a * c;
			if (drt < 0) {
				return null;
			}
			drt = Math.Sqrt(drt);
			float x1 = (-b + drt) / a / 2;
			float x2 = (-b - drt) / a / 2;
			if (x1 < 0 && x2 < 0) {
				return null;
			}

			float d;
			if (x1 > 0 && x2 > 0) {
				d = Math.Max(x1, x2);
			}
			else if (x1 > 0) {
				d = x1;
			}
			else {
				d = x2;
			}

			RayCastResult result = new RayCastResult();
			//result.happened = true;
			result.obj = this;
			result.material = Material;
			result.coords = ray.Origin + ray.Direction * d;
			result.distance = d;
			result.normal = Vector3f.Normalize(result.coords - Position);

			return result;
		}

		public override Vector2f UV2XY(Vector2f uv) {
			return new Vector2f();
		}

		public override Vector3f SelectALightPoint(Vector3f rayFrom) {
			return Tools.RandomPointInSphere() * R + Position;
		}
	}
}
