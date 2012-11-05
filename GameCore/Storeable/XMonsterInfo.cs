using GameCore.Storage;

namespace GameCore.Storeable
{
	class XMonsterInfo : XObject
	{
		public XMonsterInfo()
		{}

		public override EStoreKind Kind
		{
			get { return EStoreKind.MONSTER_INFO; }
		}
	}
}
