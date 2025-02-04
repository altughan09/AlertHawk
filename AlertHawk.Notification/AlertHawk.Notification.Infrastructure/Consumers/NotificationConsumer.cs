using System.Diagnostics.CodeAnalysis;
using AlertHawk.Notification.Domain.Entities;
using AlertHawk.Notification.Domain.Interfaces.Notifiers;
using AlertHawk.Notification.Domain.Interfaces.Services;
using MassTransit;

namespace SharedModels;

[ExcludeFromCodeCoverage]
public class NotificationConsumer : IConsumer<NotificationAlert>
{
    private readonly ISlackNotifier _slackNotifier;
    private readonly INotificationService _notificationService;

    public NotificationConsumer(ISlackNotifier slackNotifier, INotificationService notificationService)
    {
        _slackNotifier = slackNotifier;
        _notificationService = notificationService;
    }

    public async Task Consume(ConsumeContext<NotificationAlert> context)
    {
        Console.WriteLine($"Received from RabbitMq, " +
                          $"Message: {context.Message.Message} " +
                          $"NotificationId: {context.Message.NotificationId}" +
                          $"TimeStamp: {context.Message.TimeStamp}");

        var notificationItem = await _notificationService.SelectNotificationItemById(context.Message.NotificationId);

        if (notificationItem?.NotificationEmail != null)
        {
            Console.WriteLine("Sending Email notification");
            var notificationSend = new NotificationSend
            {
                NotificationEmail = notificationItem.NotificationEmail,
                Message = context.Message.Message,
                NotificationTypeId = notificationItem.NotificationTypeId
            };
            await _notificationService.Send(notificationSend);
        }

        if (notificationItem?.NotificationTeams != null)
        {
            Console.WriteLine("Sending Teams notification");
            var notificationSend = new NotificationSend
            {
                NotificationTeams = notificationItem.NotificationTeams,
                Message = context.Message.Message,
                NotificationTypeId = notificationItem.NotificationTypeId
            };
            await _notificationService.Send(notificationSend);
        }

        if (notificationItem?.NotificationSlack != null)
        {
            Console.WriteLine("Sending Slack notification");
            var notificationSend = new NotificationSend
            {
                NotificationSlack = notificationItem.NotificationSlack,
                Message = context.Message.Message,
                NotificationTypeId = notificationItem.NotificationTypeId
            };
            await _notificationService.Send(notificationSend);
        }

        if (notificationItem?.NotificationTelegram != null)
        {
            Console.WriteLine("Sending Telegram notification");
            var notificationSend = new NotificationSend
            {
                NotificationTelegram = notificationItem.NotificationTelegram,
                Message = context.Message.Message,
                NotificationTypeId = notificationItem.NotificationTypeId
            };
            await _notificationService.Send(notificationSend);
        }

        // Handle the received message
        Console.WriteLine(
            $"NotificationId: {context.Message.NotificationId} Notification Message: {context.Message.Message}");
    }
}