/// <summary>
/// 材质相关类型
/// </summary>
namespace MiRaIRender.BaseType.Materials {
	public class NormalProperty {
		private bool _enable = false;

		public bool Enable {
			get => _enable;
			set => _enable = value;
		}

		public IMaterialMapAble NormalMap;

		public Vector3f GetNormal(Vector2f xy) {
			if (!Enable || NormalMap == null) {
				return Vector3f.Zero;
			}
			Color color = NormalMap.Color(xy);
			return new Vector3f(color.R - 0.5f, color.B - 0.5f, color.G - 0.5f).Normalize();
		}
	}
}
