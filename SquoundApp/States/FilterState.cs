using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class FilterState : IState<SearchViewModel>
    {
        public Task Enter(SearchViewModel vm)
        {
            vm.Title = "Filter Options";

            vm.IsTitleLabelVisible = true;

            vm.IsFilterButtonActive = false;
            vm.IsSortButtonActive = false;

            vm.IsFilterMenuActive = true;
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
