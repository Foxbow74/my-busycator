using System;
using GameCore.Creatures;
using GameCore.Objects.Furnitures;

namespace GameCore.Objects
{
	/// <summary>
	/// 	Символизирует инвентарь существа.
	/// </summary>
	internal class BackPack : Container, ISpecial
	{
		public BackPack() : base(null) { }

		public override ETileset Tileset { get { throw new NotImplementedException(); } }

		public override string Name { get { return "рюкзак"; } }

		public override EThingCategory Category { get { throw new NotImplementedException(); } }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }
	}
}