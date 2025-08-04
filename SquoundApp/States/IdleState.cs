using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class IdleState : IState<RefinedSearchViewModel>
    {
        public Task Enter(RefinedSearchViewModel vm)
        {
            vm.IsTitleLabelVisible = false;

            vm.IsFilterButtonActive = true;
            vm.IsSortButtonActive = true;

            vm.IsFilterMenuActive = false;
            vm.IsSortMenuActive = false;

            return Task.CompletedTask;
        }


        public Task Update(RefinedSearchViewModel viewModel)
        {
            return Task.CompletedTask;
        }


        public Task Exit(RefinedSearchViewModel viewModel)
        {
            return Task.CompletedTask;
        }
    }
}
