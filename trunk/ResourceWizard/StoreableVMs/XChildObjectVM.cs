using GameCore.Storage;
using XTransport.WPF;

namespace ResourceWizard.StoreableVMs
{
	abstract class XChildObjectVM<TParent> : ClientXChildObjectVM<EStoreKind, TParent>
        where TParent : ClientXObjectVM<EStoreKind>, new()
	{
	}
}

