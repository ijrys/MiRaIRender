using MiRaIRender.BaseType.Materials;
using MiRaIRender.BaseType.SceneObject;
using System;
using System.Collections.Generic;
using System.Text;
using Math = System.MathF;
using Float = System.Single;

namespace MiRaIRender.BaseType.LightSource {
	public class PointLight : RenderObject {
		
		/// <summary>
		/// 光源位置
		/// </summary>
		public Vector3f Position;

		public Material Material {
			get;
			private set;
		}

		public PointLight() {
			Material = new Material();
			Material.Light.Enable = true;
			Material.Light.Intensity = new Color(10.0f);
		}

		public override RayCastResult Intersection(Ray ray) {
			Vector3f dir = Position - ray.Origin;
			Vector3f t = dir * ray.Direction_Inv;
			RayCastResult result = new RayCastResult();
			if (t.x < 0 || t.y < 0 || t.z < 0) {
				return result;
			}
			if (Tools.FloatClosely(t.x, t.y, 0.00001f) &&
				Tools.FloatClosely(t.x, t.z, 0.00001f)) {
				result.happened = true;
				result.obj = this;
				result.normal = -ray.Direction;
				result.material = Material;
				result.distance = dir.LengthSquare();
				result.coords = Position;
			}
			return result;
		}

		public override Vector2f UV2XY(Vector2f uv) {
			return new Vector2f();
		}

		public override Vector3f SelectALightPoint(Vector3f rayFrom) {
			return Position;
		}
	}
}
