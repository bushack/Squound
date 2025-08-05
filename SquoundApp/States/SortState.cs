using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class SortState : IState<SortAndFilterViewModel>
    {
        public Task Enter(SortAndFilterViewModel viewModel)
        {
            viewModel.Title = "Sort Options";

            viewModel.IsTitleLabelVisible = true;

            viewModel.IsFilterButtonActive = false;
            viewModel.IsSortButtonActive = false;

            viewModel.IsFilterMenuActive = false;
            viewModel.IsSortMenuActive = true;

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
