﻿using GameCore;
using GameCore.Storage;
using XTransport;

namespace Busycator.Storage
{
    class XNicksInfo : XObject
    {
#pragma warning disable 649
        [X("Sex")]
        private IXValue<int> m_sex;
        [X("Nicks")]
        private IXValue<string> m_nicks;
#pragma warning restore 649

        public override EStoreKind Kind
        {
            get { return EStoreKind.NICKS_INFO; }
        }

        public string Nicks { get { return m_nicks.Value; } set { m_nicks.Value = value; } }

        public ESex Sex { get { return (ESex)m_sex.Value; } set { m_sex.Value = (int)value; } }
    }
}