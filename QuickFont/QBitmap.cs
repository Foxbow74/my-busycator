using System;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace QuickFont
{
    public class QBitmap:IDisposable
    {
        public Bitmap bitmap;
	    private BitmapData m_bitmapData;


        public QBitmap(string filePath)
        {
            LockBits(new Bitmap(filePath));
        }

        public QBitmap(Bitmap bitmap)
        {
            LockBits(bitmap);
        }

	    public BitmapData BitmapData
	    {
		    get { return m_bitmapData; }
	    }


	    private void LockBits(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            m_bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        }



        public void Clear32(byte _r, byte _g, byte _b, byte _a)
        {
            unsafe
            {
                var sourcePtr = (byte*)(m_bitmapData.Scan0);

                for (var i = 0; i < m_bitmapData.Height; i++)
                {
                    for (var j = 0; j < m_bitmapData.Width; j++)
                    {
                        *(sourcePtr) = _b;
                        *(sourcePtr + 1) = _g;
                        *(sourcePtr + 2) = _r;
                        *(sourcePtr + 3) = _a;

                        sourcePtr += 4;
                    }
                    sourcePtr += m_bitmapData.Stride - m_bitmapData.Width * 4; //move to the end of the line (past unused space)
                }
            }
        }




        /// <summary>
        /// Returns try if the given pixel is empty (i.e. black)
        /// </summary>
        public static unsafe bool EmptyPixel(BitmapData bitmapData, int px, int py)
        {

            var addr = (byte*)(bitmapData.Scan0) + bitmapData.Stride * py + px * 3;
            return (*addr == 0 && *(addr + 1) == 0 && *(addr + 2) == 0);

        }

        /// <summary>
        /// Returns try if the given pixel is empty (i.e. alpha is zero)
        /// </summary>
        public static unsafe bool EmptyAlphaPixel(BitmapData bitmapData, int px, int py, byte alphaEmptyPixelTolerance)
        {

            var addr = (byte*)(bitmapData.Scan0) + bitmapData.Stride * py + px * 4;
            return (*(addr + 3) <= alphaEmptyPixelTolerance);

        }

        /// <summary>
        /// Blits a block of a bitmap data from source to destination, using the luminance of the source to determine the 
        /// alpha of the target. Source must be 24-bit, target must be 32-bit.
        /// </summary>
        public static void BlitMask(BitmapData _source, BitmapData _target, int _srcPx, int _srcPy, int _srcW, int _srcH, int _px, int _py)
        {

            const int sourceBpp = 3;
            const int targetBpp = 4;

	        var targetStartX = Math.Max(_px, 0);
            var targetEndX = Math.Min(_px + _srcW, _target.Width);

            var targetStartY = Math.Max(_py, 0);
            var targetEndY = Math.Min(_py + _srcH, _target.Height);

            var copyW = targetEndX - targetStartX;
            var copyH = targetEndY - targetStartY;

            if (copyW < 0)
            {
                return;
            }

            if (copyH < 0)
            {
                return;
            }

            var sourceStartX = _srcPx + targetStartX - _px;
            var sourceStartY = _srcPy + targetStartY - _py;


            unsafe
            {
                var sourcePtr = (byte*)(_source.Scan0);
                var targetPtr = (byte*)(_target.Scan0);


                var targetY = targetPtr + targetStartY * _target.Stride;
                var sourceY = sourcePtr + sourceStartY * _source.Stride;
                for (var y = 0; y < copyH; y++, targetY += _target.Stride, sourceY += _source.Stride)
                {

                    var targetOffset = targetY + targetStartX * targetBpp;
                    var sourceOffset = sourceY + sourceStartX * sourceBpp;
                    for (var x = 0; x < copyW; x++, targetOffset += targetBpp, sourceOffset += sourceBpp)
                    {
                        var lume = *(sourceOffset) + *(sourceOffset + 1) + *(sourceOffset + 2);

                        lume /= 3;

                        if (lume > 255)
                            lume = 255;

                        *(targetOffset + 3) = (byte)lume;

                    }

                }
            }
        }

        /// <summary>
        /// Blits from source to target. Both source and target must be 32-bit
        /// </summary>
        public static void Blit(BitmapData source, BitmapData target, Rectangle sourceRect, int px, int py)
        {
            Blit(source, target, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height, px, py);
        }

        /// <summary>
        /// Blits from source to target. Both source and target must be 32-bit
        /// </summary>
        public static void Blit(BitmapData source, BitmapData target, int srcPx, int srcPy, int srcW, int srcH, int destX, int destY)
        {

            var bpp = 4;

            int targetStartX, targetEndX;
            int targetStartY, targetEndY;
            int copyW, copyH;

            targetStartX = Math.Max(destX, 0);
            targetEndX = Math.Min(destX + srcW, target.Width);

            targetStartY = Math.Max(destY, 0);
            targetEndY = Math.Min(destY + srcH, target.Height);

            copyW = targetEndX - targetStartX;
            copyH = targetEndY - targetStartY;

            if (copyW < 0)
            {
                return;
            }

            if (copyH < 0)
            {
                return;
            }

            var sourceStartX = srcPx + targetStartX - destX;
            var sourceStartY = srcPy + targetStartY - destY;


            unsafe
            {
                var sourcePtr = (byte*)(source.Scan0);
                var targetPtr = (byte*)(target.Scan0);


                var targetY = targetPtr + targetStartY * target.Stride;
                var sourceY = sourcePtr + sourceStartY * source.Stride;
                for (var y = 0; y < copyH; y++, targetY += target.Stride, sourceY += source.Stride)
                {

                    var targetOffset = targetY + targetStartX * bpp;
                    var sourceOffset = sourceY + sourceStartX * bpp;
                    for (var x = 0; x < copyW*bpp; x++, targetOffset ++, sourceOffset ++)
                        *(targetOffset) = *(sourceOffset);

                }
            }
        }
        

        public unsafe void PutPixel32(int px, int py, byte r, byte g, byte b, byte a)
        {
            var addr = (byte*)(m_bitmapData.Scan0) + m_bitmapData.Stride * py + px * 4;
       
            *addr = b;
            *(addr + 1) = g;
            *(addr + 2) = r;
            *(addr + 3) = a;
        }

        public unsafe void GetPixel32(int px, int py, ref byte r, ref byte g, ref byte b, ref byte a)
        {
            var addr = (byte*)(m_bitmapData.Scan0) + m_bitmapData.Stride * py + px * 4;
        
            b = *addr;
            g = *(addr + 1);
            r = *(addr + 2);
            a = *(addr + 3); 
        }


        public unsafe void PutAlpha32(int px, int py, byte a)
        {
            *((byte*)(m_bitmapData.Scan0) + m_bitmapData.Stride * py + px * 4 + 3) = a;
        }

        public unsafe void GetAlpha32(int px, int py, ref byte a)
        {
            a = *((byte*)(m_bitmapData.Scan0) + m_bitmapData.Stride * py + px * 4 + 3);
        }

        public void DownScale32(int newWidth, int newHeight)
        {
            

            var newBitmap = new QBitmap(new Bitmap(newWidth, newHeight, bitmap.PixelFormat));

            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new Exception("DownsScale32 only works on 32 bit images");

            var xscale = (float)m_bitmapData.Width / newWidth;
            var yscale = (float)m_bitmapData.Height / newHeight;

            byte r = 0, g = 0, b = 0, a = 0;
            var summedR = 0f;
            var summedG = 0f;
            var summedB = 0f;
            var summedA = 0f;

            int left, right, top, bottom;  //the area of old pixels covered by the new bitmap


            float targetStartX, targetEndX;
            float targetStartY, targetEndY;

            float leftF, rightF, topF, bottomF; //edges of new pixel in old pixel coords
            float weight;
            var weightScale = xscale * yscale;
            var totalColourWeight = 0f;

            for (var m = 0; m < newHeight; m++)
            {
                for (var n = 0; n < newWidth; n++)
                {

                    leftF = n * xscale;
                    rightF = (n + 1) * xscale;

                    topF = m * yscale;
                    bottomF = (m + 1) * yscale;

                    left = (int)leftF;
                    right = (int)rightF;

                    top = (int)topF;
                    bottom = (int)bottomF;

                    if (left < 0) left = 0;
                    if (top < 0) top = 0;
                    if (right >= m_bitmapData.Width) right = m_bitmapData.Width - 1;
                    if (bottom >= m_bitmapData.Height) bottom = m_bitmapData.Height - 1;

                    summedR = 0f;
                    summedG = 0f;
                    summedB = 0f;
                    summedA = 0f;
                    totalColourWeight = 0f;

                    for (var j = top; j <= bottom; j++)
                    {
                        for (var i = left; i <= right; i++)
                        {
                            targetStartX = Math.Max(leftF, i);
                            targetEndX = Math.Min(rightF, i + 1);

                            targetStartY = Math.Max(topF, j);
                            targetEndY = Math.Min(bottomF, j + 1);

                            weight = (targetEndX - targetStartX) * (targetEndY - targetStartY);

                            GetPixel32(i, j, ref r, ref g, ref b, ref a);

                            summedA += weight * a;

                            if (a != 0)
                            {
                                summedR += weight * r;
                                summedG += weight * g;
                                summedB += weight * b;
                                totalColourWeight += weight;
                            }

                        }
                    }

                    summedR /= totalColourWeight;
                    summedG /= totalColourWeight;
                    summedB /= totalColourWeight;
                    summedA /= weightScale;

                    if (summedR < 0) summedR = 0f;
                    if (summedG < 0) summedG = 0f;
                    if (summedB < 0) summedB = 0f;
                    if (summedA < 0) summedA = 0f;

                    if (summedR >= 256) summedR = 255;
                    if (summedG >= 256) summedG = 255;
                    if (summedB >= 256) summedB = 255;
                    if (summedA >= 256) summedA = 255;

                    newBitmap.PutPixel32(n, m, (byte)summedR, (byte)summedG, (byte)summedB, (byte)summedA);
                }

            }
            

            this.Free();
            
            this.bitmap = newBitmap.bitmap;
            this.m_bitmapData = newBitmap.m_bitmapData;
        }





        /// <summary>
        /// Sets colour without touching alpha values
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void Colour32(byte r, byte g, byte b)
        {
            unsafe
            {
                byte* addr;
                for (var i = 0; i < m_bitmapData.Width; i++)
                {
                    for (var j = 0; j < m_bitmapData.Height; j++)
                    {
                        addr = (byte*)(m_bitmapData.Scan0) + m_bitmapData.Stride * j + i * 4;
                        *addr = b;
                        *(addr + 1) = g;
                        *(addr + 2) = r;
                    }
                }
            }
        }


        /*

        public void Blur(int radius, int passes)
        {
            QBitmap tmp = new QBitmap(new Bitmap(this.bitmap.Width, this.bitmap.Height, bitmap.PixelFormat));

            byte r=0,g=0,b=0,a=0;
            int summedR, summedG, summedB, summedA;
            int weight = 0;
            int xpos, ypos, x, y, kx, ky;


            for (int pass = 0; pass < passes; pass++)
            {

                //horizontal pass
                for (y = 0; y < bitmap.Height; y++)
                {
                    for (x = 0; x < bitmap.Width; x++)
                    {
                        summedR = summedG = summedB = summedA = weight = 0;
                        for (kx = -radius; kx <= radius; kx++)
                        {
                            xpos = x + kx;
                            if (xpos >= 0 && xpos < bitmap.Width)
                            {
                                GetPixel32(xpos, y, ref r, ref g, ref b, ref a);


                                summedR += r;
                                summedG += g;
                                summedB += b;
                                summedA += a;
                                weight++;
                            }

                        }

                        summedR /= weight;
                        summedG /= weight;
                        summedB /= weight;
                        summedA /= weight;

                        tmp.PutPixel32(x, y, (byte)summedR, (byte)summedG, (byte)summedB, (byte)summedA);
                    }
                }



                
                //vertical pass
                for (x = 0; x < bitmap.Width; ++x)
                {
                    for (y = 0; y < bitmap.Height; ++y)
                    {
                        summedR = summedG = summedB = summedA = weight = 0;
                        for (ky = -radius; ky <= radius; ky++)
                        {
                            ypos = y + ky;
                            if (ypos >= 0 && ypos < bitmap.Height)
                            {
                                tmp.GetPixel32(x, ypos, ref r, ref g, ref b, ref a);

                                summedR += r;
                                summedG += g;
                                summedB += b;
                                summedA += a;
                                weight++;
                            }
                        }

                        summedR /= weight;
                        summedG /= weight;
                        summedB /= weight;
                        summedA /= weight;

                        PutPixel32(x, y, (byte)summedR, (byte)summedG, (byte)summedB, (byte)summedA);

                    }
                } 

            }

            tmp.Free();

        }*/




        public void BlurAlpha(int radius, int passes)
        {
            var tmp = new QBitmap(new Bitmap(this.bitmap.Width, this.bitmap.Height, bitmap.PixelFormat));

            byte a = 0;
            int summedA;
            var weight = 0;
            int xpos, ypos, x, y, kx, ky;
            var width = bitmap.Width;
            var height = bitmap.Height;

            for (var pass = 0; pass < passes; pass++)
            {

                //horizontal pass
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        summedA = weight = 0;
                        for (kx = -radius; kx <= radius; kx++)
                        {
                            xpos = x + kx;
                            if (xpos >= 0 && xpos < width)
                            {
                                GetAlpha32(xpos, y, ref a);
                                summedA += a;
                                weight++;
                            }
                        }

                        summedA /= weight;
                        tmp.PutAlpha32(x, y, (byte)summedA);
                    }
                }




                //vertical pass
                for (x = 0; x <width; ++x)
                {
                    for (y = 0; y < height; ++y)
                    {
                        summedA = weight = 0;
                        for (ky = -radius; ky <= radius; ky++)
                        {
                            ypos = y + ky;
                            if (ypos >= 0 && ypos < height)
                            {
                                tmp.GetAlpha32(x, ypos,ref a);
                                summedA += a;
                                weight++;
                            }
                        }

                        summedA /= weight;

                        PutAlpha32(x, y, (byte)summedA);

                    }
                }

            }

            tmp.Free();

        }






        public void Free()
        {
            bitmap.UnlockBits(m_bitmapData);
            bitmap.Dispose();
        }

	    public void Dispose()
	    {
			bitmap.UnlockBits(m_bitmapData);
	    }

	    public void ApplyLightMaps(Bitmap[] _lightmaps)
	    {

			
			unsafe
			{

				var lightQMaps = new QBitmap[_lightmaps.Length];
				var ptrs = new byte*[_lightmaps.Length];
				for (var index = 0; index < _lightmaps.Length; index++)
				{
					lightQMaps[index] = new QBitmap(_lightmaps[index]);
					ptrs[index] = (byte*)(lightQMaps[index].m_bitmapData.Scan0);
				}

				var sourcePtr = (byte*)(m_bitmapData.Scan0);

				byte r, g, b;
				byte vr, vg, vb;
				for (var i = 0; i < m_bitmapData.Height; i++)
				{
					for (var j = 0; j < m_bitmapData.Width; j++)
					{
						vr = *(sourcePtr + 2);
						vg = *(sourcePtr + 1);
						vb = *(sourcePtr + 0);

						r = 0;
						g = 0;
						b = 0;
						for (var index = 0; index < _lightmaps.Length; index++)
						{
							r = Math.Max(r, *(ptrs[index] + 2));
							g = Math.Max(g,*(ptrs[index] + 1));
							b = Math.Max(b,*(ptrs[index] + 0));
							ptrs[index] += 4;
						}

						r = (byte)(r * vr / 255);
						g = (byte)(g*vg/255);
						b = (byte)(b*vb/255);


						//if (vr > 0 && r>0)
						//{
						//	vr += 1;
						//}

						*(sourcePtr) = b;
						*(sourcePtr + 1) = g;
						*(sourcePtr + 2) = r;
						//*(sourcePtr + 3) = _a;

						sourcePtr += 4;
					}
					sourcePtr += m_bitmapData.Stride - m_bitmapData.Width * 4; //move to the end of the line (past unused space)
					for (var index = 0; index < _lightmaps.Length; index++)
					{
						ptrs[index] += m_bitmapData.Stride - m_bitmapData.Width * 4; //move to the end of the line (past unused space)
					}
				}

				for (var index = 0; index < _lightmaps.Length; index++)
				{
					lightQMaps[index].Dispose();
				}
			}
		}
    }
}
