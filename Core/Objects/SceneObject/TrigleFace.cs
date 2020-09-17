using MiRaIRender.BaseType;
using MiRaIRender.BaseType.SceneObject;
using System;
using Float = System.Single;
using Vector2f = System.Numerics.Vector2;
using Vector3f = System.Numerics.Vector3;

namespace MiRaIRender.Objects.SceneObject {
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

		public TrigleFace(Vector3f pointa, Vector3f pointb, Vector3f pointc) {
			v0 = pointa;
			v1 = pointb;
			v2 = pointc;

			e1 = v1 - v0;
			e2 = v2 - v0;

			BoundBox = new Bounds3(v0, v1, v2);
			CenterPoint = (v0 + v1 + v2) / 3.0f;
		}

		public override RayCastResult Intersection(Ray ray, float nowbest) {
			if (ray.OriginObject == this) { return null; }
			{
				(bool happened, Float mint) = BoundBox.Intersection(ray);
				if (!happened || mint > nowbest) { // 未相交 或 当前最小解已不是最优
					return null;
				}
			}
			Float u, v, t_tmp = 0;
			Vector3f pvec = Vector3f.Cross(ray.Direction, e2); // S1
			Float det = Vector3f.Dot(e1, pvec);

			Float det_inv = 1.0f / det;
			Vector3f tvec = ray.Origin - v0; // S
			u = Vector3f.Dot(tvec, pvec) * det_inv;
			if (u < 0 || u > 1) {
				return null;
			}
			Vector3f qvec = Vector3f.Cross(tvec, e1); // S2
			v = Vector3f.Dot(ray.Direction, qvec) * det_inv;
			if (v < 0 || u + v > 1) {
				return null;
			}
			t_tmp = Vector3f.Dot(e2, qvec) * det_inv;
			if (t_tmp < 0) {
				return null;
			}

			RayCastResult result = new RayCastResult();
			
			result.distance = t_tmp;
			result.obj = this;
			result.coords = ray.Origin + t_tmp * ray.Direction;
			result.uv = new Vector2f(u, v);
			result.normal = Tools.UVMerge(u, v, n0, n1, n2);
			result.internalPoint = (Vector3f.Dot(result.normal, ray.Direction) > 0);
			if (result.internalPoint) {
				result.normal = -result.normal;
			}
			return result;

		}

		public override Vector3f SelectALightPoint(Vector3f rayFrom) {
			Random random = new Random();
			double u = random.NextDouble();
			double v = random.NextDouble();
			v *= (1.0 - u);
			return Tools.UVMerge((Float)u, (Float)v, v0, v1, v2);
		}

		public override Vector2f UV2XY(Vector2f uv) {
			return (1.0f - uv.X - uv.Y) * sp0 + uv.X * sp1 + uv.Y * sp2;
		}
	}
}
