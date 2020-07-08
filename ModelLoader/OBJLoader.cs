using MiRaIRender.BaseType;
using MiRaIRender.BaseType.Materials;
using System;
using System.Collections.Generic;
using System.IO;
using Float = System.Single;
namespace ModelLoader {
	public class OBJLoader {
		public static char[] LineSplitChars = new char[] { ' ' };
		public static char[] PInfoSplitChars = new char[] { '/' };

		public static MashTrigle[] LoadModel(string filePath) {
			string[] lines = File.ReadAllLines(filePath);

			List<MashTrigle> mashes = new List<MashTrigle>();
			List<Vector3f> v = new List<Vector3f>() { new Vector3f() };
			List<Vector2f> vt = new List<Vector2f>() { new Vector2f() };
			List<Vector3f> vn = new List<Vector3f>() { new Vector3f() };

			MashTrigle nobj = null;
			List<TrigleFace> trigles = new List<TrigleFace>();

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
							nobj.SetTrigles(trigles.ToArray());
							mashes.Add(nobj);
						}
						nobj = new MashTrigle();
						trigles = new List<TrigleFace>();
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
								vtidx[i] = -1;
							}
							if (infos.Length > 2 && int.TryParse(infos[1], out tmp)) {
								vnidx[i] = tmp;
							}
							else {
								vnidx[i] = -1;
							}
						}

						// 生成三角形面
						for (int i = 2; i < pointcount; i++) {
							TrigleFace face = new TrigleFace(v[vidx[0]], v[vidx[i - 1]], v[vidx[i]]);
							{ //normal
								Vector3f facenormal = new Vector3f();
								if (vnidx[0] == -1 || vnidx[i - 1] == -1 || vnidx[i] == -1) {
									facenormal = (face.e1 ^ face.e2).Normalize();
								}

								if (vnidx[0] == -1) {
									face.n0 = facenormal;
								}
								else {
									face.n0 = vn[vnidx[0]];
								}
								if (vnidx[i - 1] == -1) {
									face.n1 = facenormal;
								}
								else {
									face.n1 = vn[vnidx[i - 1]];
								}
								if (vnidx[i] == -1) {
									face.n2 = facenormal;
								}
								else {
									face.n2 = vn[vnidx[i]];
								}
							}
							{ //sp
								if (vtidx[0] != -1) {
									face.sp0 = vt[vtidx[0]];
								}
								if (vtidx[i - 1] != -1) {
									face.sp1 = vt[vtidx[i - 1]];
								}
								if (vtidx[i] != -1) {
									face.sp2 = vt[vtidx[i]];
								}
							}

							trigles.Add(face);
						}

						#endregion
						break;
				}
			}

			if (trigles != null && trigles.Count > 0) {
				if (nobj == null) {
					nobj = new MashTrigle();
				}
				nobj.SetTrigles(trigles.ToArray());
				mashes.Add(nobj);
			}

			return mashes.ToArray();
		}
	}
	//public class MTLLoader {
	//	public static char[] LineSplitChars = new char[] { ' ' };
	//	public static char[] PInfoSplitChars = new char[] { '/' };

	//	public static Dictionary<string, Material> GetMaterials(string filePath) {
	//		string[] lines = File.ReadAllLines(filePath);
	//		foreach (string line in lines) {
	//			if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) {
	//				continue;
	//			}
	//			string[] parts = line.Split(LineSplitChars, StringSplitOptions.RemoveEmptyEntries);
	//			switch (parts[0].ToLower()) {
	//				case "newmtl":

	//					break;
	//				case "kd":

	//					break;
	//				case "ks":

	//					break;
	//				default:
	//					break;
	//			}
	//		}
	//	}
	//}
}
