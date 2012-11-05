using XTransport.Client;

namespace GameCore.Storage
{
	internal abstract class XChildObject<TParent> : ClientXChildObject<EStoreKind, TParent>
		where TParent : XObject
	{
	}
}