
using BussinessLogicLayer;
using Service;
using Repository;
using DataAccessLayer.DAO;
namespace ScheduleVaccineRazor
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký các dịch vụ
            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton<IScheduleService, ScheduleService>();
            services.AddScoped<IChildrenProfileService, ChildrenProfileService>();
            services.AddScoped<IVaccineService, VaccineService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IChildrenProfileRepository, ChildrenProfileRepository>();
            services.AddScoped<IVaccineRepository, VaccineRepository>();
            services.AddSingleton<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddSingleton<IEmailSender, EmailSender>();
            // Đăng ký DAO
            services.AddScoped<FeedbackDAO>();
            return services;
        }
    }
}
