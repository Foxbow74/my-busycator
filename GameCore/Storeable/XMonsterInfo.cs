using GameCore.Storage;

namespace GameCore.Storeable
{
	public class XMonsterInfo : XObject
	{
		public override EStoreKind Kind
		{
			get { return EStoreKind.MONSTER_INFO; }
		}
	}
}
