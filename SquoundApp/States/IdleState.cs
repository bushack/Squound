using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class IdleState : IState<SearchViewModel>
    {
        public Task Enter(SearchViewModel vm)
        {
            vm.IsTitleLabelVisible = false;

            vm.IsFilterButtonActive = true;
            vm.IsSortButtonActive = true;

            vm.IsFilterMenuActive = false;
            vm.IsSortMenuActive = false;

            return Task.CompletedTask;
        }


        public Task Update(SearchViewModel viewModel)
        {
            return Task.CompletedTask;
        }


        public Task Exit(SearchViewModel viewModel)
        {
            return Task.CompletedTask;
        }
    }
}
