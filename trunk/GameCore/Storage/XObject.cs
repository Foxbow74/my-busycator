using System;
using System.Collections.Generic;
using GameCore.Essences;
using GameCore.Misc;
using XTransport;
using XTransport.Client;

namespace GameCore.Storage
{
	public interface IEssenceProviderHelperGenerator
	{
		TO Create<TO>(Guid _typeId) where TO : XObject;
	}

	public abstract class XObject : ClientXObject<EStoreKind>
	{
	}

	class EssenceProviderHelper : ClientXObject<EStoreKind>
	{
		static readonly Dictionary<Guid,Type> m_types = new Dictionary<Guid, Type>();

		static EssenceProviderHelper()
		{
			foreach (var type in Util.GetAllTypesOf<IEssenceProvider>())
			{
				if (typeof (ISpecial).IsAssignableFrom(type)) continue;
				var provider = (IEssenceProvider)Activator.CreateInstance(type);
				m_types.Add(provider.ProvierTypeId, type);
			}
		}
		
#pragma warning disable 649
		[X("ProvierTypeId")] private IXValue<Guid> m_provierTypeId;
#pragma warning restore 649

		public Guid ProvierTypeId { get { return m_provierTypeId.Value; } set { m_provierTypeId.Value = value; }}

		public override EStoreKind Kind
		{
			get { return EStoreKind.ESSENCE_INFO;}
		}

		public IEssenceProvider GetSpecificProvider()
		{
			return (IEssenceProvider)Client.GetByType(m_types[m_provierTypeId.Value], Uid);
		}

		public static XResourceClient Client { get; set; }
	}

	public interface IEssenceProvider
	{
		Essence Create();
		Guid ProvierTypeId { get; }
	}

	public abstract class EssenceProvider<T> : XObject, IEssenceProvider where T : Essence
	{
		public Essence Create()
		{
			return CreateT();
		}

		protected abstract T CreateT();

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


		[X("Materials")]
		private IXValue<int> m_materials;
		public EMaterial Materials { get { return (EMaterial)m_materials.Value; } set { m_materials.Value = (int)value; } }

		[X("IsArtifact")]
		private IXValue<bool> m_isArtifact;
		public bool IsArtifact { get { return m_isArtifact.Value; } set { m_isArtifact.Value = value; } }

		[X("Lever")]
		private IXValue<int> m_level;
		public int Level { get { return m_level.Value; } set { m_level.Value = value; } }
	}

	public abstract class ItemProvider<T> : EssenceProvider<T> where T : Item
	{
	}

	public abstract class WeaponProvider<T> : EssenceProvider<T> where T : Item
	{
	}
}