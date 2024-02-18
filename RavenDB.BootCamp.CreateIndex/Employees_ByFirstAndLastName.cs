using Raven.Client.Documents.Indexes;
using RavenDB.BootCamp.Core;

namespace RavenDB.BootCamp.CreateIndex
{
	public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
	{
		public Employees_ByFirstAndLastName()
		{
			Map = (employees) =>
				from employee in employees
				select new
				{
					employee.FirstName,
					employee.LastName
				};
		}
	}
}
