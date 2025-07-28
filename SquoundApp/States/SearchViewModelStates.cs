using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class SearchViewModelIdleState : IState<SearchViewModel>
    {
        public void Enter(SearchViewModel viewModel)
        {
            viewModel.IsTitleLabelVisible = false;

            viewModel.IsFilterButtonActive = true;
            viewModel.IsSortButtonActive = true;

            viewModel.IsFilterMenuActive = false;
            viewModel.IsSortMenuActive = false;
        }


        public void Update(SearchViewModel viewModel)
        {
        }


        public void Exit(SearchViewModel viewModel)
        {
        }
    }


    internal class SearchViewModelSortMenuState : IState<SearchViewModel>
    {
        public void Enter(SearchViewModel viewModel)
        {
            viewModel.Title = "Sort Options";

            viewModel.IsTitleLabelVisible = true;

            viewModel.IsFilterButtonActive = false;
            viewModel.IsSortButtonActive = false;

            viewModel.IsFilterMenuActive = false;
            viewModel.IsSortMenuActive = true;
        }


        public void Update(SearchViewModel viewModel)
        {
        }


        public void Exit(SearchViewModel viewModel)
        {
        }
    }


    internal class SearchViewModelFilterMenuState : IState<SearchViewModel>
    {
        public void Enter(SearchViewModel viewModel)
        {
            viewModel.Title = "Filter Options";

            viewModel.IsTitleLabelVisible = true;

            viewModel.IsFilterButtonActive = false;
            viewModel.IsSortButtonActive = false;

            viewModel.IsFilterMenuActive = true;
            viewModel.IsSortMenuActive = false;
        }


        public void Update(SearchViewModel viewModel)
        {
        }


        public void Exit(SearchViewModel viewModel)
        {
        }
    }
}