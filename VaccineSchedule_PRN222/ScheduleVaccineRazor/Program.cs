using ScheduleVaccineRazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR(); // Thêm SignalR
builder.Services.AddApplicationServices();



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Thời gian timeout session (30 phút)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Home/Index");
        return;
    }
    await next();
});
app.UseSession();
app.UseStaticFiles(); // Phải có dòng này để phục vụ file tĩnh (CSS, JS, Images)
app.UseRouting();
app.MapHub<SignalrServer>("/signalrServer");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
