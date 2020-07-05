using Float = System.Single;

/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	public interface IMaterialMapAble {
		public Color Color(Vector2f position);
	}

	/// <summary>
	/// 灰度贴图
	/// </summary>
	public interface IMaterialGrayMapAble : IMaterialMapAble {
		public Float Lightness(Vector2f position);
		Color IMaterialMapAble.Color(Vector2f position) {
			Float l = Lightness(position);
			return new Color(l);
		}
	}
}
