//using System.Numerics;

//using Vector3f = System.Numerics.Vector3;
using MiRaIRenderBaseType.SceneObject;
using MiRaIRenderBaseType.Skybox;

namespace MiRaIRenderBaseType {
	/// <summary>
	/// 场景
	/// </summary>
	public class Scene {

		public RenderObject[] Objects;

		public RenderObject[] LightObjects;

		public ISkyBoxAble SkyBox = DefaultSkyBox.Default;

		public RayCastResult Intersection(Ray ray) {
			RayCastResult result = new RayCastResult();
			foreach (RenderObject o in Objects) {
				RayCastResult re = o.Intersection(ray);
				if (re.happened && (!result.happened || re.distance < result.distance)) {
					result = re;
				}
			}
			return result;
		}
	}
}
