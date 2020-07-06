using MiRaIRender.BaseType.Materials;
using MiRaIRender.BaseType.SceneObject;
using Float = System.Single;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 路径追踪测试结果
	/// </summary>
	public struct RayCastResult {
		/// <summary>
		/// 追踪发生
		/// </summary>
		public bool happened;

		/// <summary>
		/// 距离
		/// </summary>
		public Float distance;

		/// <summary>
		/// 追踪点法线
		/// </summary>
		public Vector3f normal;

		/// <summary>
		/// 追踪物体
		/// </summary>
		public RenderObject obj;

		/// <summary>
		/// 追踪点材质
		/// </summary>
		public Material material;

		/// <summary>
		/// 追踪点的uv坐标
		/// </summary>
		public Vector2f uv;

		/// <summary>
		/// 追踪点坐标
		/// </summary>
		public Vector3f coords;
	}
}
