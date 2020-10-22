# Background Jobs in Hangfire.

This demo shows how to enable Hangfire in a .NET Core Application with a SQL Server Backend.

## The Context
SlowApp is a web application with one controller. The controller has one action that returns a list of generic items, retrieved from a database. The Controller uses `ItemProvider` as a dependency, and further down the line there is `ItemLogger` a sumb component that for the purposes of this demonstration, does nothing but waste time.

`ItemLogger` is a placeholder for components that in real-life would execute long-running procesess or slow remote calls. By invoking this component syunchronously with every `GET /items` request, we add to the response time anything between 1 and 8 seconds. There are many ways to take the work done by `ItemLogger` and invoke it asynchrnourly. In many other circumstances this would be a job delegated to a message broker, like Azure Service Bus or RabbitMQ.

However, when there is no necessity for complex routing, or broadcasting, Hangfire is a worthy alternative that can get the job done with minimal disruption, provided a few design decisions are taken into consideration:
- Either the work done by components analogous to `ItemLogger` are transient in relation to the state of the enclosing context, or a compensatory status update action is performed in the background job.
- Every parameter taken by every method intended to be invoked asynchronously must be serializable. In the case of this example, EF Core Entities are used, but this would not work if there are any circular dependencies due to navigation properties.
- All the parameters and the component analogous to `ItemLogger` (included in all the methods intended to be invoked in the background) must be contained in its own assembly, if the background client is to be run in a separate process than the server.

## Adding the Hangfire Client
In the `Startup.cs` file, add to the `ConfigureServices` method:

```csharp
services.AddHangfire(configuration =>
    configuration
		.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
		{
			SchemaName = "Hangfire"
		}
	)
);
```

For this example, there is an existing database connection for the generic `Item`s entity, but Hangfire can also be connected to a separate database. To distinguish the tables used by Hangfire from the ones used otherwise for the application domain, it is a good idea to specify an ANSI schema name as a prefix fot the Hangfire managed tables.

Next, in `Configure` method of the same file:
```csharp
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
	DashboardTitle = "Slow App Background Jobs Dashboard"
});
```

## Modifying existing components to use Hangfire
The main component to modify is the `ItemsProvider` class. Instead of injecting an `ItemLogger` instance, we'll replace this with a Hangfire provided class, `IBackgroundJobClient`.
```csharp
public class ItemsProvider
{
	private readonly SlowAppDbContext _db;
	private readonly IBackgroundJobClient _hangfire;

	public ItemsProvider(SlowAppDbContext db, IBackgroundJobClient hangfire)
	{
		_db = db;
		_hangfire = hangfire;
	}

	public virtual async Task<List<Item>> GetItemsAsync()
	{
		var availableItems = await (from i in _db.Items select i).ToListAsync();
		_hangfire.Enqueue<ItemsLogger>(logger => logger.ProcessItemsAsync(availableItems));

		return availableItems;
	}
}
```

Hangfire automatically registers an implementation in the `ServiceCollection` of the ASP.NET App, that is available to every other class configured with the Microsoft Dependency Injection Extensions.
At this point, launching the application and calling the `/items` endpoint

## Enabling server in the same process as the ASP.NET App

Also in the `Configure` method of `Startup.cs`:
```csharp
app.UseHangfireServer();
```

## Enabling the server in a different process (in this example, a Console App)
Create a new console App, add a reference to the `SlowApp.Core` library, add the Hangfire SQL package and modify Programs.cs to look like this:
```csharp
var connectionString = "Server=(local);Database=SlowAppDb;Trusted_Connection=True;";

GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);

using var server = new BackgroundJobServer();

Console.WriteLine("Hangfire Server Started.");
Console.ReadLine(); // This is a blocking call and will prevent the process from terminating.
```