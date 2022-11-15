using Serilog;
using ShredStore.StartUp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder);
builder.Host.UseSerilog();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ShredStore}/{action=Index}/{id?}");

app.Run();
