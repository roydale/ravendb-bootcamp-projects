using RavenDB.BootCamp.Core;

LinqCompanies();
RqlCompanies();
LinqEmployees();
RqlEmployees();
LinqEmployeesFunction();
RqlEmployeesFunction();

static void LinqCompanies()
{
	Console.WriteLine("\nLINQ Projection - Top 10 Companies");
	Console.WriteLine("-------------------------------------------------");

	using var session = DocumentStoreHolder.Store.OpenSession();
	// Request Name, City and Country for all entities from 'Companies' collection
	var results = session
		.Query<Company>()
		.Take(10)
		.Select(x => new
		{
			x.Name,
			x.Address.City,
			x.Address.Country
		})
		.ToList();

	foreach (var r in results)
	{
		Console.WriteLine($"{r.Name} - {r.City}, {r.Country}");
	}
}

static void RqlCompanies()
{
	Console.WriteLine("\nRQL Projection - Top 10 Companies");
	Console.WriteLine("-------------------------------------------------");

	using var session = DocumentStoreHolder.Store.OpenSession();
	var results = session
		.Advanced.RawQuery<dynamic>(
			@"from Companies
			select Name, Address.City as City, Address.Country as Country
			limit 10"
		).ToList();

	foreach (var r in results)
	{
		Console.WriteLine($"{r.Name} - {r.City}, {r.Country}");
	}
}

static void LinqEmployees()
{
	Console.WriteLine("\nLINQ Projection - Employees");
	Console.WriteLine("-------------------------------------------------");

	using var session = DocumentStoreHolder.Store.OpenSession();
	var results = (from e in session.Query<Employee>()
				   select new
				   {
					   FullName = e.FirstName + " " + e.LastName,
				   }).ToList();

	foreach (var r in results)
	{
		Console.WriteLine($"{r.FullName}");
	}
}

static void RqlEmployees()
{
	Console.WriteLine("\nRQL Projection - Employees");
	Console.WriteLine("-------------------------------------------------");

	using var session = DocumentStoreHolder.Store.OpenSession();
	var results = session
		.Advanced.RawQuery<dynamic>(
			@"from Employees as e
			select { FullName : e.FirstName + "" "" + e.LastName }"
		).ToList();

	foreach (var r in results)
	{
		Console.WriteLine($"{r.FullName}");
	}
}

static void LinqEmployeesFunction()
{
	Console.WriteLine("\nLINQ Projection - FullName Function for Employees");
	Console.WriteLine("-------------------------------------------------");

	using var session = DocumentStoreHolder.Store.OpenSession();
	var results = (from e in session.Query<Employee>()
				   let format =
					   (Func<Employee, string>)(p =>
						   p.FirstName + " " + p.LastName)
				   select new
				   {
					   FullName = format(e)
				   }).ToList();

	foreach (var r in results)
	{
		Console.WriteLine($"{r.FullName}");
	}
}

static void RqlEmployeesFunction()
{
	Console.WriteLine("\nRQL Projection - FullName Function for Employees");
	Console.WriteLine("-------------------------------------------------");

	using var session = DocumentStoreHolder.Store.OpenSession();
	var results = session
		.Advanced.RawQuery<dynamic>(
			@"declare function getFullName(e) {
				var format = function(p) {
					return p.FirstName + "" "" + p.LastName;
				};
				return { FullName: format(e) };
			}
			from Employees as e select getFullName(e)"
		).ToList();

	foreach (var r in results)
	{
		Console.WriteLine($"{r.FullName}");
	}
}