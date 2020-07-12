using MiRaIRender.BaseType.SceneObject;
using MiRaIRender.BaseType.Skybox;
using System.Collections.Generic;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 场景
	/// </summary>
	public class Scene {
		public SceneObjectA[] r_Objects;
		public SceneObjectA[] r_LightObjects;

		public List<SceneObjectA> Objects = new List<SceneObjectA>();

		public ISkyBoxAble SkyBox = DefaultSkyBox.Default;

		public RayCastResult Intersection(Ray ray) {
			RayCastResult result = null;
			foreach (SceneObjectA o in r_Objects) {
				RayCastResult re = o.Intersection(ray);
				result = RayCastResult.BetterOne(result, re);
			}
			return result;
		}

		public void PreRender() {
			r_Objects = Objects.ToArray();
			List<SceneObjectA> LightObjects = new List<SceneObjectA>();
			foreach (SceneObject.SceneObjectA obj in r_Objects) {
				if (obj.Material.Light.Enable) {
					LightObjects.Add(obj);
				}
			}
			r_LightObjects = LightObjects.ToArray();
		}
	}
}
