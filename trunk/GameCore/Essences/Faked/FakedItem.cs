using System;
using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences.Faked
{
	public class FakedItem : Item, IFaked
	{
		#region .ctor

		public FakedItem(Essence _essence)
			: base(_essence.Material)
		{
			if (_essence.Material==null)
			{
				
			}
			Essence = _essence;
		}

		#endregion

		#region Methods
		
		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			throw new NotImplementedException();
		}

		public override bool Is<T>()
		{
			return Essence is T;
		}

		protected override int CalcHashCode()
		{
			return ((int)Tileset)<<8|TileIndex;
		}

		public override FColor LerpColor { get { return Essence.LerpColor; } }

		#endregion

		#region Properties

		public override EItemCategory Category
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IFaked Members

		public override int TileIndex
		{
			get { return Essence.TileIndex; }
		}

		public Essence Essence { get; private set; }

		public override ETileset Tileset
		{
			get { return Essence.Tileset; }
		}

		#endregion
	}
}