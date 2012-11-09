using XTransport;
using XTransport.Server;
using XTransport.Server.Storage;

namespace GameCore.Storage
{
	class XResourceServer : AbstractXServer
	{
		protected override bool IsAsync
		{
			get { return false; }
		}

		protected override IStorage CreateStorage()
		{
			return new SQLiteStorage(Constants.RESOURCES_DB_FILE);
		}
	}
}
