using Math = System.MathF;
using Float = System.Single;
using System;

namespace MiRaIRender.BaseType {
	public struct Vector3f {
		public Float x, y, z;
		public Vector3f(Float v) {
			x = v;
			y = v;
			z = v;
		}
		public Vector3f(Float xv, Float yv, Float zv) {
			x = xv;
			y = yv;
			z = zv;
		}

		public override string ToString() {
			return $"V3f:{{{x:0.000}, {y:0.000}, {z:0.000} }}";
		}

		#region Index
		public Float this[int i] {
			get {
				switch (i) {
					case 0: return x;
					case 1: return y;
					case 2: return z;
					default:
						throw new IndexOutOfRangeException();
				}
			}
			set {
				switch (i) {
					case 0: x = value; break;
					case 1: y = value; break;
					case 2: z = value; break;
					default:
						throw new IndexOutOfRangeException();
				}
			}
		}
		#endregion

		#region functions
		public Float Length() {
			return Math.Sqrt(x * x + y * y + z * z);
		}
		public Float LengthSquare() {
			return x * x + y * y + z * z;
		}
		public Vector3f Normalize() { // todo : 优化
			Float len = this.Length();
			return this / len;
		}
		public Float MinValue() {
			float re = x;
			if (re > y) re = y;
			if (re > z) re = z;
			return re;
		}
		public Float MaxValue() {
			float re = x;
			if (re < y) re = y;
			if (re < z) re = z;
			return re;
		}
		#endregion

		#region operators
		public static Vector3f operator +(Vector3f left, Vector3f right) {
			return Vector3f.Add(left, right);
		}
		public static Vector3f operator +(Vector3f left, Float right) {
			return Vector3f.Add(left, right);
		}
		public static Vector3f operator -(Vector3f v) {
			return new Vector3f(-v.x, -v.y, -v.z);
		}
		public static Vector3f operator -(Vector3f left, Vector3f right) {
			return Vector3f.Sub(left, right);
		}
		public static Vector3f operator -(Vector3f left, Float right) {
			return Vector3f.Sub(left, right);
		}
		/// <summary>
		/// 对应坐标相乘。点积用操作符&，叉积用操作符^
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static Vector3f operator *(Vector3f left, Vector3f right) {
			return Vector3f.Mul(left, right);
		}
		public static Vector3f operator *(Vector3f left, Float right) {
			return Vector3f.Mul(left, right);
		}
		public static Vector3f operator *(Float left, Vector3f right) {
			return Vector3f.Mul(right, left);
		}
		public static Vector3f operator /(Vector3f left, Vector3f right) {
			return Vector3f.Div(left, right);
		}
		public static Vector3f operator /(Vector3f left, Float right) {
			return Vector3f.Div(left, right);
		}
		public static Vector3f operator /(Float left, Vector3f right) {
			return Vector3f.Div(left, right);
		}
		public static Vector3f operator ^(Vector3f left, Vector3f right) {
			return Vector3f.Cross(left, right);
		}

		public static Float operator &(Vector3f left, Vector3f right) {
			return Vector3f.Dot(left, right);
		}
		#endregion

		#region static functions
		public static Vector3f Add(Vector3f left, Vector3f right) {
			return new Vector3f(left.x + right.x, left.y + right.y, left.z + right.z);
		}
		public static Vector3f Add(Vector3f left, Float right) {
			return new Vector3f(left.x + right, left.y + right, left.z + right);
		}
		public static Vector3f Sub(Vector3f left, Vector3f right) {
			return new Vector3f(left.x - right.x, left.y - right.y, left.z - right.z);
		}
		public static Vector3f Sub(Vector3f left, Float right) {
			return new Vector3f(left.x - right, left.y - right, left.z - right);
		}
		public static Vector3f Mul(Vector3f left, Vector3f right) {
			return new Vector3f(left.x * right.x, left.y * right.y, left.z * right.z);
		}
		public static Vector3f Mul(Vector3f left, Float right) {
			return new Vector3f(left.x * right, left.y * right, left.z * right);
		}
		public static Vector3f Div(Vector3f left, Vector3f right) {
			return new Vector3f(left.x / right.x, left.y / right.y, left.z / right.z);
		}
		public static Vector3f Div(Vector3f left, Float right) {
			return new Vector3f(left.x / right, left.y / right, left.z / right);
		}
		public static Vector3f Div(Float left, Vector3f right) {
			return new Vector3f(left / right.x, left / right.y, left / right.z);
		}
		public static Float Dot(Vector3f left, Vector3f right) {
			return left.x * right.x + left.y * right.y + left.z * right.z;
		}
		public static Vector3f Cross(Vector3f left, Vector3f right) {
			return new Vector3f(left.y * right.z - left.z * right.y,
								left.z * right.x - left.x * right.z,
								left.x * right.y - left.y * right.x);
		}

		public static Vector3f Min(Vector3f a, Vector3f b) {
			return new Vector3f(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
		}
		public static Vector3f Max(Vector3f a, Vector3f b) {
			return new Vector3f(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));
		}

		public static Float LengthSquare(Vector3f v) {
			return v.x * v.x + v.y * v.y + v.z * v.z;
		}
		public static Float Length(Vector3f v) {
			return Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
		}


		public static Vector3f UVMerge(Float u, Float v, Vector3f v0, Vector3f v1, Vector3f v2) {
			return (1.0f - u - v) * v0 + u * v1 + v * v2;
		}
		#endregion

		#region static values
		public static readonly Vector3f Zero = new Vector3f(0.0f);
		public static readonly Vector3f OneX = new Vector3f(1.0f, 0.0f, 0.0f);
		public static readonly Vector3f OneY = new Vector3f(0.0f, 1.0f, 0.0f);
		public static readonly Vector3f OneZ = new Vector3f(0.0f, 0.0f, 1.0f);

		#endregion
	}
}
