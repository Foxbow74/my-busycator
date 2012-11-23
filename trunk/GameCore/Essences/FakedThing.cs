using System;
using GameCore.Creatures;

namespace GameCore.Essences
{
	public class FakedThing : Thing, IFaked
	{
		private readonly Essence m_essence;

		public FakedThing(Essence _essence) : base(_essence.Material)
		{
			m_essence = _essence;
		}

		public override FColor LerpColor{ get{ return m_essence.LerpColor; } }

		public override string Name { get { return m_essence.Name; } }

		public override EMaterial AllowedMaterials { get { return m_essence.AllowedMaterials; } }

		#region IFaked Members

		public override int TileIndex
		{
			get
			{
				return m_essence.TileIndex;
			}
		}

		public override ETileset Tileset
		{
			get { return m_essence.Tileset; }
		}

		public Essence ResolveFake(Creature _creature)
		{
			var type = m_essence.GetType();
			return EssenceHelper.ResolveEssence(type, Material, _creature);
		}

		#endregion

		public override bool Is<T>() { return m_essence is T; }

		protected override int CalcHashCode() { return m_essence.GetHashCode(); }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }
	}
}