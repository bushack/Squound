using SquoundApp.Interfaces;
using SquoundApp.Pages;


namespace SquoundApp
{
    public partial class App : Application
    {
        private readonly INavigationService _Navigation;


        public App(INavigationService navigation)
        {
            _Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await _Navigation.GoToAsync($"///{nameof(LoadingPage)}");
        }
    }
}