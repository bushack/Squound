

namespace Shared.Interfaces
{
    public interface IState<TState>
    {
        Task Enter(TState owner);
        Task Exit(TState owner);
        Task Update(TState owner);
    }


    public interface IStateMachine<TState>
    {
        Task ChangeState(IState<TState> newState);
        Task Update();
        IState<TState> CurrentState { get; }
        IState<TState> PreviousState { get; }
        bool IsInState(IState<TState> state);
        bool WasInState(IState<TState> state);
    }
}
