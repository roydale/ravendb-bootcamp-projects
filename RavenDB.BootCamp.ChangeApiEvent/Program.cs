using RavenDB.BootCamp.Core;

var subscription = DocumentStoreHolder.Store
	.Changes()
	.ForAllDocuments()
	.Subscribe(change => Console.WriteLine($"{change.Type} on document {change.Id}"));

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

subscription.Dispose();