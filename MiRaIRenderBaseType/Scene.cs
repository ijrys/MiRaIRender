using MiRaIRender.BaseType.SceneObject;
using MiRaIRender.BaseType.Skybox;

namespace MiRaIRender.BaseType {
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
