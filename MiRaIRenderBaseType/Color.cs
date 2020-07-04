//using System.Numerics;

//using Vector3f = System.Numerics.Vector3;
using Float = System.Single;

namespace MiRaIRenderBaseType {
	/// <summary>
	/// 颜色
	/// </summary>
	public struct Color {
		public Float R, G, B;
		public Color(Float v) {
			R = v;
			G = v;
			B = v;
		}
		public Color (Float r, Float g, Float b) {
			R = r;
			G = g;
			B = b;
		}


		public static Color operator + (Color l, Color r) {
			return Add(l, r);
		}
		public static Color operator -(Color l, Color r) {
			return Sub(l, r);
		}
		public static Color operator *(Color l, Float r) {
			return Mul(l, r);
		}
		public static Color operator *(Float l, Color r) {
			return Mul(r, l);
		}
		public static Color operator *(Color l, Color r) {
			return Mul(l, r);
		}
		public static Color operator /(Color l, Float r) {
			return Div(l, r);
		}

		public static Color Add (Color left, Color right) {
			return new Color(left.R + right.R, left.G + right.G, left.B + right.B);
		}
		public static Color Sub(Color left, Color right) {
			return new Color(left.R - right.R, left.G - right.G, left.B - right.B);
		}
		public static Color Mul(Color left, Float right) {
			return new Color(left.R * right, left.G * right, left.B * right);
		}
		public static Color Mul(Color left, Color right) {
			return new Color(left.R * right.R, left.G * right.G, left.B * right.B);
		}
		public static Color Div(Color left, Float right) {
			return new Color(left.R / right, left.G / right, left.B / right);
		}

		public static readonly Color Dark = new Color(0.0f, 0.0f, 0.0f);
	}
}
