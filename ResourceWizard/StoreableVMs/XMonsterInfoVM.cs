using GameCore.Storage;

namespace ResourceWizard.StoreableVMs
{
	class XMonsterInfoVM : XObjectVM
	{
		public XMonsterInfoVM()
		{ }

		public override EStoreKind Kind
		{
			get { return EStoreKind.MONSTER_INFO; }
		}

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
		}
	}
}