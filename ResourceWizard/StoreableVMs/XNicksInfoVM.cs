using GameCore;
using GameCore.Storage;
using XTransport;

namespace ResourceWizard.StoreableVMs
{
	class XNicksInfoVM : XObjectVM
	{
		public XNicksInfoVM() { }

#pragma warning disable 649
		[X("Sex")]private IXValue<int> m_sex;
		[X("Nicks")]private IXValue<string> m_nicks;
#pragma warning restore 649

		public override EStoreKind Kind
		{
			get { return EStoreKind.NICKS_INFO; }
		}

		public string Nicks { get { return m_nicks.Value; } set { m_nicks.Value = value; } }

		public ESex Sex { get { return (ESex)m_sex.Value; } set { m_sex.Value = (int)value; } }

		protected override void InstantiationFinished()
		{
			base.InstantiationFinished();
			BindProperty(m_nicks, () => Nicks);
			BindProperty(m_sex, () => Sex);
		}
	}
}