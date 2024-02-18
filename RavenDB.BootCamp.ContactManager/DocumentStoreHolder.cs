using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace RavenDB.BootCamp.ContactManager
{
	public static class DocumentStoreHolder
	{
		private static readonly Lazy<IDocumentStore> LazyStore = new(() =>
			{
				var store = new DocumentStore { Urls = ["http://localhost:8090"], Database = "ContactsManager" };

				store.Initialize();

				// Try to retrieve a record of this database
				var databaseRecord = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));

				if (databaseRecord != null)
					return store;

				var createDatabaseOperation = new CreateDatabaseOperation(new DatabaseRecord(store.Database));

				store.Maintenance.Server.Send(createDatabaseOperation);

				return store;
			});

		public static IDocumentStore Store => LazyStore.Value;
	}
}