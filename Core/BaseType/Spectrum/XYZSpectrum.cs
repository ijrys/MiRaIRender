using Float = System.Single;

/// <summary>
/// 光波相关
/// </summary>
namespace MiRaIRender.BaseType.Spectrum {


	public struct XYZSpectrum {
		public Float X, Y, Z;

		public XYZSpectrum(Float v) {
			X = v;
			Y = v;
			Z = v;
		}

		public XYZSpectrum(Float x, Float y, Float z) {
			X = x;
			Y = y;
			Z = z;
		}

		public override string ToString() {
			return $"Color:{{X:{X:0.000}, Y:{Y:0.000}, Z:{Z:0.000} }}";
		}

		#region Operators
		public static XYZSpectrum operator +(XYZSpectrum l, XYZSpectrum r) {
			return Add(l, r);
		}
		public static XYZSpectrum operator -(XYZSpectrum l, XYZSpectrum r) {
			return Sub(l, r);
		}
		public static XYZSpectrum operator *(XYZSpectrum l, Float r) {
			return Mul(l, r);
		}
		public static XYZSpectrum operator *(Float l, XYZSpectrum r) {
			return Mul(r, l);
		}
		public static XYZSpectrum operator *(XYZSpectrum l, XYZSpectrum r) {
			return Mul(l, r);
		}
		public static XYZSpectrum operator /(XYZSpectrum l, Float r) {
			return Div(l, r);
		}
		public static explicit operator XYZSpectrum(RGBSpectrum rgb) {
			return SpectrumTools.RGBToXYZ(rgb);
		}
		#endregion

		#region Static Functions
		public static XYZSpectrum Add(XYZSpectrum left, XYZSpectrum right) {
			return new XYZSpectrum(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		public static XYZSpectrum Sub(XYZSpectrum left, XYZSpectrum right) {
			return new XYZSpectrum(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}
		public static XYZSpectrum Mul(XYZSpectrum left, Float right) {
			return new XYZSpectrum(left.X * right, left.Y * right, left.Z * right);
		}
		public static XYZSpectrum Mul(XYZSpectrum left, XYZSpectrum right) {
			return new XYZSpectrum(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}
		public static XYZSpectrum Div(XYZSpectrum left, Float right) {
			return new XYZSpectrum(left.X / right, left.Y / right, left.Z / right);
		}
		public static XYZSpectrum Lerp(XYZSpectrum l, XYZSpectrum r, Float lPower) {
			return l * lPower + r * (1.0f - lPower);
		}
		public static XYZSpectrum FromRGB(RGBSpectrum rgb) {
			return SpectrumTools.RGBToXYZ(rgb);
		}

		#endregion
	}
}
