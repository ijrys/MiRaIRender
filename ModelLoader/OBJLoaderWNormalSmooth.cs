using MiRaIRender.BaseType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Float = System.Single;

namespace ModelLoader {
	public class OBJLoaderWNormalSmooth {
		class PW_Trigle {
			public int v0, v1, v2;
			public int vt0, vt1, vt2;
			public Vector3f Normal;

		}

		public static char[] LineSplitChars = new char[] { ' ' };
		public static char[] PInfoSplitChars = new char[] { '/' };

		public static MashTrigle[] LoadModel(string filePath) {
			string[] lines = File.ReadAllLines(filePath);

			List<MashTrigle> mashes = new List<MashTrigle>();
			List<Vector3f> v = new List<Vector3f>() { new Vector3f() };
			Dictionary<int, List<PW_Trigle>> faceList = new Dictionary<int, List<PW_Trigle>>();
			List<Vector2f> vt = new List<Vector2f>() { new Vector2f() };
			List<Vector3f> vn = new List<Vector3f>() { new Vector3f() };

			MashTrigle nobj = null;
			List<PW_Trigle> trigles = new List<PW_Trigle>();

			for (int linecount = 0; linecount < lines.Length; linecount++) {
				string line = lines[linecount];
				if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#')) {
					continue;
				}

				float p0 = 0.0f, p1 = 0.0f, p2 = 0.0f;
				string[] parts = line.Split(LineSplitChars, StringSplitOptions.RemoveEmptyEntries);
				switch (parts[0].ToLower()) {
					case "v":
						#region v
						if (parts.Length != 4) {
							throw new Exception($"line {linecount} : 缺少参数的行，请求3个，实际为{parts.Length - 1}个");
						}
						if (!Float.TryParse(parts[1], out p0)) {
							throw new Exception($"line {linecount} : 不能转换的数据 {parts[1]}");
						}
						if (!Float.TryParse(parts[2], out p1)) {
							throw new Exception($"line {linecount} : 不能转换的数据 {parts[2]}");
						}
						if (!Float.TryParse(parts[3], out p2)) {
							throw new Exception($"line {linecount} : 不能转换的数据 {parts[3]}");
						}
						v.Add(new Vector3f(p0, p1, p2));
						#endregion
						break;
					case "vt":
						#region vt
						if (parts.Length != 3 && parts.Length != 4) {
							throw new Exception($"line {linecount} : 缺少参数的行，请求2或3个，实际为{parts.Length - 1}个");
						}
						if (!Float.TryParse(parts[1], out p0)) {
							throw new Exception($"line {linecount} : 不能转换的数据 {parts[1]}");
						}
						if (!Float.TryParse(parts[2], out p1)) {
							throw new Exception($"line {linecount} : 不能转换的数据 {parts[2]}");
						}
						vt.Add(new Vector2f(p0, p1));
						#endregion
						break;
					case "vn":
						#region vn
						if (parts.Length != 4) {
							throw new Exception($"line {linecount} : 缺少参数的行，请求3个，实际为{parts.Length - 1}个");
						}
						if (!Float.TryParse(parts[1], out p0)) {
							throw new Exception($"line {linecount} : 不能转换的数据 {parts[1]}");
						}
						if (!Float.TryParse(parts[2], out p1)) {
							throw new Exception($"line {linecount} : 不能转换的数据 {parts[2]}");
						}
						if (!Float.TryParse(parts[3], out p2)) {
							throw new Exception($"line {linecount} : 不能转换的数据 {parts[3]}");
						}
						vn.Add(new Vector3f(p0, p1, p2).Normalize());
						#endregion
						break;
					case "vp":

						break;
					case "g":
						#region g
						if (trigles != null && trigles.Count > 0) {
							if (nobj == null) {
								nobj = new MashTrigle();
							}
							nobj.SetTrigles(Smooth(trigles, v, vt, faceList));
							mashes.Add(nobj);
						}
						nobj = new MashTrigle();
						trigles = new List<PW_Trigle>();
						#endregion
						break;

					case "f":
						#region f
						if (parts.Length < 4) {
							throw new Exception($"line {linecount} : 缺少参数的行，请求至少为3个，实际为{parts.Length - 1}个");
						}
						int pointcount = parts.Length - 1;
						int[] vidx = new int[pointcount], vtidx = new int[pointcount], vnidx = new int[pointcount];

						// 分割顶点信息
						for (int i = 0; i < pointcount; i++) {
							string pinfo = parts[i + 1];
							string[] infos = pinfo.Split(PInfoSplitChars);
							vidx[i] = int.Parse(infos[0]);
							int tmp;
							if (infos.Length > 1 && int.TryParse(infos[1], out tmp)) {
								vtidx[i] = tmp;
							}
							else {
								vtidx[i] = 0;//-1;
							}
							if (infos.Length > 2 && int.TryParse(infos[1], out tmp)) {
								vnidx[i] = tmp;
							}
							else {
								vnidx[i] = 0;//-1;
							}
						}

						// 生成三角形面
						for (int i = 2; i < pointcount; i++) {
							PW_Trigle face = new PW_Trigle();
							face.v0 = vidx[0];
							face.v1 = vidx[i - 1];
							face.v2 = vidx[i];

							{ //normal
								Vector3f e1 = v[face.v1] - v[face.v0], e2 = v[face.v2] - v[face.v0];
								face.Normal = (e1 ^ e2).Normalize();
							}
							{ //sp
								//if (vtidx[0] != -1) {
									face.vt0 = vtidx[0];
								//}
								//if (vtidx[i - 1] != -1) {
									face.vt1 = vtidx[i - 1];
								//}
								//if (vtidx[i] != -1) {
									face.vt2 = vtidx[i];
								//}
							}

							trigles.Add(face);
							if(!faceList.ContainsKey(face.v0)) {
								faceList[face.v0] = new List<PW_Trigle>();
							}
							faceList[face.v0].Add(face);
							if (!faceList.ContainsKey(face.v1)) {
								faceList[face.v1] = new List<PW_Trigle>();
							}
							faceList[face.v1].Add(face);
							if (!faceList.ContainsKey(face.v2)) {
								faceList[face.v2] = new List<PW_Trigle>();
							}
							faceList[face.v2].Add(face);
						}

						#endregion
						break;
				}
			}

			if (trigles != null && trigles.Count > 0) {
				if (nobj == null) {
					nobj = new MashTrigle();
				}
				nobj.SetTrigles(Smooth(trigles, v, vt, faceList));
				mashes.Add(nobj);
			}

			return mashes.ToArray();
		}

		static TrigleFace[] Smooth(List<PW_Trigle> faces, List<Vector3f> v, List<Vector2f> vt, Dictionary<int, List<PW_Trigle>> faceList) {
			if (faces == null || faces.Count == 0) {
				return null;
			}
			Dictionary<int, Vector3f> pointNormal = new Dictionary<int, Vector3f>();

			TrigleFace[] re = new TrigleFace[faces.Count];
			foreach (KeyValuePair<int, List<PW_Trigle>> kv in faceList) {
				Vector3f n = new Vector3f();
				foreach (PW_Trigle trigle in kv.Value) {
					n += trigle.Normal;
				}
				n = n.Normalize();
				pointNormal[kv.Key] = n;
			}

			int idx = 0;
			foreach (PW_Trigle trigle in faces) {
				TrigleFace t = new TrigleFace(v[trigle.v0], v[trigle.v1], v[trigle.v2]);
					t.sp0 = vt[trigle.vt0];
				t.sp1 = vt[trigle.vt1];
				t.sp2 = vt[trigle.vt2];
				t.n0 = pointNormal[trigle.v0];
				t.n1 = pointNormal[trigle.v1];
				t.n2 = pointNormal[trigle.v2];

				re[idx] = t;
				idx++;
			}
			return re;
		}
	}
}
