using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	/// <summary>
	/// 纯色贴图
	/// </summary>
	public class PureColorMaterialMap : IMaterialMapAble {
		public Color BaseColor { get; set; }
		public Color Color(Vector2f position) {
			return BaseColor;
		}
		public PureColorMaterialMap() {
			BaseColor = new Color(0.8f);
		}
		public PureColorMaterialMap(Color color) {
			BaseColor = color;
		}
	}

	public class PureGrayMaterialMap : IMaterialGrayMapAble {
		public Float BaseGray { get; set; } = 0.0f;
		
		public Color Color(Vector2f position) {
			return new Color(BaseGray);
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
		public PureGrayMaterialMap(Color color) {
			BaseGray = (color.R + color.G + color.B) / 3.0f;
		}
	}
}
