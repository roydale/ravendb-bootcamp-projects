
using RavenDB.BootCamp.WriteAssurance;
using Core = RavenDB.BootCamp.Core;

using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var newEmployee = new Core.Employee
	{
		FirstName = "Linda",
		LastName = "Luna",
		Title = "Software Developer"
	};
	session.Store(newEmployee);
	// Confirm that write until it has been replicated at least once.
	session.Advanced.WaitForReplicationAfterSaveChanges(replicas: 1);
	session.SaveChanges();
}