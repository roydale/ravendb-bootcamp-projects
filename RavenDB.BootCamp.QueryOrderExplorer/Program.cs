using Raven.Client.Documents;
using RavenDB.BootCamp.Core;

namespace RavenDB.BootCamp.QueryOrderExplorer
{
	public class Program
	{
		static void Main()
		{
			while (true)
			{
				Console.WriteLine("Please, enter a company id (0 to exit): ");

				if (!int.TryParse(Console.ReadLine(), out var companyId))
				{
					Console.WriteLine("Company # is invalid.");
					continue;
				}

				if (companyId == 0) break;

				Console.WriteLine("Use RQL? (y/n): ");
				var isRql = Console.ReadLine();

				Console.WriteLine("-------------------------------------------------");
				if (!string.IsNullOrEmpty(isRql) && isRql.ToLower().Equals("y"))
				{
					RqlCompanyOrders(companyId);
				}
				else
				{
					LinqCompanyOrders(companyId);
				}
				Console.WriteLine("-------------------------------------------------");
				Console.WriteLine();
			}

			Console.WriteLine("Goodbye!");
		}

		private static void LinqCompanyOrders(int companyId)
		{
			string companyReference = $"companies/{companyId}-A";

			using var session = DocumentStoreHolder.Store.OpenSession();
			var orders = (
					from order in session.Query<Order>()
										.Include(o => o.Company)
					where order.Company == companyReference
					select order
				).ToList();

			var company = session.Load<Company>(companyReference);

			if (company == null)
			{
				Console.WriteLine("Company not found.");
				return;
			}

			Console.WriteLine($"Using LINQ - from order in session.Query<Order>().Include(o => o.Company) where order.Company == {companyReference} select order");
			Console.WriteLine($"Orders for {company.Name}");

			foreach (var order in orders)
			{
				Console.WriteLine($"{order.Id} - {order.OrderedAt}");
			}
		}

		private static void RqlCompanyOrders(int companyId)
		{
			string companyReference = $"companies/{companyId}-A";

			using var session = DocumentStoreHolder.Store.OpenSession();
			var orders = session.Advanced.RawQuery<Order>(
					@"from Orders
					where Company == $companyId
					include Company"
				).AddParameter("companyId", companyReference)
				.ToList();

			var company = session.Load<Company>(companyReference);

			if (company == null)
			{
				Console.WriteLine("Company not found.");
				return;
			}

			Console.WriteLine($"Using RQL - from Orders where Company == {companyReference} include Company");
			Console.WriteLine();
			Console.WriteLine($"Orders for {company.Name}");

			foreach (var order in orders)
			{
				Console.WriteLine($"{order.Id} - {order.OrderedAt}");
			}
		}
	}
}