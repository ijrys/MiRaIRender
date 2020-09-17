using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;
using MiRaIRender.BaseType.Spectrum;
/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	/// <summary>
	/// XYZ颜色纯色贴图
	/// </summary>
	public class PureXYZColorMaterialMap : IMaterialMapAble<XYZSpectrum> {
		public XYZSpectrum BaseColor { get; set; }
		private Float lightness;
		public XYZSpectrum Color(Vector2f position) {
			return BaseColor;
		}
		public float Lightness(Vector2f position) {
			return BaseColor.Lightness;
		}

		public PureXYZColorMaterialMap() {
			BaseColor = new XYZSpectrum(0.8f);
		}

		public PureXYZColorMaterialMap(XYZSpectrum color) {
			BaseColor = color;
		}
	}

	/// <summary>
	/// RGB颜色纯色贴图
	/// </summary>
	public class PureRGBColorMaterialMap : IMaterialMapAble<RGBSpectrum> {
		public RGBSpectrum BaseColor { get; set; }
		private Float lightness;
		public RGBSpectrum Color(Vector2f position) {
			return BaseColor;
		}
		public float Lightness(Vector2f position) {
			return BaseColor.Lightness;
		}

		public PureRGBColorMaterialMap() {
			BaseColor = new RGBSpectrum(0.8f);
		}

		public PureRGBColorMaterialMap(RGBSpectrum color) {
			BaseColor = color;
		}
	}
}
