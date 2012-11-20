using GameCore.Storage;
using RusLanguage;
using XTransport;

namespace GameCore.Storeable
{
	class XNicksInfo : XObject
	{
		public XNicksInfo() { }

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
	}
}