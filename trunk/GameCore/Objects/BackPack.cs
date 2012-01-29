using System;
using GameCore.Creatures;

namespace GameCore.Objects
{
	/// <summary>
	/// Символизирует инвентарь существа.
	/// </summary>
	public class BackPack : Container, ISpecial
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
