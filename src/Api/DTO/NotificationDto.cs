namespace Api.Dto;

public class NotificationDto
{
    public Guid NotificationId { get; set; }
    public string NotificationTitle { get; set; } = default!;
    public string NotificationMessage { get; set; } = default!;
    public bool IsRead { get; set; }
}
