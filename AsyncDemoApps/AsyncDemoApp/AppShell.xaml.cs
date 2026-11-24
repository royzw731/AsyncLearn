using Views;

namespace AsyncDemoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                nameof(TasksPage),
                new TasksPageRouteFactory());
        }
    }
}