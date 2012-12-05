using System;
using GameCore.AbstractLanguage;
using GameCore.Essences.Things;

namespace GameCore.Essences
{
	/// <summary>
	/// 	Символизирует инвентарь существа.
	/// </summary>
	internal class BackPack : Container, ISpecial
	{
		public BackPack() : base(EALNouns.BackPack, null)
		{
		}

		public override ETileset Tileset { get { throw new NotImplementedException(); } }
	}
}