using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class CancelState : IState<RefinedSearchViewModel>
    {
        public Task Enter(RefinedSearchViewModel vm)
        {
            vm.RestoreQueryToUserInterface(vm.PreviousQuery);

            return Task.CompletedTask;
        }


        public Task Update(RefinedSearchViewModel vm)
        {
            return Task.CompletedTask;
        }


        public Task Exit(RefinedSearchViewModel vm)
        {
            return Task.CompletedTask;
        }
    }
}
