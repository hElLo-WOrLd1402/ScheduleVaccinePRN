using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BussinessLogicLayer;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class MenuModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;

        public MenuModel(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public List<Feedback> Feedbacks { get; set; } = new();

        [BindProperty]
        public int Rating { get; set; } = 5; // Mặc định 5 sao

        [BindProperty]
        public string? Comments { get; set; }

        public async Task OnGetAsync()
        {
            Feedbacks = await _feedbackService.GetAllFeedbacksAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Rating < 1 || Rating > 5)
            {
                TempData["ErrorMessage"] = "Rating must be between 1 and 5.";
                return RedirectToPage();
            }

            var feedback = new Feedback
            {
                Id = Guid.NewGuid().ToString(),
                UserId = "test_user", // ⚠️ Cần thay bằng UserId thực tế (từ session hoặc token)
                ScheduleId = "123", // ⚠️ Thay bằng ScheduleId hợp lệ
                Rating = Rating,
                Comments = string.IsNullOrWhiteSpace(Comments) ? null : Comments,
                CreatedTime = DateTime.UtcNow
            };

            var result = await _feedbackService.AddFeedbackAsync(feedback);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to submit feedback. Please try again.";
            }

            return RedirectToPage();
        }
    }
}
