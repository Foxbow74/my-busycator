#region

using Microsoft.Xna.Framework;
using Point = GameCore.Misc.Point;

#endregion

namespace RGL1
{
	public static class VectorHelpers
	{
		public static float GetDistanceToVector(this Point _pnt, Point _point)
		{
			var lineVector = new Vector2(_point.X, _point.Y);
			lineVector.Normalize();

			var myVector = new Vector2(_pnt.X, _pnt.Y);

			var distanceAlongLine = Vector2.Dot(myVector, lineVector) - Vector2.Dot(Vector2.Zero, lineVector);
			Vector2 nearestPoint;
			if (distanceAlongLine < 0)
			{
				nearestPoint = Vector2.Zero;
			}
			else
			{
				nearestPoint = distanceAlongLine*lineVector;
			}

			return Vector2.Distance(nearestPoint, myVector);
		}
	}
}