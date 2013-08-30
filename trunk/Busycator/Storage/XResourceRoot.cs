using System.Collections.Generic;
using GameCore.Storage;
using XTransport;

namespace Busycator.Storage
{
    internal class XResourceRoot : XObject
    {
#pragma warning disable 649
        [X((int)EStoreKind.NICKS_INFO)]
        private ICollection<XNicksInfo> m_nicksInfos;
#pragma warning restore 649

        public override EStoreKind Kind
        {
            get { return EStoreKind.ALL; }
        }

        public ICollection<XNicksInfo> NickInfos
        {
            get { return m_nicksInfos; }
        }
    }
}