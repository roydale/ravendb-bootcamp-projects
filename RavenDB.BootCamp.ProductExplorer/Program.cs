using Newtonsoft.Json;
using RavenDB.BootCamp.Core;

//var documentStore = new DocumentStore
//{
//	Urls = ["http://localhost:8090"],
//	Database = "Northwind"
//};
//documentStore.Initialize();

//using (var session = documentStore.OpenSession())
//{
//	var p = session.Load<Product>("products/1-A");
//	Console.WriteLine(JsonConvert.SerializeObject(p, Formatting.Indented));
//}

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	Console.WriteLine("--------------------------------------------------------------------");
	Console.WriteLine("Product & Category Details: products/1-A");
	Console.WriteLine("--------------------------------------------------------------------");

	var product = session
		.Include<Product>(p => p.Category)
		.Load<Product>("products/1-A");
	var category = session.Load<Category>(product.Category);

	Console.WriteLine(JsonConvert.SerializeObject(product, Formatting.Indented));
	Console.WriteLine(JsonConvert.SerializeObject(category, Formatting.Indented));

	Console.WriteLine("--------------------------------------------------------------------");
	Console.WriteLine("Product & Category Details: products/2-A, products/3-A, products/4-A");
	Console.WriteLine("--------------------------------------------------------------------");

	var products = session
		.Include<Product>(p => p.Category)
		.Load<Product>(new[] { "products/2-A", "products/3-A", "products/4-A" });

	Console.WriteLine(JsonConvert.SerializeObject(products, Formatting.Indented));
	foreach (var p in products)
	{
		var c = session.Load<Category>(p.Value.Category);
		Console.WriteLine(JsonConvert.SerializeObject(c, Formatting.Indented));
	}

	Console.WriteLine("--------------------------------------------------------------------");
	Console.WriteLine("Order: orders/1-A");
	Console.WriteLine("--------------------------------------------------------------------");

	var order = session
		.Include<Order>(x => x.Company)
		.Include(x => x.Employee)
		.Include(x => x.Lines.Select(l => l.Product))
		.Load("orders/1-A");
	Console.WriteLine(JsonConvert.SerializeObject(order, Formatting.Indented));
}