using MiRaIRender.BaseType.Materials;
using MiRaIRender.BaseType.SceneObject;
using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 路径追踪测试结果
	/// </summary>
	public class RayCastResult {

		/// <summary>
		/// 相交点是否在物体内部
		/// </summary>
		public bool internalPoint = false;

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

		/// <summary>
		/// 出射物体ior
		/// </summary>
		public Float IOR;

		/// <summary>
		/// 返回两个结果中更好的一个
		/// </summary>
		/// <param name="res1"></param>
		/// <param name="res2"></param>
		/// <returns></returns>
		public static RayCastResult BetterOne(RayCastResult res1, RayCastResult res2) {
			if (res1 == null) {
				return res2;
			}
			if (res2 == null) {
				return res1;
			}
			if (res1.distance < res2.distance) {
				return res1;
			}
			else {
				return res2;
			}
		}
	}
}
