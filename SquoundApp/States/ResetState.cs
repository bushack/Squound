using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class ResetState : IState<SearchViewModel>
    {
        public Task Enter(SearchViewModel vm)
        {
            vm.CurrentQuery.Keyword         = null;
            vm.CurrentQuery.Category        = null;
            vm.CurrentQuery.Manufacturer    = null;
            vm.CurrentQuery.MinPrice        = null;
            vm.CurrentQuery.MaxPrice        = null;

            vm.RestoreQueryToUserInterface(vm.CurrentQuery);

            return Task.CompletedTask;
        }


        public Task Update(SearchViewModel vm)
        {
            return Task.CompletedTask;
        }


        public Task Exit(SearchViewModel vm)
        {
            return Task.CompletedTask;
        }
    }
}
