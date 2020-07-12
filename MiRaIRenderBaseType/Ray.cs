using MiRaIRender.BaseType.SceneObject;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 光线
	/// </summary>
	public struct Ray {
		/// <summary>
		/// 源点
		/// </summary>
		public Vector3f Origin;
		/// <summary>
		/// 方向
		/// </summary>
		public Vector3f Direction;
		/// <summary>
		/// 方向的倒数，方便计算
		/// </summary>
		public Vector3f Direction_Inv;

		public RenderObject OriginObject;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o">源点</param>
		/// <param name="d">方向，请确保已经单位化</param>
		public Ray(Vector3f o, Vector3f d) {
			Origin = o;
			Direction = d;
			Direction_Inv = Vector3f.One / Direction; // new Vector3f(1.0f / d.x, 1.0f / d.y, 1.0f / d.z);
			OriginObject = null;
		}

		public Ray(Vector3f o, Vector3f d, RenderObject originObject) {
			Origin = o;
			Direction = d;
			Direction_Inv = Vector3f.One / d; //new Vector3f(1.0f / d.x, 1.0f / d.y, 1.0f / d.z);
			OriginObject = originObject;
		}
	}
}
