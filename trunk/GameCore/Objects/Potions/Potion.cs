using System;
using GameCore.Creatures;

namespace GameCore.Objects.Potions
{
	public class Potion:Item
	{
		public override ETiles Tile
		{
			get
			{
				return ETiles.POTION;
			}
		}

		public override string Name
		{
			get { return "булька"; }
		}

		public override void Resolve(Creature _creature)
		{
			
		}

		public override EThingCategory Category
		{
			get { return EThingCategory.POTION; }
		}
	}
}
