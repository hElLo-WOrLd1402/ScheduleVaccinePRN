using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
<<<<<<< Updated upstream
=======
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessLogicLayer;
using Microsoft.AspNetCore.Http;
>>>>>>> Stashed changes

namespace ScheduleVaccineRazor.Pages.Home
{
    public class MenuModel : PageModel
    {
<<<<<<< Updated upstream
        public void OnGet()
        {
=======
        private readonly IFeedbackService _feedbackService;
        private readonly IScheduleService _scheduleService; // Add IScheduleService to fetch the schedule
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuModel(IFeedbackService feedbackService, IHttpContextAccessor httpContextAccessor, IScheduleService scheduleService)
        {
            _feedbackService = feedbackService;
            _httpContextAccessor = httpContextAccessor;
            _scheduleService = scheduleService; // Inject IScheduleService
        }

        public List<Feedback> Feedbacks { get; set; } = new();

        [BindProperty]
        public int Rating { get; set; } = 5; // Default rating is 5 stars

        [BindProperty]
        public string? Comments { get; set; }

        // Get all feedbacks when the page loads
        public async Task OnGetAsync()
        {
            Feedbacks = await _feedbackService.GetAllFeedbacksAsync();
        }

        // Handle feedback submission
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate rating
            if (Rating < 1 || Rating > 5)
            {
                TempData["ErrorMessage"] = "Rating must be between 1 and 5.";
                return RedirectToPage();
            }

            // Retrieve the ParentId (UserId) from the session (assuming the user is logged in)
            var userId = _httpContextAccessor.HttpContext?.Session.GetString("ParentId");

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User is not logged in. Please log in to submit feedback.";
                return RedirectToPage("/Accounts/Login"); // Redirect to login page
            }

            // Fetch the first schedule for the user (or use a default if none exists)
            var schedule = await _scheduleService.GetAllSchedulesAsync();
            var userSchedule = schedule.FirstOrDefault(s => s.Child.ParentId == userId);

            if (userSchedule == null)
            {
                TempData["ErrorMessage"] = "No schedule found for the user. Please create a schedule first.";
                return RedirectToPage("/Home/ScheduleIndex"); // Redirect to the schedule creation page
            }

            var scheduleId = userSchedule.Id; // Use the first schedule ID as the ScheduleId

            // Create a new Feedback object
            var feedback = new Feedback
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId, // Retrieve UserId from session
                ScheduleId = scheduleId, // Use the first schedule ID
                Rating = Rating,
                Comments = string.IsNullOrWhiteSpace(Comments) ? null : Comments,
                CreatedTime = DateTime.UtcNow
            };

            // Add feedback to the database
            var result = await _feedbackService.AddFeedbackAsync(feedback);

            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to submit feedback. Please try again.";
            }
            else
            {
                TempData["SuccessMessage"] = "Feedback submitted successfully!";
            }

            return RedirectToPage();
>>>>>>> Stashed changes
        }
    }
}
