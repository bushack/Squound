using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class FilterState : IState<RefinedSearchViewModel>
    {
        public Task Enter(RefinedSearchViewModel vm)
        {
            vm.Title = "Filter Options";

            vm.IsTitleLabelVisible = true;

            vm.IsFilterButtonActive = false;
            vm.IsSortButtonActive = false;

            vm.IsFilterMenuActive = true;
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
