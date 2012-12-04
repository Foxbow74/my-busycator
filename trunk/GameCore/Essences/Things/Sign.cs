﻿using GameCore.AbstractLanguage;

namespace GameCore.Essences.Things
{
	/// <summary>
	/// Табличка на здании, на стене
	/// </summary>
	public class Sign : Thing, ISpecial
	{
		private readonly string m_name;
		private readonly ETileset m_tileset;

		public Sign(ETileset _tileset, Material _material, string _name)
			: base("знак".AsNoun(ESex.MALE, false) + _name.AsIm(), _material)
		{
			m_tileset = _tileset;
			m_name = _name;
		}

		public override ETileset Tileset { get { return m_tileset; } }
	}
}