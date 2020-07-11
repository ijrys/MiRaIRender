using Float = System.Single;

namespace MiRaIRender.BaseType.SceneObject {
	public abstract class RenderObject : IRayCastAble { //: IRenderObjectAble {

		protected Bounds3 _boundBox;

		public Bounds3 BoundBox { get => _boundBox; }

		public Vector3f CenterPoint {
			get;
			protected set;
		}

		public abstract RayCastResult Intersection(Ray ray);

		public abstract Vector2f UV2XY(Vector2f uv);

		/// <summary>
		/// 在发光体表面选择一个光追点
		/// </summary>
		/// <param name="rayFrom">光追来源坐标，用于多面发光体选择能照射到来源点的光追点</param>
		/// <returns></returns>
		public abstract Vector3f SelectALightPoint(Vector3f rayFrom);
	}
}
