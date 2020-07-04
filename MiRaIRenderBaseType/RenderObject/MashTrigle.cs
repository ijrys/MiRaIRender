//using System.Numerics;

//using Vector3f = System.Numerics.Vector3;
using MiRaIRenderBaseType.SceneObject;
using System;

namespace MiRaIRenderBaseType {
	/// <summary>
	/// 三角形网格对象
	/// </summary>
	public class MashTrigle : RenderObject {
		TrigleFace[] trigles;

		/// <summary>
		/// 光线追踪测试
		/// </summary>
		/// <param name="ray"></param>
		/// <returns></returns>
		public override RayCastResult Intersection(Ray ray) {
			// 包围盒测试失败
			if (!_boundBox.Intersection(ray)) {
				return new RayCastResult();
			}

			RayCastResult result = new RayCastResult();
			foreach (TrigleFace trigle in trigles) {
				RayCastResult resTemp = trigle.Intersection(ray);
				if (resTemp.happened && (!result.happened || result.distance > resTemp.distance)) {
					result = resTemp;
				}
			}
			return result;
		}

		public override Vector2f UV2XY(Vector2f uv) {
			return new Vector2f();
		}
		public override Vector3f SelectALightPoint(Vector3f rayFrom) {
			throw new NotImplementedException();
		}
	}
}
