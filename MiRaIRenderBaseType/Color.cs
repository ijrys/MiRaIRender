using Float = System.Single;

namespace MiRaIRender.BaseType {
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

		public override string ToString() {
			return $"Color:{{R:{R:0.000}, G:{G:0.000}, B:{B:0.000} }}";
		}

		#region Operators
		public static Color operator +(Color l, Color r) {
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
		#endregion

		#region Static Functions
		public static Color Add(Color left, Color right) {
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
		#endregion

		public static readonly Color Dark = new Color(0.0f, 0.0f, 0.0f);
	}
}
