﻿namespace AlertHawk.Notification.Domain.Entities
{
    public class NotificationSend
    {
        public int NotificationTypeId { get; set; }
        public required string Message { get; set; }
        public NotificationEmail NotificationEmail { get; set; } = null!;
        public NotificationSlack NotificationSlack { get; set; } = null!;
        public NotificationTeams NotificationTeams { get; set; } = null!;
        public NotificationTelegram NotificationTelegram { get; set; } = null!;
        public DateTime NotificationTimeStamp { get; set; }
    }
}