using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class ResetState : IState<SortAndFilterViewModel>
    {
        public Task Enter(SortAndFilterViewModel viewModel)
        {
            viewModel.CurrentQuery.Keyword         = null;
            viewModel.CurrentQuery.Category        = null;
            viewModel.CurrentQuery.Manufacturer    = null;
            viewModel.CurrentQuery.MinPrice        = null;
            viewModel.CurrentQuery.MaxPrice        = null;

            viewModel.RestoreQueryToUserInterface(viewModel.CurrentQuery);

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
