/// <summary>
/// 光波相关
/// </summary>
namespace MiRaIRender.BaseType.Spectrum {
	public static class SpectrumTools {
		public static RGBSpectrum ToRGB(this XYZSpectrum xyz) {
			return XYZToRGB(xyz);
		}
		public static XYZSpectrum ToXYZ(this RGBSpectrum rgb) {
			return RGBToXYZ(rgb);
		}

		public static RGBSpectrum XYZToRGB(XYZSpectrum xyz) {
			return new RGBSpectrum(
				3.240479f * xyz.X - 1.537150f * xyz.Y - 0.498535f * xyz.Z,
				-0.969256f * xyz.X + 1.875991f * xyz.Y + 0.041556f * xyz.Z,
				0.055648f * xyz.X - 0.204043f * xyz.Y + 1.057311f * xyz.Z);
		}
		public static XYZSpectrum RGBToXYZ(RGBSpectrum rgb) {
			return new XYZSpectrum(
				0.412453f * rgb.R + 0.357580f * rgb.G + 0.180423f * rgb.B,
				0.212671f * rgb.R + 0.715160f * rgb.G + 0.072169f * rgb.B,
				0.019334f * rgb.R + 0.119193f * rgb.G + 0.950227f * rgb.B);
		}
	}
}
