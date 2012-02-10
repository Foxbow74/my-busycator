using System;
using System.Collections.Generic;

namespace GameCore.Misc
{
	public struct Vector2 : IEquatable<Vector2>
	{
		#region Private Fields

		private static readonly Vector2 m_zeroVector = new Vector2(0f, 0f);
		private static readonly Vector2 m_unitVector = new Vector2(1f, 1f);
		private static readonly Vector2 m_unitXVector = new Vector2(1f, 0f);
		private static readonly Vector2 m_unitYVector = new Vector2(0f, 1f);

		#endregion Private Fields

		#region Public Fields

		public float X;
		public float Y;

		#endregion Public Fields

		#region Properties

		public static Vector2 Zero
		{
			get { return m_zeroVector; }
		}

		public static Vector2 One
		{
			get { return m_unitVector; }
		}

		public static Vector2 UnitX
		{
			get { return m_unitXVector; }
		}

		public static Vector2 UnitY
		{
			get { return m_unitYVector; }
		}

		#endregion Properties

		#region Constructors

		public Vector2(float _x, float _y)
		{
			X = _x;
			Y = _y;
		}

		public Vector2(float _value)
		{
			X = _value;
			Y = _value;
		}

		#endregion Constructors

		#region Public Methods

		public IEnumerable<Vector2> GetLineToPoints(Vector2 _point, float _d)
		{
			var lx = Math.Abs(_point.X - X);
			var ly = Math.Abs(_point.Y - Y);

			var max = Math.Max(lx, ly) / _d;

			float dx = Math.Sign(_point.X - X) * _d;
			float dy = Math.Sign(_point.Y - Y) * _d;

			var x = X;
			var y = Y;

			if (lx > ly)
			{
				dy *= ly / lx;
			}
			else if ((lx < ly))
			{
				dx *= lx / ly;
			}

			yield return this;
			for (var i = 0; i <= max; ++i)
			{
				x += dx;
				y += dy;
				yield return new Vector2(x, y);
			}
		}

		public bool Equals(Vector2 _other)
		{
			return (X == _other.X) && (Y == _other.Y);
		}

		public static Vector2 Add(Vector2 _value1, Vector2 _value2)
		{
			_value1.X += _value2.X;
			_value1.Y += _value2.Y;
			return _value1;
		}

		public static void Add(ref Vector2 _value2, out Vector2 _result)
		{
			var value1 = new Vector2();
			Add(ref value1, ref _value2, out _result);
		}

		public static void Add(ref Vector2 _value1, ref Vector2 _value2, out Vector2 _result)
		{
			_result.X = _value1.X + _value2.X;
			_result.Y = _value1.Y + _value2.Y;
		}

		public static Vector2 Barycentric(Vector2 _value1, Vector2 _value2, Vector2 _value3, float _amount1, float _amount2)
		{
			return new Vector2(
				MathHelper.Barycentric(_value1.X, _value2.X, _value3.X, _amount1, _amount2),
				MathHelper.Barycentric(_value1.Y, _value2.Y, _value3.Y, _amount1, _amount2));
		}

		public static void Barycentric(ref Vector2 _value1, ref Vector2 _value2, ref Vector2 _value3, float _amount1,
		                               float _amount2, out Vector2 _result)
		{
			_result = new Vector2(
				MathHelper.Barycentric(_value1.X, _value2.X, _value3.X, _amount1, _amount2),
				MathHelper.Barycentric(_value1.Y, _value2.Y, _value3.Y, _amount1, _amount2));
		}

		public static Vector2 CatmullRom(Vector2 _value1, Vector2 _value2, Vector2 _value3, Vector2 _value4, float _amount)
		{
			return new Vector2(
				MathHelper.CatmullRom(_value1.X, _value2.X, _value3.X, _value4.X, _amount),
				MathHelper.CatmullRom(_value1.Y, _value2.Y, _value3.Y, _value4.Y, _amount));
		}

		public static void CatmullRom(ref Vector2 _value1, ref Vector2 _value2, ref Vector2 _value3, ref Vector2 _value4,
		                              float _amount, out Vector2 _result)
		{
			_result = new Vector2(
				MathHelper.CatmullRom(_value1.X, _value2.X, _value3.X, _value4.X, _amount),
				MathHelper.CatmullRom(_value1.Y, _value2.Y, _value3.Y, _value4.Y, _amount));
		}

		public static Vector2 Clamp(Vector2 _value1, Vector2 _min, Vector2 _max)
		{
			return new Vector2(
				MathHelper.Clamp(_value1.X, _min.X, _max.X),
				MathHelper.Clamp(_value1.Y, _min.Y, _max.Y));
		}

		public static void Clamp(ref Vector2 _value1, ref Vector2 _min, ref Vector2 _max, out Vector2 _result)
		{
			_result = new Vector2(
				MathHelper.Clamp(_value1.X, _min.X, _max.X),
				MathHelper.Clamp(_value1.Y, _min.Y, _max.Y));
		}

		public static float Distance(Vector2 _value1, Vector2 _value2)
		{
			float v1 = _value1.X - _value2.X, v2 = _value1.Y - _value2.Y;
			return (float) Math.Sqrt((v1*v1) + (v2*v2));
		}

		public static void Distance(ref Vector2 _value1, ref Vector2 _value2, out float _result)
		{
			float v1 = _value1.X - _value2.X, v2 = _value1.Y - _value2.Y;
			_result = (float) Math.Sqrt((v1*v1) + (v2*v2));
		}

		public static float DistanceSquared(Vector2 _value1, Vector2 _value2)
		{
			float v1 = _value1.X - _value2.X, v2 = _value1.Y - _value2.Y;
			return (v1*v1) + (v2*v2);
		}

		public static void DistanceSquared(ref Vector2 _value1, ref Vector2 _value2, out float _result)
		{
			float v1 = _value1.X - _value2.X, v2 = _value1.Y - _value2.Y;
			_result = (v1*v1) + (v2*v2);
		}

		public static Vector2 Divide(Vector2 _value1, Vector2 _value2)
		{
			_value1.X /= _value2.X;
			_value1.Y /= _value2.Y;
			return _value1;
		}

		public static void Divide(ref Vector2 _value1, ref Vector2 _value2, out Vector2 _result)
		{
			_result.X = _value1.X/_value2.X;
			_result.Y = _value1.Y/_value2.Y;
		}

		public static Vector2 Divide(Vector2 _value1, float _divider)
		{
			var factor = 1/_divider;
			_value1.X *= factor;
			_value1.Y *= factor;
			return _value1;
		}

		public static void Divide(ref Vector2 _value1, float _divider, out Vector2 _result)
		{
			var factor = 1/_divider;
			_result.X = _value1.X*factor;
			_result.Y = _value1.Y*factor;
		}

		public static float Dot(Vector2 _value1, Vector2 _value2)
		{
			return (_value1.X*_value2.X) + (_value1.Y*_value2.Y);
		}

		public static void Dot(ref Vector2 _value1, ref Vector2 _value2, out float _result)
		{
			_result = (_value1.X*_value2.X) + (_value1.Y*_value2.Y);
		}

		public override bool Equals(object _obj)
		{
			if (_obj is Vector2)
			{
				return Equals(this);
			}

			return false;
		}

		public static Vector2 Reflect(Vector2 _vector, Vector2 _normal)
		{
			Vector2 result;
			var val = 2.0f*((_vector.X*_normal.X) + (_vector.Y*_normal.Y));
			result.X = _vector.X - (_normal.X*val);
			result.Y = _vector.Y - (_normal.Y*val);
			return result;
		}

		public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
		{
			var val = 2.0f*((vector.X*normal.X) + (vector.Y*normal.Y));
			result.X = vector.X - (normal.X*val);
			result.Y = vector.Y - (normal.Y*val);
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode();
		}

		public static Vector2 Hermite(Vector2 _value1, Vector2 _tangent1, Vector2 _value2, Vector2 _tangent2, float _amount)
		{
			Vector2 result;
			Hermite(ref _value1, ref _tangent1, ref _value2, ref _tangent2, _amount, out result);
			return result;
		}

		public static void Hermite(ref Vector2 _value1, ref Vector2 _tangent1, ref Vector2 _value2, ref Vector2 _tangent2,
		                           float _amount, out Vector2 _result)
		{
			_result.X = MathHelper.Hermite(_value1.X, _tangent1.X, _value2.X, _tangent2.X, _amount);
			_result.Y = MathHelper.Hermite(_value1.Y, _tangent1.Y, _value2.Y, _tangent2.Y, _amount);
		}

		public float Length()
		{
			return (float) Math.Sqrt((X*X) + (Y*Y));
		}

		public float LengthSquared()
		{
			return (X*X) + (Y*Y);
		}

		public static Vector2 Lerp(Vector2 _value1, Vector2 _value2, float _amount)
		{
			return new Vector2(
				MathHelper.Lerp(_value1.X, _value2.X, _amount),
				MathHelper.Lerp(_value1.Y, _value2.Y, _amount));
		}

		public static void Lerp(ref Vector2 _value1, ref Vector2 _value2, float _amount, out Vector2 _result)
		{
			_result = new Vector2(
				MathHelper.Lerp(_value1.X, _value2.X, _amount),
				MathHelper.Lerp(_value1.Y, _value2.Y, _amount));
		}

		public static Vector2 Max(Vector2 _value1, Vector2 _value2)
		{
			return new Vector2(_value1.X > _value2.X ? _value1.X : _value2.X,
			                   _value1.Y > _value2.Y ? _value1.Y : _value2.Y);
		}

		public static void Max(ref Vector2 _value1, ref Vector2 _value2, out Vector2 _result)
		{
			_result.X = _value1.X > _value2.X ? _value1.X : _value2.X;
			_result.Y = _value1.Y > _value2.Y ? _value1.Y : _value2.Y;
		}

		public static Vector2 Min(Vector2 _value1, Vector2 _value2)
		{
			return new Vector2(_value1.X < _value2.X ? _value1.X : _value2.X,
			                   _value1.Y < _value2.Y ? _value1.Y : _value2.Y);
		}

		public static void Min(ref Vector2 _value1, ref Vector2 _value2, out Vector2 result)
		{
			result.X = _value1.X < _value2.X ? _value1.X : _value2.X;
			result.Y = _value1.Y < _value2.Y ? _value1.Y : _value2.Y;
		}

		public static Vector2 Multiply(Vector2 _value1, Vector2 _value2)
		{
			_value1.X *= _value2.X;
			_value1.Y *= _value2.Y;
			return _value1;
		}

		public static Vector2 Multiply(Vector2 _value1, float _scaleFactor)
		{
			_value1.X *= _scaleFactor;
			_value1.Y *= _scaleFactor;
			return _value1;
		}

		public static void Multiply(ref Vector2 _value1, float _scaleFactor, out Vector2 _result)
		{
			_result.X = _value1.X*_scaleFactor;
			_result.Y = _value1.Y*_scaleFactor;
		}

		public static void Multiply(ref Vector2 _value1, ref Vector2 _value2, out Vector2 _result)
		{
			_result.X = _value1.X*_value2.X;
			_result.Y = _value1.Y*_value2.Y;
		}

		public static Vector2 Negate(Vector2 value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}

		public static void Negate(ref Vector2 _value, out Vector2 _result)
		{
			_result.X = -_value.X;
			_result.Y = -_value.Y;
		}

		public void Normalize()
		{
			var val = 1.0f/(float) Math.Sqrt((X*X) + (Y*Y));
			X *= val;
			Y *= val;
		}

		public static Vector2 Normalize(Vector2 _value)
		{
			var val = 1.0f/(float) Math.Sqrt((_value.X*_value.X) + (_value.Y*_value.Y));
			_value.X *= val;
			_value.Y *= val;
			return _value;
		}

		public static void Normalize(ref Vector2 _value, out Vector2 _result)
		{
			var val = 1.0f/(float) Math.Sqrt((_value.X*_value.X) + (_value.Y*_value.Y));
			_result.X = _value.X*val;
			_result.Y = _value.Y*val;
		}

		public static Vector2 SmoothStep(Vector2 _value1, Vector2 _value2, float amount)
		{
			return new Vector2(
				MathHelper.SmoothStep(_value1.X, _value2.X, amount),
				MathHelper.SmoothStep(_value1.Y, _value2.Y, amount));
		}

		public static void SmoothStep(ref Vector2 _value1, ref Vector2 _value2, float amount, out Vector2 result)
		{
			result = new Vector2(
				MathHelper.SmoothStep(_value1.X, _value2.X, amount),
				MathHelper.SmoothStep(_value1.Y, _value2.Y, amount));
		}

		public static Vector2 Subtract(Vector2 _value1, Vector2 _value2)
		{
			_value1.X -= _value2.X;
			_value1.Y -= _value2.Y;
			return _value1;
		}

		public static void Subtract(ref Vector2 _value1, ref Vector2 _value2, out Vector2 result)
		{
			result.X = _value1.X - _value2.X;
			result.Y = _value1.Y - _value2.Y;
		}

		public override string ToString()
		{
			return string.Format("{{X:{0} Y:{1}}}", new object[]
			                                        	{
			                                        		X.ToString(), Y.ToString()
			                                        	});
		}

		#endregion Public Methods

		#region Operators

		public static Vector2 operator -(Vector2 value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}


		public static bool operator ==(Vector2 _value1, Vector2 _value2)
		{
			return _value1.X == _value2.X && _value1.Y == _value2.Y;
		}


		public static bool operator !=(Vector2 _value1, Vector2 _value2)
		{
			return _value1.X != _value2.X || _value1.Y != _value2.Y;
		}


		public static Vector2 operator +(Vector2 _value1, Vector2 _value2)
		{
			_value1.X += _value2.X;
			_value1.Y += _value2.Y;
			return _value1;
		}


		public static Vector2 operator -(Vector2 _value1, Vector2 _value2)
		{
			_value1.X -= _value2.X;
			_value1.Y -= _value2.Y;
			return _value1;
		}


		public static Vector2 operator *(Vector2 _value1, Vector2 _value2)
		{
			_value1.X *= _value2.X;
			_value1.Y *= _value2.Y;
			return _value1;
		}


		public static Vector2 operator *(Vector2 value, float scaleFactor)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}


		public static Vector2 operator *(float scaleFactor, Vector2 value)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}


		public static Vector2 operator /(Vector2 _value1, Vector2 _value2)
		{
			_value1.X /= _value2.X;
			_value1.Y /= _value2.Y;
			return _value1;
		}


		public static Vector2 operator /(Vector2 _value1, float divider)
		{
			var factor = 1/divider;
			_value1.X *= factor;
			_value1.Y *= factor;
			return _value1;
		}

		#endregion Operators
	}
}