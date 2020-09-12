using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;
using MiRaIRender.BaseType.Spectrum;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	/// <summary>
	/// 纯色贴图
	/// </summary>
	public class PureColorMaterialMap : IMaterialMapAble {
		public RGBSpectrum BaseColor { get; set; }
		public RGBSpectrum Color(Vector2f position) {
			return BaseColor;
		}
		public PureColorMaterialMap() {
			BaseColor = new RGBSpectrum(0.8f);
		}
		public PureColorMaterialMap(RGBSpectrum color) {
			BaseColor = color;
		}
	}

	public class PureGrayMaterialMap : IMaterialGrayMapAble {
		public Float BaseGray { get; set; } = 0.0f;
		
		public RGBSpectrum Color(Vector2f position) {
			return new RGBSpectrum(BaseGray);
		}

		public float Lightness(Vector2f position) {
			return BaseGray;
		}

		public PureGrayMaterialMap() {
			BaseGray = 0.8f;
		}
		public PureGrayMaterialMap(Float lightness) {
			BaseGray = lightness;
		}
		public PureGrayMaterialMap(RGBSpectrum color) {
			BaseGray = (color.R + color.G + color.B) / 3.0f;
		}
	}
}
