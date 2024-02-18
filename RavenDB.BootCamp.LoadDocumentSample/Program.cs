using RavenDB.BootCamp.LoadDocumentSample;

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var query = session
		.Query<Products_ByCategory.Result, Products_ByCategory>();
	//.Include(x => x.Category);

	var results = (
		from result in query
		select result
		).ToList();

	foreach (var result in results)
	{
		//var category = session.Load<Category>(result.Category);
		//Console.WriteLine($"{category.Name} has {result.Count} items.");
		Console.WriteLine($"{result.Category} has {result.Count} items.");
	}

	//http://localhost:8080/databases/Northwind/queries?query=from%20index%20%27Products/ByCategory%27
}