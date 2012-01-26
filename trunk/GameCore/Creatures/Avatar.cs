#region

using GameCore.Misc;

#endregion

namespace GameCore.Creatures
{
	public class Avatar : Intelligent
	{
		public Avatar() : base(Point.Zero, 100)
		{
			m_silence = false;
		}

		public override ETiles Tile
		{
			get { return ETiles.AVATAR; }
		}

		public override string Name
		{
			get { return "аватар"; }
		}
	}
}