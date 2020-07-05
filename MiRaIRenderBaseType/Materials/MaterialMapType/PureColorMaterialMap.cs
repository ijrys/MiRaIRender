using Float = System.Single;

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
	}

	public class PureGrayMaterialMap : IMaterialGrayMapAble {
		public Float BaseGray { get; set; } = 0.0f;
		
		public Color Color(Vector2f position) {
			return new Color(BaseGray);
		}

		public float Lightness(Vector2f position) {
			return BaseGray;
		}
	}
}
