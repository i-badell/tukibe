using Api.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests.Controllers;

public class CartControllerTests : BaseControllerTest
{
	[Fact]
	public async Task GetCart_WithValidStandId_ReturnsOkWithCart()
	{
		// Arrange
		await SeedStandAsync(TestStandId);
		var cart = await SeedCartAsync(TestStandId);
		var product = await SeedProductAsync(TestStandId);
		var item = await SeedCartItemAsync(TestStandId, cart.Id, product.Id);

		// Act
		var response = await Controller.GetCart(TestStandId);

		// Assert
		Assert.IsType<OkObjectResult>(response.Result);
		var cartDto = (response.Result as OkObjectResult).Value as CartDto;
		Assert.NotNull(cartDto);
		Assert.Equal(cart.Id, cartDto.Id);
		Assert.Single(cartDto.Items);
		Assert.Equal(item.ProductId, cartDto.Items[0].ProductId);
	}

	[Fact]
	public async Task GetCart_WithInvalidStandId_ReturnsNotFound()
	{
		// Arrange
		await SeedStandAsync(TestStandId);

		// Act
		var response = await Controller.GetCart(Guid.NewGuid());

		// Assert
		Assert.IsType<NotFoundResult>(response.Result);
	}

	[Fact]
	public async Task GetCart_WithNoCartForStand_ReturnsNotFound()
	{
		// Arrange
		await SeedStandAsync(TestStandId);

		// Act
		var response = await Controller.GetCart(TestStandId);

		// Assert
		Assert.IsType<NotFoundResult>(response.Result);
	}

	[Fact]
	public async Task UpsertCart_CreateNewCart_ReturnsCreatedAtAction()
	{
		// Arrange 
		await SeedStandAsync(TestStandId);
		await SeedUserAsync();
		var product = await SeedProductAsync(TestStandId);
		var cartDto = new CartDto
		{
			StandId = TestStandId,
			Items = [new CartItemDto
			{
				ProductId = product.Id,
				Quantity = 2,
				UnitPrice = product.Price
			}]
		};

		// Act
		var response = await Controller.UpsertCart(TestStandId, cartDto);

		// Assert
		Assert.IsType<CreatedAtActionResult>(response.Result);
		var createdCart = (response.Result as CreatedAtActionResult).Value as CartDto;
		Assert.NotNull(createdCart);
		Assert.Equal(TestStandId, createdCart.StandId);
		Assert.Single(createdCart.Items);
		Assert.Equal(product.Id, createdCart.Items[0].ProductId);
		var cartInDb = await Context.Carts.FirstOrDefaultAsync(x => x.Id == createdCart.Id);
		Assert.NotNull(cartInDb);
		Assert.Single(cartInDb.Items);
	}

	[Fact]
	public async Task UpsertCart_AddItemsToExistingCart_ReturnsOk()
	{
		// Arrange 
		await SeedStandAsync(TestStandId);
		await SeedUserAsync();
		var existingProduct = await SeedProductAsync(TestStandId);
		var existingCart = await SeedCartAsync(TestStandId);
		await SeedCartItemAsync(TestStandId, existingCart.Id, existingProduct.Id);
		var newProduct = await SeedProductAsync(TestStandId, "New Product");
		var cartDto = new CartDto
		{
			StandId = TestStandId,
			Items = [new CartItemDto
			{
				ProductId = newProduct.Id,
				Quantity = 2,
				UnitPrice = newProduct.Price
			}]
		};

		// Act
		var response = await Controller.UpsertCart(TestStandId, cartDto);

		// Assert
		Assert.IsType<OkObjectResult>(response.Result);
		var updatedCart = (response.Result as OkObjectResult).Value as CartDto;
		Assert.NotNull(updatedCart);
		Assert.Equal(TestStandId, updatedCart.StandId);
		Assert.Equal(2, updatedCart.Items.Count);
		var cartInDb = await Context.Carts
			.Include(c => c.Items)
			.FirstOrDefaultAsync(x => x.Id == updatedCart.Id);
		Assert.NotNull(cartInDb);
		Assert.Equal(2, cartInDb.Items.Count);
	}

	[Fact]
	public async Task UpsertCart_MergeDuplicateItems_AggregatesQuantity()
	{
		// Arrange 
		await SeedStandAsync(TestStandId);
		await SeedUserAsync();
		var product = await SeedProductAsync(TestStandId);
		var existingCart = await SeedCartAsync(TestStandId);
		await SeedCartItemAsync(TestStandId, existingCart.Id, product.Id, 2);
		var cartDto = new CartDto
		{
			StandId = TestStandId,
			Items = [new CartItemDto
			{
				ProductId = product.Id,
				Quantity = 2,
				UnitPrice = product.Price
			}]
		};

		// Act
		var response = await Controller.UpsertCart(TestStandId, cartDto);

		// Assert
		Assert.IsType<OkObjectResult>(response.Result);
		var updatedCart = (response.Result as OkObjectResult).Value as CartDto;
		Assert.NotNull(updatedCart);
		Assert.Equal(TestStandId, updatedCart.StandId);
		Assert.Single(updatedCart.Items);
		var cartInDb = await Context.Carts
			.Include(c => c.Items)
			.FirstOrDefaultAsync(x => x.Id == updatedCart.Id);
		Assert.NotNull(cartInDb);
		Assert.Single(cartInDb.Items);
		Assert.Equal(4, cartInDb.Items.First().Quantity);
	}

	[Fact]
	public async Task UpsertCart_WithCompletedCart_CreatesNewCart()
	{
		// Arrange 
		await SeedStandAsync(TestStandId);
		await SeedUserAsync();
		var product = await SeedProductAsync(TestStandId);
		var completedCart = await SeedCartAsync(TestStandId);
		completedCart.IsCompleted = true;
		Context.Carts.Update(completedCart);
		await Context.SaveChangesAsync();
		var cartDto = new CartDto
		{
			StandId = TestStandId,
			Items = [new CartItemDto
			{
				ProductId = product.Id,
				Quantity = 2,
				UnitPrice = product.Price
			}]
		};

		// Act
		var response = await Controller.UpsertCart(TestStandId, cartDto);

		// Assert
		Assert.IsType<CreatedAtActionResult>(response.Result);
		var newCart = (response.Result as CreatedAtActionResult).Value as CartDto;
		Assert.NotNull(newCart);
		Assert.NotEqual(completedCart.Id, newCart.Id);
		Assert.Equal(TestStandId, newCart.StandId);
		Assert.Single(newCart.Items);
		var cartInDb = await Context.Carts
			.Include(c => c.Items)
			.FirstOrDefaultAsync(x => x.Id == newCart.Id);
		Assert.NotNull(cartInDb);
		Assert.Single(cartInDb.Items);
		Assert.NotEqual(completedCart.Id, cartInDb.Id);
	}

	[Fact]
	public async Task UpsertCart_WithInvalidStandId_ReturnsNotFound()
	{
		// Arrange
		await SeedStandAsync(TestStandId);
		var cartDto = new CartDto
		{
			StandId = TestStandId,
			Items = []
		};

		// Act
		var response = await Controller.UpsertCart(Guid.NewGuid(), cartDto);

		// Assert
		Assert.IsType<NotFoundResult>(response.Result);
	}

	[Fact]
	public async Task UpsertCart_WithEmptyItems_CreatesCartWithNoItems()
	{
		// Arrange
		await SeedStandAsync(TestStandId);
		await SeedUserAsync();
		var cartDto = new CartDto
		{
			StandId = TestStandId,
			Items = []
		};

		// Act
		var response = await Controller.UpsertCart(TestStandId, cartDto);

		// Assert
		Assert.IsType<CreatedAtActionResult>(response.Result);
		var createdCart = (response.Result as CreatedAtActionResult).Value as CartDto;
		Assert.NotNull(createdCart);
		Assert.Equal(TestStandId, createdCart.StandId);
		Assert.Empty(createdCart.Items);
		var cartInDb = await Context.Carts
				.Include(c => c.Items)
				.FirstOrDefaultAsync(x => x.Id == createdCart.Id);
		Assert.NotNull(cartInDb);
		Assert.Empty(cartInDb.Items);
	}

	[Fact]
	public async Task UpsertCart_WithMismatchedStandId_ReturnsBadRequest()
	{
		// Arrange
		await SeedStandAsync(TestStandId);
		await SeedUserAsync();
		var product = await SeedProductAsync(TestStandId);
		var differentStandId = Guid.NewGuid();
		var cartDto = new CartDto
		{
			StandId = differentStandId,
			Items = [new CartItemDto
			{
				ProductId = product.Id,
				Quantity = 2,
				UnitPrice = product.Price
			}]
		};

		// Act
		var response = await Controller.UpsertCart(TestStandId, cartDto);

		// Assert
		Assert.IsType<BadRequestResult>(response.Result);
	}

}
