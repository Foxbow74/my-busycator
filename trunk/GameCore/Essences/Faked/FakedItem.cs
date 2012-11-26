using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Battle;
using GameCore.Creatures;

namespace GameCore.Essences.Faked
{
	public class FakedItem : Item, IFaked
	{
		#region Fields

		private readonly int m_tileIndex;
		private readonly ETileset m_tileset;
		private readonly List<Type> m_types = new List<Type>();

		#endregion

		#region .ctor

		public FakedItem(ETileset _tileset, Material _material, int _tileIndex) : base(_material)
		{
			m_tileset = _tileset;
			m_tileIndex = _tileIndex;
		}

		#endregion

		#region Methods

		public void Add(Type _type)
		{
			m_types.Add(_type);
		}

		public override ItemBattleInfo CreateItemInfo(Creature _creature)
		{
			throw new NotImplementedException();
		}

		public override bool Is<T>()
		{
			return m_types.Any(_type => typeof (T).IsAssignableFrom(_type));
		}

		public override void Resolve(Creature _creature)
		{
			throw new NotImplementedException();
		}


		protected override int CalcHashCode()
		{
			return (int) m_tileset;
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
			get { return m_tileIndex; }
		}

		public override ETileset Tileset
		{
			get { return m_tileset; }
		}

		public Essence ResolveFake(Creature _creature)
		{
			var type = m_types[World.Rnd.Next(m_types.Count)];
			return EssenceHelper.ResolveEssence(type, Material, _creature);
		}

		#endregion
	}
}