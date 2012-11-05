using XTransport;
using XTransport.Server;
using XTransport.Server.Storage;

namespace GameCore.Storage
{
	class StoreServer : AbstractXServer
	{
		protected override bool IsAsync
		{
			get { return true; }
		}

		protected override IStorage CreateStorage()
		{
			return new SQLiteStorage(Constants.RESOURCES_DB_FILE);
		}
	}
}
