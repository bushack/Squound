using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class FilterState : IState<SortAndFilterViewModel>
    {
        public Task Enter(SortAndFilterViewModel viewModel)
        {
            viewModel.Title = "Filter Options";

            viewModel.IsTitleLabelVisible = true;

            viewModel.IsFilterButtonActive = false;
            viewModel.IsSortButtonActive = false;

            viewModel.IsFilterMenuActive = true;
            viewModel.IsSortMenuActive = false;

            return Task.CompletedTask;
        }


        public Task Update(SortAndFilterViewModel viewModel)
        {
            return Task.CompletedTask;
        }


        public Task Exit(SortAndFilterViewModel viewModel)
        {
            return Task.CompletedTask;
        }
    }
}
