using Microsoft.Extensions.DependencyInjection;
using Views;

namespace AsyncDemoApp
{
    public class TasksPageRouteFactory : RouteFactory
    {
        // מימוש ראשון
        public override Element GetOrCreate()
        {
            var services = Application.Current?
                .Handler?
                .MauiContext?
                .Services;

            if (services == null)
            {
                throw new InvalidOperationException(
                    "Service provider not found");
            }

            return services.GetRequiredService<TasksPage>();
        }

        // מימוש שני
        public override Element GetOrCreate(IServiceProvider services)
        {
            return services.GetRequiredService<TasksPage>();
        }
    }
}