using RavenDB.BootCamp.Core;

var subscriptionWorker = DocumentStoreHolder.Store
	.Subscriptions
	.GetSubscriptionWorker<Order>("BigOrders");

var subscriptionRuntimeTask = subscriptionWorker
	.Run(batch =>
		{
			foreach (var order in batch.Items)
			{
				// Business Logic here
				// or do some Data Transformation Logic
				Console.WriteLine(order.Id);
			}
		}
	);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();