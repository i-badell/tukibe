using Api.Context;
using Api.Controllers;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Tests;

public abstract class BaseControllerTest : IAsyncLifetime
{
	protected ApiContext Context { get; private set; }
	protected CartController Controller { get; private set; }
	protected Guid TestEventId { get; private set; }
	protected Guid TestStandId { get; private set; }
	protected Guid TestUserId { get; private set; }

	public async Task InitializeAsync()
	{
		TestEventId = Guid.NewGuid();
		TestStandId = Guid.NewGuid();
		TestUserId = Guid.NewGuid();

		var options = new DbContextOptionsBuilder<ApiContext>()
			.UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
			.Options;

		Context = new ApiContext(options);
		await Context.Database.EnsureCreatedAsync();
		Controller = new CartController(null, Context);

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, "123"),
			new(ClaimTypes.Name, "Roberto"),
			new("user-id", TestUserId.ToString())
		};
		var identity = new ClaimsIdentity(claims, "TestAuth");
		var user = new ClaimsPrincipal(identity);
		Controller.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext
			{
				User = user
			}
		};

	}

	public async Task DisposeAsync()
	{
		await Context.Database.EnsureDeletedAsync();
		await Context.DisposeAsync();
	}

	protected ClaimsPrincipal CreateTestUser(string userId = null)
	{
		var claims = new[]
		{
			new Claim("user_id", userId ?? TestUserId.ToString()),
			new Claim(ClaimTypes.NameIdentifier, userId ?? TestUserId.ToString())
		};
		var identity = new ClaimsIdentity(claims, "Test");
		return new ClaimsPrincipal(identity);
	}

	protected async Task<Event> SeedEventAsync(Guid? eventId = null)
	{
		var id = eventId ?? TestEventId;
		var @event = new Event
		{
			Id = id,
			Name = "Test Event",
			ImageUrl = "https://placehold.co/200"
		};
		Context.Events.Add(@event);
		await Context.SaveChangesAsync();
		return @event;
	}

	protected async Task<Stand> SeedStandAsync(Guid? eventId = null, Guid? standId = null)
	{
		var testEventId = eventId ?? TestEventId;
		var @event = await Context.Events.FindAsync(testEventId);

		if (@event == null)
		{
			@event = await SeedEventAsync(testEventId);
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
		Context.Stands.Add(stand);
		await Context.SaveChangesAsync();
		return stand;
	}

	protected async Task<Product> SeedProductAsync(Guid standId, string name = "Test Product")
	{
		var stand = await Context.Stands
			.Include(s => s.Event)
			.FirstOrDefaultAsync(s => s.Id == standId);

		stand ??= await SeedStandAsync(standId: standId);

		var product = new Product
		{
			Id = Guid.NewGuid(),
			Name = name,
			Description = "Test product description",
			ImageUrl = "https://placehold.co/200",
			StandId = standId,
			Stand = stand,
			Price = 100.00m,
			IsAvailable = true
		};
		Context.Products.Add(product);
		await Context.SaveChangesAsync();
		return product;
	}

	protected async Task<User> SeedUserAsync(Guid? userId = null)
	{
		var id = userId ?? TestUserId;
		var user = new User
		{
			Id = id,
			Auth0Id = $"auth0|test-{id}"
		};
		Context.Users.Add(user);
		await Context.SaveChangesAsync();
		return user;
	}

	protected async Task<Cart> SeedCartAsync(Guid? standId = null, Guid? userId = null)
	{
		var testStandId = standId ?? TestStandId;
		var testUserId = userId ?? TestUserId;

		var stand = await Context.Stands.FindAsync(testStandId);
		if (stand == null)
		{
			stand = await SeedStandAsync(standId: testStandId);
		}

		var user = await Context.Users.FindAsync(testUserId);
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
		Context.Carts.Add(cart);
		await Context.SaveChangesAsync();
		return cart;
	}

	protected async Task<CartItem> SeedCartItemAsync(Guid standId, Guid cartId, Guid productId, int quantity = 1)
	{
		var cart = await Context.Carts.FindAsync(cartId, standId);
		var product = await Context.Products.FindAsync(productId);

		if (cart == null || product == null)
			throw new InvalidOperationException("Cart or Product not found");

		var cartItem = new CartItem
		{
			Id = Guid.NewGuid(),
			CartId = cart.Id,
			ProductId = product.Id,
			Quantity = quantity,
			Cart = cart,
			Product = product
		};
		Context.CartItems.Add(cartItem);
		await Context.SaveChangesAsync();
		return cartItem;
	}

	protected async Task<Notification> SeedNotificationAsync(Guid? eventId = null, Guid? userId = null)
	{
		var testEventId = eventId ?? TestEventId;
		var testUserId = userId ?? TestUserId;

		var @event = await Context.Events.FindAsync(testEventId);
		@event ??= await SeedEventAsync(testEventId);

		var user = await Context.Users.FindAsync(testUserId);
		user ??= await SeedUserAsync(testUserId);

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
		Context.Notifications.Add(notification);
		await Context.SaveChangesAsync();
		return notification;
	}
}
