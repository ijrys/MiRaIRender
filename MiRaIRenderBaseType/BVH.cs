using MiRaIRender.BaseType.SceneObject;
using System;
using System.Collections.Generic;

using System.Text;

namespace MiRaIRender.BaseType {
	public class BVH {
		public Bounds3 BoundsBox;

		public BVH Left, Right;
		RenderObject[] Childs;

		public RayCastResult Intersection(Ray ray) {
			if (!BoundsBox.Intersection(ray)) { // 未相交
				return null;
			}
			RayCastResult lres = null;
			RayCastResult rres = null;
			if (Left != null) {
				lres = Left.Intersection(ray);
			}
			if (Right != null) {
				rres = Right.Intersection(ray);
			}
			RayCastResult result =  RayCastResult.BetterOne(lres, rres);

			if (Childs != null) {
				foreach (RenderObject obj in Childs) {
					RayCastResult restmp = obj.Intersection(ray);
					result = RayCastResult.BetterOne(result, restmp);
				}
			}

			return result;
		}


		private static int QuickCoordationX<T>(Span<T> objs) where T : RenderObject {
			int len = objs.Length;
			int aimid = len / 2;
			int s = 0, e = len, f = 1, t = len - 1;
			while (s < e - 1) {
				f = s + 1;
				t = e - 1;
				while (f < t) {
					// 前找大
					while (f < t) {
						if (objs[f].CenterPoint.x > objs[s].CenterPoint.x) {
							break;
						}
						f++;
					}
					// 后找小
					while (f < t) {
						if (objs[t].CenterPoint.x < objs[s].CenterPoint.x) {
							break;
						}
						t--;
					}
					// 交换
					if (f != t) {
						var tmp = objs[f];
						objs[f] = objs[t];
						objs[t] = tmp;
					}
				}
				if (objs[f].CenterPoint.x > objs[s].CenterPoint.x) {
					f--;
				}
				if (f != s) {
					var tmp = objs[f];
					objs[f] = objs[s];
					objs[s] = tmp;
				}
				if (f == aimid) {
					return aimid;
				}
				if (f < aimid) {
					s = f + 1;
				}
				else {
					e = f;
				}
			}
			return aimid;
		}
		private static int QuickCoordationY<T>(Span<T> objs) where T : RenderObject {
			int len = objs.Length;
			int aimid = len / 2;
			int s = 0, e = len, f = 1, t = len - 1;
			while (s < e - 1) {
				f = s + 1;
				t = e - 1;
				while (f < t) {
					// 前找大
					while (f < t) {
						if (objs[f].CenterPoint.y > objs[s].CenterPoint.y) {
							break;
						}
						f++;
					}
					// 后找小
					while (f < t) {
						if (objs[t].CenterPoint.y < objs[s].CenterPoint.y) {
							break;
						}
						t--;
					}
					// 交换
					if (f != t) {
						var tmp = objs[f];
						objs[f] = objs[t];
						objs[t] = tmp;
					}
				}
				if (objs[f].CenterPoint.y > objs[s].CenterPoint.y) {
					f--;
				}
				if (f != s) {
					var tmp = objs[f];
					objs[f] = objs[s];
					objs[s] = tmp;
				}
				if (f == aimid) {
					return aimid;
				}
				if (f < aimid) {
					s = f + 1;
				}
				else {
					e = f;
				}
			}
			return aimid;
		}
		private static int QuickCoordationZ<T>(Span<T> objs) where T : RenderObject {
			int len = objs.Length;
			int aimid = len / 2;
			int s = 0, e = len, f = 1, t = len - 1;
			while (s < e - 1) {
				f = s + 1;
				t = e - 1;
				while (f < t) {
					// 前找大
					while (f < t) {
						if (objs[f].CenterPoint.z > objs[s].CenterPoint.z) {
							break;
						}
						f++;
					}
					// 后找小
					while (f < t) {
						if (objs[t].CenterPoint.z < objs[s].CenterPoint.z) {
							break;
						}
						t--;
					}
					// 交换
					if (f != t) {
						var tmp = objs[f];
						objs[f] = objs[t];
						objs[t] = tmp;
					}
				}
				if (objs[f].CenterPoint.z > objs[s].CenterPoint.z) {
					f--;
				}
				if (f != s) {
					var tmp = objs[f];
					objs[f] = objs[s];
					objs[s] = tmp;
				}
				if (f == aimid) {
					return aimid;
				}
				if (f < aimid) {
					s = f + 1;
				}
				else {
					e = f;
				}
			}
			return aimid;
		}

		public static BVH Build<T>(Span<T> objs) where T:RenderObject {
			Bounds3 boundbox = objs[0].BoundBox;
			foreach (var item in objs) {
				boundbox = Bounds3.Union(boundbox, item.BoundBox);
			}
			BVH re = new BVH() { BoundsBox = boundbox };
			if (objs.Length < 5) {
				re.Childs = objs.ToArray();
				return re;
			}

			int divideWay = 0;
			{
				Vector3f v = boundbox.pMax - boundbox.pMin;
				if (v.x < v.y) {
					if (v.y < v.z) {
						divideWay = 2;
					}
					else {
						divideWay = 1;
					}
				}
				else {
					if (v.x < v.z) {
						divideWay = 2;
					}
					else {
						divideWay = 0;
					}
				}
			}
			int dividePoint = -1;
			if (divideWay == 0) {
				dividePoint = QuickCoordationX(objs);
			}
			else if (divideWay == 1) {
				dividePoint = QuickCoordationY(objs);
			}
			else {
				dividePoint = QuickCoordationZ(objs);
			}
			re.Left = Build(objs.Slice(0, dividePoint));
			re.Right = Build(objs.Slice(dividePoint));

			return re;
		}
	}
}
