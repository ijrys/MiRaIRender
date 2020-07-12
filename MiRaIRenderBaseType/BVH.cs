using MiRaIRender.BaseType.SceneObject;
using System;
using System.Collections.Generic;

using System.Text;
using Vector3f = System.Numerics.Vector3;
using Vector2f = System.Numerics.Vector2;

namespace MiRaIRender.BaseType {
	public class BVH : IRayCastAble {
		//public BVH Left, Right;
		IRayCastAble[] Childs;
		public Bounds3 BoundBox { get; set; }


		public RayCastResult Intersection(Ray ray) {
			if (!BoundBox.Intersection(ray)) { // 未相交
				return null;
			}
			//RayCastResult lres = null;
			//RayCastResult rres = null;
			//if (Left != null) {
			//	lres = Left.Intersection(ray);
			//}
			//if (Right != null) {
			//	rres = Right.Intersection(ray);
			//}
			RayCastResult result = null; // RayCastResult.BetterOne(lres, rres);

			if (Childs != null) {
				foreach (IRayCastAble obj in Childs) {
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
						if (objs[f].CenterPoint.X > objs[s].CenterPoint.X) {
							break;
						}
						f++;
					}
					// 后找小
					while (f < t) {
						if (objs[t].CenterPoint.X < objs[s].CenterPoint.X) {
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
				if (objs[f].CenterPoint.X > objs[s].CenterPoint.X) {
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
						if (objs[f].CenterPoint.Y > objs[s].CenterPoint.Y) {
							break;
						}
						f++;
					}
					// 后找小
					while (f < t) {
						if (objs[t].CenterPoint.Y < objs[s].CenterPoint.Y) {
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
				if (objs[f].CenterPoint.Y > objs[s].CenterPoint.Y) {
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
						if (objs[f].CenterPoint.Z > objs[s].CenterPoint.Z) {
							break;
						}
						f++;
					}
					// 后找小
					while (f < t) {
						if (objs[t].CenterPoint.Z < objs[s].CenterPoint.Z) {
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
				if (objs[f].CenterPoint.Z > objs[s].CenterPoint.Z) {
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

		public static BVH Build<T>(Span<T> objs) where T : RenderObject {
			Bounds3 boundbox = objs[0].BoundBox;
			foreach (var item in objs) {
				boundbox = Bounds3.Union(boundbox, item.BoundBox);
			}

			BVH re = new BVH() { BoundBox = boundbox };
			if (objs.Length < 5) {
				re.Childs = objs.ToArray();
				goto RTPoint;
				//return re;
			}

			int divideWay = 0;
			{
				Vector3f v = boundbox.pMax - boundbox.pMin;
				if (v.X < v.Y) {
					if (v.Y < v.Z) {
						divideWay = 2;
					}
					else {
						divideWay = 1;
					}
				}
				else {
					if (v.X < v.Z) {
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

			BVH l = Build(objs.Slice(0, dividePoint));
			BVH r = Build(objs.Slice(dividePoint));
			re.Childs = new IRayCastAble[] { l, r };

		RTPoint:
			return re;
		}
	}
}
