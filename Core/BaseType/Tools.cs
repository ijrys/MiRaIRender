using System;
using Math = System.MathF;
using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.BaseType {
	public static class Tools {
		public const Float EPSILON = 0.000001f;

		public const Float PI = 3.1415926535897931f;

		public static bool FloatClosely(Float a, Float b, Float di) {
			return (b > a - di &&
				b < a + di);
		}

		public static Float GetRadianByAngle(Float angle) {
			return (angle % 360) * PI / 180.0f;
		}

		/// <summary>
		/// 获取反射方向
		/// </summary>
		/// <param name="dir"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static Vector3f Reflect(Vector3f dir, Vector3f n) {
			//return dir - Vector3f.Dot(dir, n) * 2.0f * n;
			return Vector3f.Reflect(dir, n);
		}

		/// <summary>
		/// schlick反射率
		/// </summary>
		/// <param name="f0"></param>
		/// <param name="cosa"></param>
		/// <returns></returns>
		public static Float Schlick(Float f0, Float cosa) {
			cosa = 1.0f - cosa;
			Float c = cosa;
			cosa *= cosa;
			cosa *= cosa;
			cosa *= c;
			return f0 + (1.0f - f0) * cosa;
		}

		public static Vector3f RandomPointInSphere() {
			Random r = new Random();
			double x;
			double y;
			double z;
			do {
				x = r.NextDouble();
				y = r.NextDouble();
				z = r.NextDouble();
			} while (x * x + y * y + z * z > 1.0);

			return new Vector3f((Float)x, (Float)y, (Float)z);
		}

		/// <summary>
		/// 菲涅尔精确反射率
		/// </summary>
		/// <param name="cosAlpha"></param>
		/// <param name="niOverNt"></param>
		/// <returns></returns>
		public static Float FresnelEquation(Float cosAlpha, Float niOverNt) {
			Float sinAlphaSq = 1.0f - cosAlpha * cosAlpha;
			Float sinBetaSq = niOverNt * niOverNt * sinAlphaSq;
			if (sinBetaSq > 1.0f) { return 1.0f; }
			Float cosBeta = Math.Sqrt(1.0f - sinBetaSq);
			Float p1, p2, n = 1.0f / niOverNt;
			p1 = (cosAlpha - n * cosBeta) / (cosAlpha + n * cosBeta);
			p2 = (cosBeta - n * cosAlpha) / (cosBeta + n * cosAlpha);
			return (p1 * p1 + p2 * p2) * 0.5f;
		}

		/// <summary>
		/// 返回折射光方向
		/// </summary>
		/// <param name="dir"></param>
		/// <param name="niOverNt">入射折射率/出射折射率</param>
		/// <returns></returns>
		public static Vector3f Refract(Vector3f dir, Vector3f normal, float niOverNt) {
			dir = -dir;
			Float cosAlpha = Vector3f.Dot(dir, normal);
			Float sinAlphaSq = 1.0f - cosAlpha * cosAlpha;
			Float sinBetaSq = niOverNt * niOverNt * sinAlphaSq;

			Float cosBetaSq = 1.0f - sinBetaSq;
			Float cosBeta = Math.Sqrt(cosBetaSq);
			Vector3f re = ((normal * cosAlpha) - dir) * niOverNt - normal * cosBeta;
			return re;
		}

		public static Float Clamp(Float min, Float max, Float value) {
			if (value < min) { return min; }
			if (value > max) { return max; }
			return value;
		}
		public static int Clamp(int min, int max, int value) {
			if (value < min) { return min; }
			if (value > max) { return max; }
			return value;
		}

		public static Vector3f UVMerge(Float u, Float v, Vector3f n0, Vector3f n1, Vector3f n2) {
			return (1 - u - v) * n0 + u * n1 + v * n2;
		}

		public static Float MinElement(Vector3f v) {
			return Math.Min(Math.Min(v.X, v.Y), v.Z);
		}
		public static Float MaxElement(Vector3f v) {
			return Math.Max(Math.Max(v.X, v.Y), v.Z);
		}
	}
}
