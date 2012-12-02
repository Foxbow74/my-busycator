using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Essences;
using GameCore.Misc;
using GameCore.Storage;
using GameCore.Storage.XResourceEssences;
using MagickSetting.Items.Weapons;

namespace MagickSetting
{
	public class XResourceEssenceGenerator
	{
		static readonly Dictionary<Type, Guid> m_types = new Dictionary<Type, Guid>();

		static XResourceEssenceGenerator()
		{
			foreach (var type in Util.GetAllTypesOf<IResourceEssence>())
			{
				if (typeof (ISpecial).IsAssignableFrom(type)) continue;
				var provider = (IResourceEssence)Activator.CreateInstance(type);
				m_types.Add(type, provider.ProvierTypeId);
			}
		}

		private readonly IXResourceEssenceProvider m_helperGenerator;

		public XResourceEssenceGenerator(IXResourceEssenceProvider _helperGenerator)
		{
			m_helperGenerator = _helperGenerator;
		}

		public IEnumerable<IResourceEssence> Generate()
		{
			yield return Add<XResourceSword>("меч", 1, 1);
			yield return Add<XResourceSword>("меч-леденец", 2, 1);
			yield return Add<XResourceSword>("меч-зарубец", 3, 2);
			yield return Add<XResourceSword>("меч-холодец", 4, 3);
			yield return Add<XResourceSword>("меч-всем-звиздец", 5, 4, EMaterialType.UNIQ, true);
		}

		private T Add<T>(string _name, int _tileIndex, int _level, EMaterialType _materialType = EMaterialType.METAL, bool _isArtifact = false) where T : XObject, IResourceEssence
		{
			var result = m_helperGenerator.CreateXResourceEssence<T>(m_types[typeof(T)]);
			result.Name = _name;
			result.TileIndex = _tileIndex;
			result.IsArtifact = _isArtifact;
			result.Level = _level;
			result.MaterialTypes = _materialType;
			return result;
		}
	}
}
