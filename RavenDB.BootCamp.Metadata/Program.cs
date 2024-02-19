using Newtonsoft.Json;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Conventions;
using RavenDB.BootCamp.Core;
using Sparrow.Json;

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var product = session.Load<Product>("products/1-A");
	var metadata = session.Advanced.GetMetadataFor(product);

	foreach (var info in metadata)
	{
		Console.WriteLine($"{info.Key}: {info.Value}");
	}
}

Console.WriteLine("\n-------------------------------------------------");
// Adding a property to the metadata
using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var product = session.Load<Product>("products/1-A");
	var metadata = session.Advanced.GetMetadataFor(product);

	metadata["last-modified-by"] = "Roy Dale Caliwag";
	session.SaveChanges();
}

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var conventions = new DocumentConventions();
	var command = new GetDocumentsCommand(conventions, "products/1-A", null, metadataOnly: true);
	session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);

	var result = (BlittableJsonReaderObject)command.Result.Results[0];
	var metadata = (BlittableJsonReaderObject)result["@metadata"];

	foreach (var propertyName in metadata.GetPropertyNames())
	{
		metadata.TryGet<object>(propertyName, out var value);
		Console.WriteLine($"{propertyName}: {value}");
		//Console.WriteLine($"{JsonConvert.SerializeObject(metadata, Formatting.Indented)}");
	}
}