namespace RecipeManagement.Resources.HangfireUtilities;

using Hangfire.Dashboard;

public class HangfireAuthorizationFilter : IDashboardAsyncAuthorizationFilter
{
    private readonly IServiceProvider _serviceProvider;
    
    public HangfireAuthorizationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<bool> AuthorizeAsync(DashboardContext context)
    {

        var env = _serviceProvider.GetService<IWebHostEnvironment>();
        return Task.FromResult(env.IsDevelopment());
    }
}