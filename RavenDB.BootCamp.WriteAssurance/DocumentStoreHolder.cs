using Raven.Client.Documents;

namespace RavenDB.BootCamp.WriteAssurance
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

				return store.Initialize();
			});

		public static IDocumentStore Store => LazyStore.Value;
	}
}