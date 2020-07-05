using Math = System.MathF;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 三维包围盒
	/// </summary>
	public class Bounds3 {
		public Vector3f pMin, pMax;
		public Bounds3() { }

		public Bounds3(Vector3f p) {
			pMin = pMax = p;
		}
		public Bounds3(Vector3f p1, Vector3f p2) {
			pMin = new Vector3f(Math.Min(p1.x, p2.x), Math.Min(p1.y, p2.y), Math.Min(p1.z, p2.z));
			pMax = new Vector3f(Math.Max(p1.x, p2.x), Math.Max(p1.y, p2.y), Math.Max(p1.z, p2.z));
		}

		/// <summary>
		/// 一点相对pMin的偏移比例
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Vector3f Offset(Vector3f p) {
			Vector3f o = p - pMin;
			if (pMax.x > pMin.x)
				o.x /= pMax.x - pMin.x;
			if (pMax.y > pMin.y)
				o.y /= pMax.y - pMin.y;
			if (pMax.z > pMin.z)
				o.z /= pMax.z - pMin.z;
			return o;
		}

		/// <summary>
		/// 判断光线与包围盒是否相交
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public bool Intersection(Ray ray) {
			Vector3f tmins = (pMin - ray.Origin) / ray.Direction;
			Vector3f tmaxs = (pMax - ray.Origin) / ray.Direction;

			float tmin = Vector3f.Min(tmins, tmaxs).MaxValue();
			float tmax = Vector3f.Max(tmins, tmaxs).MinValue();

			return (tmin < tmax);
		}

		#region Boolean Operaters
		/// <summary>
		/// 交运算
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Bounds3 Intersect(Bounds3 a, Bounds3 b) {
			return new Bounds3(
				new Vector3f(Math.Max(a.pMin.x, b.pMin.x), Math.Max(a.pMin.y, b.pMin.y), Math.Max(a.pMin.z, b.pMin.z)),
				new Vector3f(Math.Min(a.pMax.x, b.pMax.x), Math.Min(a.pMax.y, b.pMax.y), Math.Min(a.pMax.z, b.pMax.z))
			);
		}
		/// <summary>
		/// 并运算
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Bounds3 Union(Bounds3 a, Bounds3 b) {
			Bounds3 ret = new Bounds3();
			ret.pMin = Vector3f.Min(a.pMin, b.pMin);
			ret.pMax = Vector3f.Max(a.pMax, b.pMax);
			return ret;
		}
		/// <summary>
		/// 并运算
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Bounds3 Union(Bounds3 a, Vector3f b) {
			Bounds3 ret = new Bounds3();
			ret.pMin = Vector3f.Min(a.pMin, b);
			ret.pMax = Vector3f.Max(a.pMax, b);
			return ret;
		}
		#endregion


	}
}
