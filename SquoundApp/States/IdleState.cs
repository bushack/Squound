//using Shared.Interfaces;

//using SquoundApp.ViewModels;


//namespace SquoundApp.States
//{
//    internal class IdleState : IState<SortAndFilterViewModel>
//    {
//        public Task Enter(SortAndFilterViewModel viewModel)
//        {
//            viewModel.IsTitleLabelVisible = false;

//            viewModel.IsFilterButtonActive = true;
//            viewModel.IsSortButtonActive = true;

//            viewModel.IsFilterMenuActive = false;
//            viewModel.IsSortMenuActive = false;

//            return Task.CompletedTask;
//        }


//        public Task Update(SortAndFilterViewModel viewModel)
//        {
//            return Task.CompletedTask;
//        }


//        public Task Exit(SortAndFilterViewModel viewModel)
//        {
//            return Task.CompletedTask;
//        }
//    }
//}
