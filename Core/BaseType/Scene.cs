using MiRaIRender.BaseType.SceneObject;
using MiRaIRender.BaseType.Skybox;
using System.Collections.Generic;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 场景
	/// </summary>
	public class Scene {
		public Scene (ISkyBoxAble skyBox) {
			SkyBox = skyBox;
		}

		public RenderObject[] r_Objects;
		public RenderObject[] r_LightObjects;

		public List<RenderObject> Objects = new List<RenderObject>();

		public ISkyBoxAble SkyBox;// = DefaultSkyBox.Default; // todo: 删除默认值

		public RayCastResult Intersection(Ray ray) {
			RayCastResult result = null;
			foreach (RenderObject o in r_Objects) {
				RayCastResult re = o.Intersection(ray, float.MaxValue);
				result = RayCastResult.BetterOne(result, re);
			}
			return result;
		}

		public void PreRender() {
			r_Objects = Objects.ToArray();
			List<RenderObject> LightObjects = new List<RenderObject>();
			foreach (SceneObject.RenderObject obj in r_Objects) {
				if (obj.Material.Light.Enable) {
					LightObjects.Add(obj);
				}
			}
			r_LightObjects = LightObjects.ToArray();
		}
	}
}
