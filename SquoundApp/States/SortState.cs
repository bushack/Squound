using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class SortState : IState<SearchViewModel>
    {
        public Task Enter(SearchViewModel vm)
        {
            vm.Title = "Sort Options";

            vm.IsTitleLabelVisible = true;

            vm.IsFilterButtonActive = false;
            vm.IsSortButtonActive = false;

            vm.IsFilterMenuActive = false;
            vm.IsSortMenuActive = true;

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
