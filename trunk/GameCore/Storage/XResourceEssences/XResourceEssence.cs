using System;
using GameCore.Essences;
using XTransport;

namespace GameCore.Storage.XResourceEssences
{
	public abstract class XResourceEssence<T> : XObject, IResourceEssence where T : Essence
	{
		public Essence Create(Material _material)
		{
			return CreateT(_material);
		}

		protected abstract T CreateT(Material _material);

		public override EStoreKind Kind
		{
			get { return EStoreKind.ESSENCE_INFO; }
		}

		public abstract Guid ProvierTypeId { get; }

		[X("TileIndex")]
		private IXValue<int> m_tileIndex;
		public int TileIndex { get { return m_tileIndex.Value; } set { m_tileIndex.Value = value; } }

		[X("Name")]
		private IXValue<string> m_name;
		public string Name { get { return m_name.Value; } set { m_name.Value = value; } }


		[X("MaterialTypes")]
		private IXValue<int> m_materials;
		public EMaterialType MaterialTypes { get { return (EMaterialType)m_materials.Value; } set { m_materials.Value = (int)value; } }

		[X("IsArtifact")]
		private IXValue<bool> m_isArtifact;
		public bool IsArtifact { get { return m_isArtifact.Value; } set { m_isArtifact.Value = value; } }

		[X("Lever")]
		private IXValue<int> m_level;
		public int Level { get { return m_level.Value; } set { m_level.Value = value; } }
	}
}