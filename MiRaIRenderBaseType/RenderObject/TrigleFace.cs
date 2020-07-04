//using System.Numerics;

//using Vector3f = System.Numerics.Vector3;
using MiRaIRenderBaseType.Materials;
using MiRaIRenderBaseType.SceneObject;
using System;
using Float = System.Single;
using Math = System.MathF;

namespace MiRaIRenderBaseType {
	public class TrigleFace : RenderObject {
		/// <summary>
		/// 顶点
		/// </summary>
		public Vector3f v0, v1, v2;
		/// <summary>
		/// 法线
		/// </summary>
		public Vector3f n0, n1, n2;
		/// <summary>
		/// uv坐标轴
		/// </summary>
		public Vector3f e1, e2;
		/// <summary>
		/// 贴图坐标
		/// </summary>
		public Vector2f sp0, sp1, sp2;
		/// <summary>
		/// 材质
		/// </summary>
		public Material material;

		public TrigleFace(Vector3f pointa, Vector3f pointb, Vector3f pointc) {
			v0 = pointa;
			v1 = pointb;
			v2 = pointc;

			e1 = v1 - v0;
			e2 = v2 - v0;

			_boundBox = Bounds3.Union(new Bounds3(v0, v1), v2);
		}

		public override RayCastResult Intersection(Ray ray) {
			RayCastResult result = new RayCastResult();

			Float u, v, t_tmp = 0;
			Vector3f pvec = ray.Direction ^ e2; // S1
			Float det = e1 & pvec;

			if (Math.Abs(det) < Tools.EPSILON) {
				return result;
			}

			Float det_inv = 1.0f / det;
			Vector3f tvec = ray.Origin - v0; // S
			u = (tvec & pvec) * det_inv;
			if (u < 0 || u > 1) {
				return result;
			}
			Vector3f qvec = (tvec ^ e1); // S2
			v = (ray.Direction & qvec) * det_inv;
			if (v < 0 || u + v > 1) {
				return result;
			}
			t_tmp = (e2 & qvec) * det_inv;
			if (t_tmp < 0) {
				return result;
			}

			result.happened = true;
			result.distance = t_tmp;
			result.obj = this;
			result.material = material;
			result.coords = ray.Origin + t_tmp * ray.Direction;
			result.uv = new Vector2f(u, v);
			result.normal = Vector3f.UVMerge(u, v, n0, n1, n2);
			return result;
		}

		public override Vector3f SelectALightPoint(Vector3f rayFrom) {
			Random random = new Random();
			double u = random.NextDouble();
			double v = random.NextDouble();
			v *= (1.0 - u);
			return Vector3f.UVMerge((Float)u, (Float)v, v0, v1, v2);
		}

		public override Vector2f UV2XY(Vector2f uv) {
			return (1.0f - uv.x - uv.y) * sp0 + uv.x * sp1 + uv.y * sp2;
		}
	}
}
