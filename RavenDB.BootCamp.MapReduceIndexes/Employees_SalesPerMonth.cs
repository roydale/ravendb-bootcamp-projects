using Raven.Client.Documents.Indexes;
using RavenDB.BootCamp.Core;

namespace RavenDB.BootCamp.MapReduceIndexes
{
	public class Employees_SalesPerMonth : AbstractIndexCreationTask<Order, Employees_SalesPerMonth.Result>
	{
		public class Result
		{
			public string Employee { get; set; }
			public string Month { get; set; }
			public int TotalSales { get; set; }
		}

		public Employees_SalesPerMonth()
		{
			Map = orders =>
				from order in orders
				select new
				{
					order.Employee,
					Month = order.OrderedAt.ToString("yyyy-MM"),
					TotalSales = 1
				};

			Reduce = results =>
				from result in results
				group result by new
				{
					result.Employee,
					result.Month
				}
				into g
				select new
				{
					g.Key.Employee,
					g.Key.Month,
					TotalSales = g.Sum(x => x.TotalSales)
				};
		}
	}
}
