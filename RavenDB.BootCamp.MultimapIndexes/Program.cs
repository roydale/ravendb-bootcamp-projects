
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using MultimapIndexes = RavenDB.BootCamp.MultimapIndexes;

Console.Title = "Multi-map sample";
using (var session = MultimapIndexes.DocumentStoreHolder.Store.OpenSession())
{
	while (true)
	{
		Console.Write("\nSearch terms: ");
		var searchTerms = Console.ReadLine();

		foreach (var result in Search(session, searchTerms))
		{
			Console.WriteLine($"{result.SourceId}\t{result.Type}\t{result.Name}");
		}
	}
}

static IEnumerable<MultimapIndexes.People_Search.Result> Search(IDocumentSession session, string searchTerms)
{
	var results = session.Query<MultimapIndexes.People_Search.Result, MultimapIndexes.People_Search>()
		.Search(
			r => r.Name,
			searchTerms
		)
		.ProjectInto<MultimapIndexes.People_Search.Result>()
		.ToList();

	return results;
}