using System;

namespace GameCore.Storage.XResourceEssences
{
	public interface IXResourceEssenceProvider
	{
		TO CreateXResourceEssence<TO>(Guid _typeId) where TO : XObject;
	}
}