using Float = System.Single;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.BaseType.SceneObject {
	public abstract class RenderObject : IRayCastAble { //: IRenderObjectAble {
		/// <summary>
		/// 包围盒
		/// </summary>
		public Bounds3 BoundBox { get; protected set; }

		/// <summary>
		/// 材质
		/// </summary>
		public Materials.Material Material { get; set; } = new Materials.Material();

		/// <summary>
		/// 中心点
		/// </summary>
		public Vector3f CenterPoint { get; protected set; }

		/// <summary>
		/// 光线相交运算
		/// </summary>
		/// <param name="ray"></param>
		/// <param name="nowbest"></param>
		/// <returns></returns>
		public abstract RayCastResult Intersection(Ray ray, float nowbest);

		public abstract Vector2f UV2XY(Vector2f uv);

		/// <summary>
		/// 在发光体表面选择一个光追点
		/// </summary>
		/// <param name="rayFrom">光追来源坐标，用于多面发光体选择能照射到来源点的光追点</param>
		/// <returns></returns>
		public abstract Vector3f SelectALightPoint(Vector3f rayFrom);
	}
}
