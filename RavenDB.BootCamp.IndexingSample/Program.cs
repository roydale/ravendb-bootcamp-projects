using RavenDB.BootCamp.Core;

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var ordersIds = (
			from order in session.Query<Order>()
			where order.Company == "companies/1-A"
			orderby order.OrderedAt
			select order.Id
		).ToList();

	foreach (var id in ordersIds)
	{
		Console.WriteLine(id);
	}
}