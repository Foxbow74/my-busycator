using System;
using GameCore.Creatures;

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

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}
	}
}
