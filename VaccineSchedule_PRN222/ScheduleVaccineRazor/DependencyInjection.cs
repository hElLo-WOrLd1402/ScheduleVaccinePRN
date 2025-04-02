
using BussinessLogicLayer;
using Service;

namespace ScheduleVaccineRazor
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký các dịch vụ
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IScheduleService, ScheduleService>();
<<<<<<< Updated upstream

=======
            services.AddScoped<IChildrenProfileService, ChildrenProfileService>();
            services.AddScoped<IVaccineService, VaccineService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IChildrenProfileRepository, ChildrenProfileRepository>();
            services.AddScoped<IVaccineRepository, VaccineRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddHttpContextAccessor();
            // Đăng ký DAO
            services.AddScoped<FeedbackDAO>();
>>>>>>> Stashed changes
            return services;
        }
    }
}
