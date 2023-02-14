#nullable disable

namespace TaleLearnCode.ThumbnailGenerator.Models;

public class EventSubscriptionNotification
{
	public string Id { get; set; }
	public string Topic { get; set; }
	public string Subject { get; set; }
	public string EventTime { get; set; }
	public string EventType { get; set; }
	public EventSubscriptionNotificationData Data { get; set; }
}