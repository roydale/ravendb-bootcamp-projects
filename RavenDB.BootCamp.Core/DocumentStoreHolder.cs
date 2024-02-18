using Raven.Client.Documents;

namespace RavenDB.BootCamp.Core
{
	public static class DocumentStoreHolder
	{
		private static readonly Lazy<IDocumentStore> LazyStore = new(() =>
			{
				var store = new DocumentStore { Urls = ["http://localhost:8090"], Database = "Northwind" };

				return store.Initialize();
			});

		public static IDocumentStore Store => LazyStore.Value;
	}
}