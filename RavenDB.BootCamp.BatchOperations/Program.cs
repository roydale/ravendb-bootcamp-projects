using Newtonsoft.Json;
using Raven.Client.Documents.Operations;
using RavenDB.BootCamp.Core;

Console.WriteLine("-------------------------------------------------------------");
Console.WriteLine("                           INITIAL                           ");
LoadProducts();

UpdateProductByBatch();

Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("                          UPDATED                            ");
LoadProducts();

static void LoadProducts()
{
	using var session = DocumentStoreHolder.Store.OpenSession();
	var products = session
		.Advanced.RawQuery<Product>(
			@"from Products as p
            where p.Discontinued = false"
		).ToList();

	foreach (var product in products)
	{
		Console.WriteLine("-------------------------------------------------------------");
		//Console.WriteLine($"{JsonConvert.SerializeObject(product, Formatting.Indented)}");
		Console.WriteLine($"{product.Id} {product.Name} - Price: {product.PricePerUnit}, Discontinued: {product.Discontinued}");
	}
}

static void UpdateProductByBatch()
{
	var operation = DocumentStoreHolder.Store
		.Operations
		.Send(new PatchByQueryOperation(
			@"from Products as p
            where p.Discontinued = false
            update
            {
                p.PricePerUnit = p.PricePerUnit * 1.1
            }"));
	operation.WaitForCompletion();
}
