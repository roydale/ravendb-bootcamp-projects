using Raven.Client.Documents.Indexes;
using RavenDB.BootCamp.Core;

namespace RavenDB.BootCamp.MapReduceIndexes
{
	public class Products_ByCategory : AbstractIndexCreationTask<Product, Products_ByCategory.Result>
	{
		public class Result
		{
			public string Category { get; set; }

			public int Count { get; set; }
		}

		public Products_ByCategory()
		{
			Map = (products) =>
				from product in products
				select new
				{
					product.Category,
					Count = 1
				};

			Reduce = (results) =>
				from result in results
				group result by result.Category into g
				select new
				{
					Category = g.Key,
					Count = g.Sum(x => x.Count)
				};
		}
	}
}
