using Api.Context;
using Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests;

public abstract class BaseControllerTest : IAsyncLifetime
{

	protected WebApplicationFactory<Program> Factory { get; private set; }
	protected HttpClient Client { get; private set; }
	protected Guid TestEventId { get; private set; }
	protected Guid TestStandId { get; private set; }
	protected Guid TestUserId { get; private set; }

	public async Task InitializeAsync()
	{
		TestEventId = Guid.NewGuid();
		TestStandId = Guid.NewGuid();
		TestUserId = Guid.NewGuid();

		Factory = new WebApplicationFactory<Program>()
				.WithWebHostBuilder(builder =>
				{
					builder.ConfigureServices(services =>
							{
								// Remove the existing DbContext registration
								var descriptor = services.SingleOrDefault(
												d => d.ServiceType == typeof(DbContextOptions<ApiContext>));
								if (descriptor != null)
									services.Remove(descriptor);

								// Add in-memory database for tests
								services.AddDbContext<ApiContext>(options =>
												options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));
							});
				});

		Client = Factory.CreateClient();

		// Seed test data
		using (var scope = Factory.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
			await context.Database.EnsureCreatedAsync();
		}
	}

	public async Task DisposeAsync()
	{
		using (var scope = Factory.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
			await context.Database.EnsureDeletedAsync();
		}
		await Factory.DisposeAsync();
		Client.Dispose();
	}

	protected async Task<Event> SeedEventAsync(Guid? eventId = null)
	{
		using (var scope = Factory.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
			var id = eventId ?? TestEventId;
			var @event = new Event
			{
				Id = id,
				Name = "Test Event",
				ImageUrl = "https://placehold.co/200"
			};
			context.Events.Add(@event);
			await context.SaveChangesAsync();
			return @event;
		}
	}

	protected async Task<Stand> SeedStandAsync(Guid? eventId = null, Guid? standId = null)
	{
		using (var scope = Factory.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
			var testEventId = eventId ?? TestEventId;
			var @event = await context.Events.FindAsync(testEventId);

			if (@event == null)
			{
				@event = new Event
				{
					Id = testEventId,
					Name = "Test Event",
					ImageUrl = "https://placehold.co/200"
				};
				context.Events.Add(@event);
				await context.SaveChangesAsync();
			}

			var id = standId ?? TestStandId;
			var stand = new Stand
			{
				Id = id,
				Name = "Test Stand",
				ImageUrl = "https://placehold.co/200",
				EventId = testEventId,
				Event = @event
			};
			context.Stands.Add(stand);
			await context.SaveChangesAsync();
			return stand;
		}
	}

	protected async Task<Product> SeedProductAsync(Guid? standId = null, string name = "Test Product")
	{
		using (var scope = Factory.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
			var testStandId = standId ?? TestStandId;
			var stand = await context.Stands
					.Include(s => s.Event)
					.FirstOrDefaultAsync(s => s.Id == testStandId);

			if (stand == null)
			{
				var @event = new Event
				{
					Id = TestEventId,
					Name = "Test Event",
					ImageUrl = "https://placehold.co/200"
				};
				context.Events.Add(@event);
				await context.SaveChangesAsync();

				stand = new Stand
				{
					Id = testStandId,
					Name = "Test Stand",
					ImageUrl = "https://placehold.co/200",
					EventId = TestEventId,
					Event = @event
				};
				context.Stands.Add(stand);
				await context.SaveChangesAsync();
			}

			var product = new Product
			{
				Id = Guid.NewGuid(),
				Name = name,
				Description = "Test product description",
				ImageUrl = "https://placehold.co/200",
				StandId = testStandId,
				Stand = stand,
				Price = 100.00m,
				IsAvailable = true
			};
			context.Products.Add(product);
			await context.SaveChangesAsync();
			return product;
		}
	}

	protected async Task<User> SeedUserAsync(Guid? userId = null)
	{
		using (var scope = Factory.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
			var id = userId ?? TestUserId;
			var user = new User
			{
				Id = id,
				Auth0Id = $"auth0|test-{id}"
			};
			context.Users.Add(user);
			await context.SaveChangesAsync();
			return user;
		}
	}

	protected async Task<Cart> SeedCartAsync(Guid? standId = null, Guid? userId = null)
	{
		using (var scope = Factory.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
			var testStandId = standId ?? TestStandId;
			var testUserId = userId ?? TestUserId;

			var stand = await context.Stands.FindAsync(testStandId);
			if (stand == null)
			{
				stand = await SeedStandAsync(standId: testStandId);
			}

			var user = await context.Users.FindAsync(testUserId);
			if (user == null)
			{
				user = await SeedUserAsync(testUserId);
			}

			var cart = new Cart
			{
				Id = Guid.NewGuid(),
				UserId = testUserId.ToString(),
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				IsCompleted = false,
				StandId = testStandId,
				Stand = stand
			};
			context.Carts.Add(cart);
			await context.SaveChangesAsync();
			return cart;
		}
	}

	protected async Task<Notification> SeedNotificationAsync(Guid? eventId = null, Guid? userId = null)
	{
		using (var scope = Factory.Services.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
			var testEventId = eventId ?? TestEventId;
			var testUserId = userId ?? TestUserId;

			var @event = await context.Events.FindAsync(testEventId);
			if (@event == null)
			{
				@event = await SeedEventAsync(testEventId);
			}

			var user = await context.Users.FindAsync(testUserId);
			if (user == null)
			{
				user = await SeedUserAsync(testUserId);
			}

			var notification = new Notification
			{
				Id = Guid.NewGuid(),
				EventId = testEventId,
				Event = @event,
				UserId = testUserId,
				User = user,
				Title = "Test Notification",
				Message = "Test message"
			};
			context.Notifications.Add(notification);
			await context.SaveChangesAsync();
			return notification;
		}
	}
}
