using System;
using System.Collections.Generic;
using GameCore.Essences;
using GameCore.Misc;
using XTransport;
using XTransport.Client;

namespace GameCore.Storage.XResourceEssences
{
	class XResourceEssenceDummy : ClientXObject<EStoreKind>
	{
		static readonly Dictionary<Guid,Type> m_types = new Dictionary<Guid, Type>();

		static XResourceEssenceDummy()
		{
			foreach (var type in Util.GetAllTypesOf<IResourceEssence>())
			{
				if (typeof (ISpecial).IsAssignableFrom(type)) continue;
				var provider = (IResourceEssence)Activator.CreateInstance(type);
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

		public IResourceEssence GetResourceEssence()
		{
			return (IResourceEssence)Client.GetByType(m_types[m_provierTypeId.Value], Uid);
		}

		public static XResourceClient Client { get; set; }
	}
}