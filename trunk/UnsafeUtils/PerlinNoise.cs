using System;
using System.Drawing;

namespace UnsafeUtils
{
	public static class PerlinNoise
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_width"></param>
		/// <param name="_height"></param>
		/// <param name="_frequency">0-1</param>
		/// <param name="_amplitude">0-1</param>
		/// <param name="_detalization">0-1</param>
		/// <param name="_octaves">1-10</param>
		/// <param name="_seed"></param>
		/// <param name="_minVal"></param>
		/// <param name="_maxVal"></param>
		/// <returns></returns>
		public static Bitmap GenerateBitmap(int _width,int _height, float _frequency,float _amplitude,float _detalization,int _octaves,int _seed, int _minVal=0, int _maxVal=255)	
		{
			var returnValue = new Bitmap(_width, _height);
			var noise = GenerateNoise(_seed, _width, _height);
			for (var x = 0; x < _width; ++x)	
			{	
			    for (var y = 0; y < _height; ++y)
			    {
			    	var value = GetValue(x, y, _width, _height, _frequency, _amplitude, _detalization, _octaves, noise);
			    	value = (value*0.5f) + 0.5f;
			    	value *= 255;
					if(value>_maxVal || value<_minVal) continue;
					//var rgbValue = value > _maxVal ? _maxVal : (value < _minVal ? _minVal : (int)value);
					var rgbValue = (int)value;
					returnValue.SetPixel(x, y, Color.FromArgb(rgbValue, rgbValue, rgbValue));
			    }
			}	
		    return returnValue;	
		}

		public static Bitmap GenerateBitmap(int _width, int _height, float _frequency, float _amplitude, float _detalization, int _octaves, int _seed, float[,] _noise, int _minVal = 0, int _maxVal = 255)
		{
			var returnValue = new Bitmap(_width, _height);
			for (var x = 0; x < _width; ++x)
			{
				for (var y = 0; y < _height; ++y)
				{
					var value = GetValue(x, y, _width, _height, _frequency, _amplitude, _detalization, _octaves, _noise);
					value = (value * 0.5f) + 0.5f;
					value *= 255;
					if (value > _maxVal || value < _minVal) continue;
					//var rgbValue = value > _maxVal ? _maxVal : (value < _minVal ? _minVal : (int)value);
					var rgbValue = (int)value;
					returnValue.SetPixel(x, y, Color.FromArgb(rgbValue, rgbValue, rgbValue));
				}
			}
			return returnValue;
		}

		public static float[,] Generate(int _width, int _height, float _frequency, float _amplitude, float _detalization, int _octaves, int _seed)
		{
			var noise = GenerateNoise(_seed, _width, _height);
			var returnValue = new float[_width,_height];
			for (var x = 0; x < _width; ++x)
			{
				for (var y = 0; y < _height; ++y)
				{
					returnValue[x,y] = GetValue(x, y, _width, _height, _frequency, _amplitude, _detalization, _octaves, noise);
				}
			}
			return returnValue;
		}

		private static float GetValue(int _x, int _y, int _width, int _height, float _frequency, float _amplitude, float _detalization, int _octaves, float[,] _noise)
		{
			var finalValue = 0.0f;

			for (var i = 0; i < _octaves; ++i)
			{
				finalValue += GetSmoothNoise(_x*_frequency, _y*_frequency, _width, _height, _noise)*_amplitude;
				_frequency *= 2.0f;
				_amplitude *= _detalization;
			}

			if (finalValue < -1.0f)
			{
				finalValue = -1.0f;
			}

			else if (finalValue > 1.0f)
			{
				finalValue = 1.0f;
			}

			return finalValue;
		}


		private static float GetSmoothNoise(float _x, float _y, int _width, int _height, float[,] _noise)
		{
			var fractionX = _x - (int) _x;
			var fractionY = _y - (int) _y;
			var x1 = ((int) _x + _width)%_width;
			var y1 = ((int) _y + _height)%_height;
			var x2 = ((int) _x + _width - 1)%_width;
			var y2 = ((int) _y + _height - 1)%_height;


			var finalValue = 0.0f;
			finalValue += fractionX*fractionY*_noise[x1, y1];
			finalValue += fractionX*(1 - fractionY)*_noise[x1, y2];
			finalValue += (1 - fractionX)*fractionY*_noise[x2, y1];
			finalValue += (1 - fractionX)*(1 - fractionY)*_noise[x2, y2];
			return finalValue;
		}


		private static float[,] GenerateNoise(int _seed, int _width, int _height)
		{
			var noise = new float[_width,_height];
			var randomGenerator = new Random(_seed);
			for (var x = 0; x < _width; ++x)
			{
				for (var y = 0; y < _height; ++y)
				{
					noise[x, y] = ((float) (randomGenerator.NextDouble()) - 0.5f)*2.0f;
				}
			}
			return noise;
		}
	}
}