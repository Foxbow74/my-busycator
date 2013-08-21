using GameCore.AbstractLanguage;
using GameCore.Creatures;
using GameCore.Essences;

namespace GameCore.Battle
{
	public class Corpse:Item, ISpecial
	{
		readonly EDirections m_direction;
		private readonly Creature m_creature;

		public Corpse(Creature _creature)
			: base(EALNouns.Corpse, _creature.Material)
		{
			m_direction = Util.AllDirections.RandomItem(World.Rnd);
			m_creature = _creature;
		}

		protected override Noun GetUpdatedName(Noun _noun)
		{
			if (m_creature == null)
			{
				return _noun;
			}
			return _noun + m_creature.Name.AsOf();
		}

		public override EDirections Direction
		{
			get
			{
				return m_direction;
			}
		}

		public override ETileset Tileset
		{
			get
			{
				return m_creature.Tileset;
			}
		}

		public override EItemCategory Category
		{
			get { return EItemCategory.FOOD; }
		}

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			throw new System.NotImplementedException();
		}

		public override int TileIndex
		{
			get
			{
				return m_creature.TileIndex;
			}
		}

		public override bool IsCorpse
		{
			get
			{
				return true;
			}
		}

		public override FColor LerpColor
		{
			get
			{
				return m_creature.LerpColor.Lerp(FColor.Crimson, 0.2f);
			}
			protected set
			{
				base.LerpColor = value;
			}
		}
	}
}