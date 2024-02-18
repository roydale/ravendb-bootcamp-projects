﻿using RavenDB.BootCamp.ContactManager;

Run();

void Run()
{
	while (true)
	{
		Console.WriteLine("Please, press:");
		Console.WriteLine("C - Create");
		Console.WriteLine("R - Retrieve");
		Console.WriteLine("U - Update");
		Console.WriteLine("D - Delete");
		Console.WriteLine("Q - Query all contacts (limit to 128 items)");

		var input = Console.ReadKey();

		Console.WriteLine("\n------------");

		switch (input.Key)
		{
			case ConsoleKey.C:
				CreateContact();
				break;
			case ConsoleKey.R:
				RetrieveContact();
				break;
			case ConsoleKey.U:
				UpdateContact();
				break;
			case ConsoleKey.D:
				DeleteContact();
				break;
			case ConsoleKey.Q:
				QueryAllContacts();
				break;
			default:
				return;
		}

		Console.WriteLine("------------");
	}
}

void CreateContact()
{
	using var session = DocumentStoreHolder.Store.OpenSession();
	Console.WriteLine("Name: ");
	var name = Console.ReadLine();

	Console.WriteLine("Email: ");
	var email = Console.ReadLine();

	var contact = new Contact
	{
		Name = name,
		Email = email
	};

	session.Store(contact);

	Console.WriteLine($"New Contact ID {contact.Id}");

	session.SaveChanges();
}

void RetrieveContact()
{
	Console.WriteLine("Enter the contact id: ");
	var id = Console.ReadLine();
	string contactReference = $"contacts/{id}-A";

	using var session = DocumentStoreHolder.Store.OpenSession();
	var contact = session.Load<Contact>(contactReference);

	if (contact == null)
	{
		Console.WriteLine("Contact not found.");
		return;
	}

	Console.WriteLine($"Name: {contact.Name}");
	Console.WriteLine($"Email: {contact.Email}");
}

void UpdateContact()
{
	Console.WriteLine("Enter the contact id: ");
	var id = Console.ReadLine();
	string contactReference = $"contacts/{id}-A";

	using var session = DocumentStoreHolder.Store.OpenSession();
	var contact = session.Load<Contact>(contactReference);

	if (contact == null)
	{
		Console.WriteLine("Contact not found.");
		return;
	}

	Console.WriteLine($"Actual name: {contact.Name}");
	Console.WriteLine("New name: ");
	contact.Name = Console.ReadLine();

	Console.WriteLine($"Actual email: {contact.Email}");
	Console.WriteLine("New email address: ");
	contact.Email = Console.ReadLine();

	session.SaveChanges();
}

void DeleteContact()
{
	Console.WriteLine("Enter the contact id: ");
	var id = Console.ReadLine();
	string contactReference = $"contacts/{id}-A";

	using var session = DocumentStoreHolder.Store.OpenSession();
	var contact = session.Load<Contact>(contactReference);

	if (contact == null)
	{
		Console.WriteLine("Contact not found.");
		return;
	}

	session.Delete(contact);
	session.SaveChanges();
}

void QueryAllContacts()
{
	using var session = DocumentStoreHolder.Store.OpenSession();
	var contacts = session.Query<Contact>().ToList();

	foreach (var contact in contacts)
	{
		Console.WriteLine($"{contact.Id} - {contact.Name} - {contact.Email}");
	}

	Console.WriteLine($"{contacts.Count} contacts found.");
}