using RavenDB.BootCamp.Core;
using RavenDB.BootCamp.CreateIndex;

//var store = DocumentStoreHolder.Store;
//new Employees_ByFirstAndLastName().Execute(store);

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var results = session
		.Query<Employee, Employees_ByFirstAndLastName>()
		.Where(x => x.FirstName == "Robert")
		.ToList();

	foreach (var employee in results)
	{
		Console.WriteLine($"{employee.LastName}, {employee.FirstName}");
	}
}