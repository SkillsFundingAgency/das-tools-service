using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using SFA.DAS.ToolService.Core;
using SFA.DAS.ToolService.Core.IRepositories;
using SFA.DAS.ToolService.Web.AppStart;
using SFA.DAS.ToolsNotifications.Client;
using SFA.DAS.ToolsNotifications.Client.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
IdentityModelEventSource.ShowPII = false;
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddServices(configuration);
builder.Services.AddDbContext<IToolServiceDbContext, ToolServiceDbContext>(options =>
        options.UseSqlServer(configuration["DatabaseConnectionString"]));

builder.Services.AddOptions();

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.SuppressXFrameOptionsHeader = true;
});

builder.Services.AddAuthorizationService();

builder.Services.AddAuthentication(configuration);
builder.Services.AddNotificationClient(configuration.Get<NotificationClientConfiguration>());

builder.Services.AddHealthChecks();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(options =>
    {
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(365);
    });
} else {

}

builder.Services.AddMvc(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
    options.EnableEndpointRouting = false;
})
.AddControllersAsServices();

builder.Services.AddApplicationInsightsTelemetry(configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

builder.Services.AddDistributedCache(configuration, builder.Environment);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else {
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Xss-Protection", "1");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'");
    await next();
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    if (context.Response.Headers.ContainsKey("X-Frame-Options"))
    {
        context.Response.Headers.Remove("X-Frame-Options");
    }

    context.Response.Headers.Add("X-Frame-Options", "DENY");

    await next();

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        //Re-execute the request so the user gets the error page
        var originalPath = context.Request.Path.Value;
        context.Items["originalPath"] = originalPath;
        context.Request.Path = "/error/404";
        await next();
    }
});

app.UseAuthentication();
app.UseHealthChecks("/health");
app.UseAuthorization();
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
