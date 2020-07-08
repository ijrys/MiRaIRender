using MiRaIRender.BaseType.SceneObject;
using MiRaIRender.BaseType.Skybox;
using System.Collections.Generic;

namespace MiRaIRender.BaseType {
	/// <summary>
	/// 场景
	/// </summary>
	public class Scene {
		public RenderObject[] r_Objects;
		public RenderObject[] r_LightObjects;

		public List<RenderObject> Objects = new List<RenderObject>();

		public List<RenderObject> LightObjects = new List<RenderObject>();

		public ISkyBoxAble SkyBox = DefaultSkyBox.Default;

		public RayCastResult Intersection(Ray ray) {
			RayCastResult result = new RayCastResult();
			foreach (RenderObject o in r_Objects) {
				//if (o == this.r_Objects[1]) {
				//	System.Console.WriteLine("a");
				//}
				RayCastResult re = o.Intersection(ray);
				if (re.happened && (!result.happened || re.distance < result.distance)) {
					result = re;
				}
			}
			return result;
		}

		public void PreRender () {
			r_Objects = Objects.ToArray();
			r_LightObjects = LightObjects.ToArray();
		}
	}
}
