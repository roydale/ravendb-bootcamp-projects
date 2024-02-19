using Newtonsoft.Json;
using RavenDB.BootCamp.Core;

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var revisions = session.Advanced.Revisions
		.GetFor<Employee>("employees/4-A");

	foreach (var revision in revisions)
	{
		Console.WriteLine("-------------------------------------------------");
		Console.WriteLine($"{JsonConvert.SerializeObject(revision, Formatting.Indented)}");
	}
}