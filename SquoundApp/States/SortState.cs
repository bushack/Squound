using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class SortState : IState<RefinedSearchViewModel>
    {
        public Task Enter(RefinedSearchViewModel vm)
        {
            vm.Title = "Sort Options";

            vm.IsTitleLabelVisible = true;

            vm.IsFilterButtonActive = false;
            vm.IsSortButtonActive = false;

            vm.IsFilterMenuActive = false;
            vm.IsSortMenuActive = true;

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
