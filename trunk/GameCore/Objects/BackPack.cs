using System;

namespace GameCore.Objects
{
	public class BackPack : Container
	{
		public override ETiles Tile
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { return "рюкзак"; }
		}
	}
}
