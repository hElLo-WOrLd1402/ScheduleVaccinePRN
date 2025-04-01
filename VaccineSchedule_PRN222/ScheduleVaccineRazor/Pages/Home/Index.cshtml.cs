using BussinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace ScheduleVaccineRazor.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IAccountService _accountService;
        private readonly IScheduleService _scheduleService;

        public IndexModel(IFeedbackService feedbackService, IAccountService accountService, IScheduleService scheduleService)
        {
            _feedbackService = feedbackService;
            _accountService = accountService;
            _scheduleService = scheduleService;
        }

        [BindProperty]
        public string Comments { get; set; } = string.Empty;

        [BindProperty]
        public int Rating { get; set; }

        public bool FeedbackSent { get; set; } = false;

        public List<Feedback> FeedbackList { get; set; } = new();

        public async Task OnGetAsync()
        {
            FeedbackList = await _feedbackService.GetAllFeedbacksAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Rating < 1 || Rating > 5)
            {
                ModelState.AddModelError(string.Empty, "Rating must be between 1 and 5.");
                return Page();
            }

            // **Lấy UserId từ tài khoản hiện tại** (giả sử email đăng nhập là "test@example.com")
            string email = "test@example.com"; // Thay thế bằng email đăng nhập thực tế
            var user = await _accountService.GetAccountByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }

            // **Lấy ScheduleId theo lịch tiêm gần nhất**
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            var upcomingSchedule = schedules
                .Where(s => s.AppointmentDate >= DateTime.UtcNow)
                .OrderBy(s => s.AppointmentDate)
                .FirstOrDefault();

            var selectedSchedule = upcomingSchedule ?? schedules.OrderByDescending(s => s.Id).FirstOrDefault();

            if (selectedSchedule == null)
            {
                ModelState.AddModelError(string.Empty, "No schedule found.");
                return Page();
            }

            // **Sinh FeedbackId tự động theo `F00001++`**
            var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
            string newFeedbackId = GenerateFeedbackId(feedbacks);

            var feedback = new Feedback
            {
                Id = newFeedbackId,
                UserId = user.Id,
                ScheduleId = selectedSchedule.Id,
                Rating = Rating,
                Comments = Comments,
                CreatedTime = DateTime.UtcNow
            };

            bool success = await _feedbackService.AddFeedbackAsync(feedback);

            if (success)
            {
                FeedbackSent = true;
                FeedbackList = await _feedbackService.GetAllFeedbacksAsync();
            }

            return Page();
        }

        // **Hàm sinh mã Feedback tự động theo F00001++**
        private string GenerateFeedbackId(List<Feedback> feedbacks)
        {
            if (feedbacks == null || feedbacks.Count == 0)
            {
                return "F00001";
            }

            var lastFeedback = feedbacks.OrderByDescending(f => f.Id).FirstOrDefault();
            if (lastFeedback == null || !lastFeedback.Id.StartsWith("F"))
            {
                return "F00001";
            }

            string numberPart = lastFeedback.Id.Substring(1);
            if (int.TryParse(numberPart, out int lastNumber))
            {
                return $"F{(lastNumber + 1):D5}";
            }

            return "F00001";
        }
    }
}
