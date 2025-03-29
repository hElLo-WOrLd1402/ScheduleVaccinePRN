
using BussinessLogicLayer;
using Service;
using Repository;
namespace ScheduleVaccineRazor
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký các dịch vụ
            services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IChildrenProfileService, ChildrenProfileService>();
            services.AddScoped<IChildrenProfileRepository, ChildrenProfileRepository>();
            return services;
        }
    }
}
