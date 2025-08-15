using SquoundApp.Services;


namespace SquoundApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await ServiceLocator.GetService<NavigationService>().GoToAsync("///LoadingPage");
        }
    }
}