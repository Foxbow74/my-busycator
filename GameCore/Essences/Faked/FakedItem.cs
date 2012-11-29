using System;
using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences.Faked
{
	public class FakedItem : Item, IFaked
	{
		private readonly Essence m_essence;

		#region .ctor

		public FakedItem(Essence _essence)
			: base(_essence.Material)
		{
			m_essence = _essence;
		}

		#endregion

		#region Methods
		
		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			throw new NotImplementedException();
		}

		public override bool Is<T>()
		{
			return m_essence is T;
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}


		protected override int CalcHashCode()
		{
			return TileIndex;
		}

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
			get { return m_essence.TileIndex; }
		}

		public override ETileset Tileset
		{
			get { return m_essence.Tileset; }
		}

		public Essence ResolveFake(Creature _creature)
		{
			return EssenceHelper.ResolveEssence(m_essence.GetType(), Material, _creature);
		}

		#endregion
	}
}