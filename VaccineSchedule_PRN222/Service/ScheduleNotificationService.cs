using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ScheduleNotificationService : BackgroundService
    {
        private readonly IEmailSender _emailSender;
        private readonly IScheduleService _scheduleService;

        public ScheduleNotificationService(IEmailSender emailSender, IScheduleService scheduleService)
        {
            _emailSender = emailSender;
            _scheduleService = scheduleService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendRemindersAsync(); // Run the reminder task
                await DelayUntilNextExecution(stoppingToken); // Wait until the next day
            }
        }

        private async Task SendRemindersAsync()
        {
            var tomorrow = DateTime.UtcNow.Date.AddDays(1); // Get tomorrow's date
            var upcomingAppointments = await _scheduleService.GetAllSchedulesAsync();
            upcomingAppointments = upcomingAppointments
                .Where(s => s.AppointmentDate.Date == tomorrow && s.Status == "Scheduled")
                .ToList();

            foreach (var schedule in upcomingAppointments)
            {
                string email = schedule.Child.Parent.Email; // Adjust based on your entity structure
                string subject = "Appointment Reminder";
                string message = $"Dear {schedule.Child.Parent.Username},\n\n"
                               + "This is a reminder that you have an appointment scheduled for tomorrow.\n\n"
                               + $"Date: {schedule.AppointmentDate:MMMM dd, yyyy hh:mm tt}\n"
                               + "Please ensure you attend on time.\n\nBest Regards,\nYour Clinic";

                await _emailSender.SendEmailAsync(email, subject, message);
            }
        }

        private async Task DelayUntilNextExecution(CancellationToken stoppingToken)
        {
            // Set the execution time to 12:00 AM UTC daily
            var now = DateTime.UtcNow;
            var nextRunTime = now.Date.AddDays(1); // Next midnight UTC
            var delay = nextRunTime - now;

            await Task.Delay(delay, stoppingToken);
        }
    }
}
