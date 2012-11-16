using System;
using GameCore.Creatures;
using GameCore.Essences.Things;

namespace GameCore.Essences
{
	/// <summary>
	/// 	Символизирует инвентарь существа.
	/// </summary>
	internal class BackPack : Container, ISpecial
	{
		public BackPack() : base(null) { }

		public override ETileset Tileset { get { throw new NotImplementedException(); } }

		public override string Name { get { return "рюкзак"; } }

		public override EEssenceCategory Category { get { throw new NotImplementedException(); } }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }
	}
}