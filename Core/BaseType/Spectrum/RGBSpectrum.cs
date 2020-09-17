using Float = System.Single;

/// <summary>
/// 光波相关
/// </summary>
namespace MiRaIRender.BaseType.Spectrum {
	/// <summary>
	/// RGB颜色
	/// </summary>
	public struct RGBSpectrum : ISpectrum {
		public Float R, G, B;
		public RGBSpectrum(Float v) {
			R = v;
			G = v;
			B = v;
		}
		public RGBSpectrum(Float r, Float g, Float b) {
			R = r;
			G = g;
			B = b;
		}

		public override string ToString() {
			return $"Color:{{R:{R:0.000}, G:{G:0.000}, B:{B:0.000} }}";
		}

		public Float Lightness {
			get {
				return (R + G + B) / 3.0f;
			}
		}

		#region Operators
		public static RGBSpectrum operator +(RGBSpectrum l, RGBSpectrum r) {
			return Add(l, r);
		}
		public static RGBSpectrum operator -(RGBSpectrum l, RGBSpectrum r) {
			return Sub(l, r);
		}
		public static RGBSpectrum operator *(RGBSpectrum l, Float r) {
			return Mul(l, r);
		}
		public static RGBSpectrum operator *(Float l, RGBSpectrum r) {
			return Mul(r, l);
		}
		public static RGBSpectrum operator *(RGBSpectrum l, RGBSpectrum r) {
			return Mul(l, r);
		}
		public static RGBSpectrum operator /(RGBSpectrum l, Float r) {
			return Div(l, r);
		}
		public static explicit operator RGBSpectrum(XYZSpectrum xyz) {
			return SpectrumTools.XYZToRGB(xyz);
		}
		#endregion

		#region Static Functions
		public static RGBSpectrum Add(RGBSpectrum left, RGBSpectrum right) {
			return new RGBSpectrum(left.R + right.R, left.G + right.G, left.B + right.B);
		}
		public static RGBSpectrum Sub(RGBSpectrum left, RGBSpectrum right) {
			return new RGBSpectrum(left.R - right.R, left.G - right.G, left.B - right.B);
		}
		public static RGBSpectrum Mul(RGBSpectrum left, Float right) {
			return new RGBSpectrum(left.R * right, left.G * right, left.B * right);
		}
		public static RGBSpectrum Mul(RGBSpectrum left, RGBSpectrum right) {
			return new RGBSpectrum(left.R * right.R, left.G * right.G, left.B * right.B);
		}
		public static RGBSpectrum Div(RGBSpectrum left, Float right) {
			return new RGBSpectrum(left.R / right, left.G / right, left.B / right);
		}
		public static RGBSpectrum Lerp(RGBSpectrum l, RGBSpectrum r, Float lPower) {
			return l * lPower + r * (1.0f - lPower);
		}
		public static RGBSpectrum FromRGB(XYZSpectrum xyz) {
			return SpectrumTools.XYZToRGB(xyz);
		}

		#endregion

		public static readonly RGBSpectrum Dark = new RGBSpectrum(0.0f, 0.0f, 0.0f);

	}
}
