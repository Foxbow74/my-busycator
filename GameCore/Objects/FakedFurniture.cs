using System;
using GameCore.Creatures;

namespace GameCore.Objects
{
	public class FakedFurniture : FurnitureThing, IFaked
	{
		private readonly Thing m_thing;

		public FakedFurniture(Thing _thing) : base(_thing.Material)
		{
			m_thing = _thing;
		}

		public override FColor LerpColor{ get{ return m_thing.LerpColor; } }

		public override ETiles Tile
		{
			get { return m_thing.Tile; }
		}

		public override string Name { get { return m_thing.Name; } }

		public override EThingCategory Category { get { return m_thing.Category; } }
		public override EMaterial AllowedMaterials { get { return m_thing.AllowedMaterials; } }

		#region IFaked Members

		public Thing ResolveFake(Creature _creature)
		{
			var type = m_thing.GetType();
			return ThingHelper.ResolveThing(type, Material, _creature);
		}

		#endregion

		public override bool Is<T>() { return m_thing is T; }

		protected override int CalcHashCode() { return m_thing.GetHashCode(); }

		public override void Resolve(Creature _creature) { throw new NotImplementedException(); }
	}
}