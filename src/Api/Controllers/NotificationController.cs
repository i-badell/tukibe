using System.Security.Claims;
using Api.Dto;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers;

[ApiController]
[Route("api/events")]
public class NotificationController : ControllerBase
{
	private readonly ILogger<NotificationController> _logger;

	public NotificationController(ILogger<NotificationController> logger)
	{
		_logger = logger;
	}

	[Authorize]
	[HttpGet("{eventId:guid}/notifications/count")]
	[ProducesResponseType<NotificationsCountResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetUnreadNotificationsCount(Guid eventId)
	{
		throw new NotImplementedException();
	}

	[Authorize]
	[HttpGet("{eventId:guid}/notifications")]
	[ProducesResponseType<List<NotificationDto>>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetNotifications(Guid eventId)
	{
		throw new NotImplementedException();
	}

	[Authorize]
	[HttpDelete("{eventId:guid}/notifications")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> DeleteAllNotifications(Guid eventId)
	{
		throw new NotImplementedException();
	}

	[Authorize]
	[HttpDelete("{eventId:guid}/notifications/{notificationId:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> DeleteNotificationById(Guid eventId, Guid notificationId)
	{
		throw new NotImplementedException();
	}

	[Authorize]
	[HttpPut("{eventId:guid}/notifications/read")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> ReadAllNotifications(Guid eventId)
	{
		throw new NotImplementedException();
	}

	[Authorize]
	[HttpPut("{eventId:guid}/notifications/read/{notificationId:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> ReadNotificationById(Guid eventId, Guid notificationId)
	{
		throw new NotImplementedException();
	}

	[Authorize]
	[HttpPut("{eventId:guid}/notifications/unread")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> UnreadAllNotifications(Guid eventId)
	{
		throw new NotImplementedException();
	}

	[Authorize]
	[HttpPut("{eventId:guid}/notifications/unread/{notificationId:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> UnreadNotificationById(Guid eventId, Guid notificationId)
	{
		throw new NotImplementedException();
	}
}
