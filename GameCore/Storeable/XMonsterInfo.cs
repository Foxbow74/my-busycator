using GameCore.Storage;

namespace GameCore.Storeable
{
	class XMonsterInfo : XObject
	{
		public override EStoreKind Kind
		{
			get { return EStoreKind.MONSTER_INFO; }
		}
	}
}
