using System;
//using System.Numerics;

//using Vector3f = System.Numerics.Vector3;
using Float = System.Single;

namespace MiRaIRenderBaseType {
	public struct Vector2f {
		public Float x, y;
		public Vector2f(Float v) {
			x = v;
			y = v;
		}
		public Vector2f(Float xv, Float yv) {
			x = xv;
			y = yv;
		}

		#region Index
		public Float this[int i] {
			get {
				switch (i) {
					case 0: return x;
					case 1: return y;
					default:
						throw new IndexOutOfRangeException();
				}
			}
			set {
				switch (i) {
					case 0: x = value; break;
					case 1: y = value; break;
					default:
						throw new IndexOutOfRangeException();
				}
			}
		}
		#endregion

		#region functions
		public Float Length() {
			return MathF.Sqrt(x * x + y * y);
		}
		public Float LengthSquare() {
			return x * x + y * y;
		}
		public Vector2f Normalize() {
			return this / this.LengthSquare();
		}
		public Float MinValue() {
			if (x > y) return y;
			else return x;
		}
		public Float MaxValue() {
			if (x < y) return y;
			else return x;
		}
		#endregion

		#region operators
		public static Vector2f operator +(Vector2f left, Vector2f right) {
			return Vector2f.Add(left, right);
		}
		public static Vector2f operator +(Vector2f left, Float right) {
			return Vector2f.Add(left, right);
		}
		public static Vector2f operator -(Vector2f left, Vector2f right) {
			return Vector2f.Sub(left, right);
		}
		public static Vector2f operator -(Vector2f left, Float right) {
			return Vector2f.Sub(left, right);
		}
		public static Vector2f operator *(Vector2f left, Vector2f right) {
			return Vector2f.Dot(left, right);
		}
		public static Vector2f operator *(Vector2f left, Float right) {
			return Vector2f.Mul(left, right);
		}
		public static Vector2f operator *(Float left, Vector2f right) {
			return Vector2f.Mul(right, left);
		}
		public static Vector2f operator /(Vector2f left, Vector2f right) {
			return Vector2f.Div(left, right);
		}
		public static Vector2f operator /(Vector2f left, Float right) {
			return Vector2f.Div(left, right);
		}
		public static Vector2f operator /(Float left, Vector2f right) {
			return Vector2f.Div(left, right);
		}
		#endregion

		#region static functions
		public static Vector2f Add(Vector2f left, Vector2f right) {
			return new Vector2f(left.x + right.x, left.y + right.y);
		}
		public static Vector2f Add(Vector2f left, Float right) {
			return new Vector2f(left.x + right, left.y + right);
		}
		public static Vector2f Sub(Vector2f left, Vector2f right) {
			return new Vector2f(left.x - right.x, left.y - right.y);
		}
		public static Vector2f Sub(Vector2f left, Float right) {
			return new Vector2f(left.x - right, left.y - right);
		}
		public static Vector2f Mul(Vector2f left, Float right) {
			return new Vector2f(left.x * right, left.y * right);
		}
		public static Vector2f Div(Vector2f left, Vector2f right) {
			return new Vector2f(left.x / right.x, left.y / right.y);
		}
		public static Vector2f Div(Vector2f left, Float right) {
			return new Vector2f(left.x / right, left.y / right);
		}
		public static Vector2f Div(Float left, Vector2f right) {
			return new Vector2f(left / right.x, left / right.y);
		}
		public static Vector2f Dot(Vector2f left, Vector2f right) {
			return new Vector2f(left.x * right.x, left.y * right.y);
		}


		public static Vector2f Min(Vector2f a, Vector2f b) {
			return new Vector2f(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y));
		}
		public static Vector2f Max(Vector2f a, Vector2f b) {
			return new Vector2f(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y));
		}

		public static Float LengthSquare(Vector2f v) {
			return v.x * v.x + v.y * v.y;
		}
		public static Float Length(Vector2f v) {
			return MathF.Sqrt(v.x * v.x + v.y * v.y);
		}
		public static Vector2f UVMerge(Float u, Float v, Vector2f v0, Vector2f v1, Vector2f v2) {
			return (1.0f - u - v) * v0 + u * v1 + v * v2;
		}
		#endregion
	}
}
