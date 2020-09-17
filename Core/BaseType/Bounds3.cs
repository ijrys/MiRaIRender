using Math = System.MathF;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 三维包围盒
	/// </summary>
	public class Bounds3 {
		public static float marginValue = 0.001f;
		public static Vector3f Margin = new Vector3f(marginValue);
		public Vector3f pMin, pMax;
		public Bounds3() { }

		public Bounds3(Vector3f p) {
			pMin = p - Margin;
			pMax = p + Margin;
		}
		public Bounds3(Vector3f p1, Vector3f p2) {
			pMin = new Vector3f(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Min(p1.Z, p2.Z));
			pMax = new Vector3f(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y), Math.Max(p1.Z, p2.Z));

			ExpandMargin();
		}
		public Bounds3(Vector3f p1, Vector3f p2, Vector3f p3) {
			pMin = Vector3f.Min(Vector3f.Min(p1, p2), p3);
			pMax = Vector3f.Max(Vector3f.Max(p1, p2), p3);

			ExpandMargin();
		}

		private void ExpandMargin() {
			Vector3f diff = pMax - pMin;
			float m = marginValue * 2;
			if (diff.X < m) {
				pMin.X -= marginValue; pMax.X += marginValue;
			}
			if (diff.Y < m) {
				pMin.Y -= marginValue; pMax.Y += marginValue;
			}
			if (diff.Z < m) {
				pMin.Z -= marginValue; pMax.Z += marginValue;
			}
		}

		/// <summary>
		/// 一点相对pMin的偏移比例
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public Vector3f Offset(Vector3f p) {
			Vector3f o = p - pMin;
			//if (pMax.X > pMin.X)
			//	o.X /= pMax.X - pMin.X;
			//if (pMax.Y > pMin.Y)
			//	o.Y /= pMax.Y - pMin.Y;
			//if (pMax.Z > pMin.Z)
			//	o.Z /= pMax.Z - pMin.Z;
			//return o;
			Vector3f b = pMax - pMin;
			return o / b;
		}

		/// <summary>
		/// 判断光线与包围盒是否相交
		/// </summary>
		/// <param name="ray"></param>
		/// <returns>是否相交，相交点到ray.d距离</returns>
		public (bool, float) Intersection(Ray ray) {
			Vector3f tmins = (pMin - ray.Origin) * ray.Direction_Inv;
			Vector3f tmaxs = (pMax - ray.Origin) * ray.Direction_Inv;

			float tmin = Tools.MaxElement(Vector3f.Min(tmins, tmaxs));
			float tmax = Tools.MinElement(Vector3f.Max(tmins, tmaxs));

			return (tmin < tmax, tmin);
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
				new Vector3f(Math.Max(a.pMin.X, b.pMin.X), Math.Max(a.pMin.Y, b.pMin.Y), Math.Max(a.pMin.Z, b.pMin.Z)),
				new Vector3f(Math.Min(a.pMax.X, b.pMax.X), Math.Min(a.pMax.Y, b.pMax.Y), Math.Min(a.pMax.Z, b.pMax.Z))
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

		///// <summary>
		///// 并运算
		///// </summary>
		///// <param name="a"></param>
		///// <param name="b"></param>
		///// <returns></returns>
		//public static Bounds3 Union(Vector3f a, Vector3f b, Vector3f c) {
		//	return new Bounds3(a, b, c);
		//}
		#endregion
	}
}
