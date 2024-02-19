using Newtonsoft.Json;
using Raven.Client.Documents.Session;
using RavenDB.BootCamp.Core;

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	Console.WriteLine("\nRetrieving Order with Company: companies/1-a");
	Console.WriteLine("-------------------------------------------------");

	var orders = (
			from order in session.Query<Order>().Statistics(out QueryStatistics stats)
			where order.Company == "companies/1-a"
			orderby order.OrderedAt
			select order
		).ToList();

	Console.WriteLine($"Index used was: {stats.IndexName}");
	Console.WriteLine($"Is index up to date: {!stats.IsStale}");
	Console.WriteLine($"{JsonConvert.SerializeObject(stats, Formatting.Indented)}");

	Console.WriteLine("\nForcing Non-Stale Results");
	Console.WriteLine("If you need to make sure that your results are up");
	Console.WriteLine("to date, then you can use the Customize method.");
	Console.WriteLine("-------------------------------------------------");

	var query = session.Query<Order>()
		.Customize(q => q.WaitForNonStaleResults(TimeSpan.FromSeconds(5)));

	orders = (
			from order in query
			where order.Company == "companies/1-a"
			orderby order.OrderedAt
			select order
		).ToList();

	Console.WriteLine($"{JsonConvert.SerializeObject(stats, Formatting.Indented)}");
}