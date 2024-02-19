using Newtonsoft.Json;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Operations;
using RavenDB.BootCamp.Core;


while (true)
{
	Console.WriteLine("Please, enter a order id (0 to exit): ");
	if (!int.TryParse(Console.ReadLine(), out var orderId))
	{
		Console.WriteLine("Company # is invalid.");
		continue;
	}

	if (orderId == 0) break;

	var orderNumber = $"orders/{orderId}-A";

	Console.WriteLine("-------------------------- INITIAL --------------------------");
	LoadOrderDocument(orderNumber);

	UntypedPatchCommand(orderNumber);

	TypedPatchCommand(orderNumber);

	Console.WriteLine("-------------------------- UPDATED --------------------------");
	LoadOrderDocument(orderNumber);
}

static void LoadOrderDocument(string orderNumber)
{
	using var session = DocumentStoreHolder.Store.OpenSession();
	var order = session.Load<Order>(orderNumber);
	Console.WriteLine(JsonConvert.SerializeObject(order, Formatting.Indented));
}

static void UntypedPatchCommand(string orderNumber)
{
	using var session = DocumentStoreHolder.Store.OpenSession();
	var patchRequest = new PatchRequest
	{
		Script = "this.Lines.push(args.NewLine)",
		Values =
		{
			{
				"NewLine"
				, new { Product = "products/1-a", ProductName = "Chai", PricePerUnit = 18M, Quantity = 1, Discount = 0 }
			}
		}
	};
	session.Advanced.Defer(
		new PatchCommandData(id: orderNumber, changeVector: null, patch: patchRequest, patchIfMissing: null));

	session.SaveChanges();
}

static void TypedPatchCommand(string orderNumber)
{
	using var session = DocumentStoreHolder.Store.OpenSession();
	session.Advanced
		.Patch<Order, OrderLine>(
			orderNumber,
			x => x.Lines,
			lines => lines.Add(
				new OrderLine
				{
					Product = "products/2-a",
					ProductName = "Chang",
					PricePerUnit = 19M,
					Quantity = 1,
					Discount = 0
				}
				)
			);

	session.SaveChanges();
}