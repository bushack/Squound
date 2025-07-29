using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class CancelState : IState<SearchViewModel>
    {
        public Task Enter(SearchViewModel vm)
        {
            vm.RestoreQueryToUserInterface(vm.PreviousQuery);

            return Task.CompletedTask;
        }


        public Task Update(SearchViewModel vm)
        {
            return Task.CompletedTask;
        }


        public Task Exit(SearchViewModel vm)
        {
            return Task.CompletedTask;
        }
    }
}
