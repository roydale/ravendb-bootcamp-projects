using Raven.Client.Documents;
using RavenDB.BootCamp.Core;
using RavenDB.BootCamp.MapReduceIndexes;
using MapReduceIndexes = RavenDB.BootCamp.MapReduceIndexes;

using (var session = MapReduceIndexes.DocumentStoreHolder.Store.OpenSession())
{
	var results = session
		.Query<MapReduceIndexes.Products_ByCategory.Result, MapReduceIndexes.Products_ByCategory>()
		.Include(x => x.Category)
		.ToList();

	foreach (var result in results)
	{
		var category = session.Load<Category>(result.Category);
		Console.WriteLine($"{category.Name} has {result.Count} items.");
	}

	//http://localhost:8080/databases/Northwind/queries?query=from%20index%20%27Products/ByCategory%27%20include%20Category
}

Console.WriteLine();

using (var session = MapReduceIndexes.DocumentStoreHolder.Store.OpenSession())
{
	var query = session
		.Query<Employees_SalesPerMonth.Result, Employees_SalesPerMonth>()
		.Include(x => x.Employee);

	var results = (
		from result in query
		where result.Month == "1998-03"
		orderby result.TotalSales descending
		select result
		).ToList();

	foreach (var result in results)
	{
		var employee = session.Load<Employee>(result.Employee);
		Console.WriteLine(
			$"{employee.FirstName} {employee.LastName}"
			+ $" made {result.TotalSales} sales.");
	}
}