using System.Diagnostics;
using GameCore.Essences;
using GameCore.Misc;

namespace GameCore.Creatures.Dummies
{
	public class Splatter:ITileInfoProvider
	{
		private readonly FColor m_color;
		private readonly EDirections m_direction;
		private readonly int m_tileIndex;

		public const int COUNT=13;

		public Splatter(FColor _color, int _tileIndex)
		{
			m_color = _color;
			m_tileIndex = _tileIndex;
			m_direction = World.Rnd.GetRandomDirection();
		}

		#region ITileInfoProvider Members

		public ETileset Tileset
		{
			get { return ETileset.SPLATTERS; }
		}

		public FColor LerpColor
		{
			get { return m_color; }
		}

		public EDirections Direction
		{
			get { return m_direction; }
		}

		public bool IsCorpse
		{
			get { return false; }
		}

		public int TileIndex
		{
			get { return m_tileIndex; }
		}

		#endregion
	}
}