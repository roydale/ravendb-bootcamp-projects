using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System.Reflection;

namespace RavenDB.BootCamp.MultimapIndexes
{
	public static class DocumentStoreHolder
	{
		private static readonly Lazy<IDocumentStore> LazyStore = new(() =>
			{
				var store = new DocumentStore
				{
					Urls = [
						"http://127.0.0.1:8090",
						"http://127.0.0.2:8090",
						"http://127.0.0.3:8090"
					],
					Database = "Northwind"
				};

				store.Initialize();

				// Get all indexes classes and create it
				var assembly = Assembly.GetExecutingAssembly();
				IndexCreation.CreateIndexes(assembly, store);

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
