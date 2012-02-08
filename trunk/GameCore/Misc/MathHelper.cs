namespace GameCore.Misc
{
	public static class MathHelper
	{
		public static float Lerp(float _value1, float _value2, float _amount)
		{
			return _value1 + (_value2 - _value1) * _amount;
		}

		public static float SmoothStep(float _value1, float _value2, float _amount)
		{
			// It is expected that 0 < amount < 1
			// If amount < 0, return value1
			// If amount > 1, return value2
			var result = Clamp(_amount, 0f, 1f);
			return Hermite(_value1, 0f, _value2, 0f, result);
		}

		public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
		{
			// Using formula from http://www.mvps.org/directx/articles/catmull/
			// Internally using doubles not to lose precission
			double amountSquared = amount * amount;
			double amountCubed = amountSquared * amount;
			return (float)(0.5 * (2.0 * value2 +
				(value3 - value1) * amount +
				(2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
				(3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
		}

		public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
		{
			return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
		}

		public static float Hermite(float _value1, float _tangent1, float _value2, float _tangent2, float _amount)
		{
			// All transformed to double not to lose precission
			// Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
			double v1 = _value1, v2 = _value2, t1 = _tangent1, t2 = _tangent2, s = _amount, result;
			var sCubed = s * s * s;
			var sSquared = s * s;

			if (_amount == 0f)
				result = _value1;
			else if (_amount == 1f)
				result = _value2;
			else
				result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
					(3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
					t1 * s +
					v1;
			return (float)result;
		}

		public static float Clamp(float _value, float _min, float _max)
		{
			// First we check to see if we're greater than the max
			_value = (_value > _max) ? _max : _value;

			// Then we check to see if we're less than the min.
			_value = (_value < _min) ? _min : _value;

			// There's no check to see if min > max.
			return _value;
		}
	}
}
