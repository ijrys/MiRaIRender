//using System.Numerics;

//using Vector3f = System.Numerics.Vector3;

namespace MiRaIRenderBaseType.SceneObject {
	public interface IRenderObjectAble {
		//Vector3f Position { get; set; }
		//Vector3f Rotation { get; set; }
		Bounds3 BoundBox { get; }

		Vector2f UV2XY(Vector2f uv);
	}
}
