using Newtonsoft.Json;
using Raven.Client.Documents.Session;
using RavenDB.BootCamp.Core;

// Storing a new document
string categoryId;
using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var newCategory = new Category
	{
		Name = "New Category",
		Description = "Description of the new category"
	};

	session.Store(newCategory);
	categoryId = newCategory.Id;
	session.SaveChanges();
}

// Loading and Modifying
using (var session = DocumentStoreHolder.Store.OpenSession())
{
	var storedCategory = session
		.Load<Category>(categoryId);

	Console.WriteLine(JsonConvert.SerializeObject(storedCategory, Formatting.Indented));
	storedCategory.Name = "Updated Category";
	storedCategory.Description = "This is a description of the updated category";

	LogChanges(session);
	session.SaveChanges();

	Console.WriteLine(JsonConvert.SerializeObject(storedCategory, Formatting.Indented));
	LogChanges(session);
}

// Deleting
using (var session = DocumentStoreHolder.Store.OpenSession())
{
	session.Delete(categoryId);
	session.SaveChanges();

	var storedCategory = session
		.Load<Category>(categoryId);

	var categoryJson = JsonConvert.SerializeObject(storedCategory, Formatting.Indented);
	Console.WriteLine(categoryJson == "null" ? "Not found" : categoryJson);
}

static void LogChanges(IDocumentSession session)
{
	var hasChanges = session.Advanced.HasChanges;
	Console.WriteLine($"Was the document updated? {hasChanges}");
	IDictionary<string, DocumentsChanges[]> changes = session.Advanced.WhatChanged();
	foreach (var change in changes)
	{
		foreach (var value in change.Value)
		{
			Console.WriteLine($"   OLD - {value.FieldName}:{value.FieldOldValue}, NEW - {value.FieldName}:{value.FieldNewValue}");
		}
	}
}