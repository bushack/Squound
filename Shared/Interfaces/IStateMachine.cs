

namespace Shared.Interfaces
{
    public interface IState<T>
    {
        Task Enter(T owner);
        Task Exit(T owner);
        Task Update(T owner);
    }


    public interface IStateMachine<T>
    {
        Task ChangeState(IState<T> newState);
        Task Update();
        IState<T> CurrentState { get; }
        IState<T> PreviousState { get; }
        bool IsInState(IState<T> state);
        bool WasInState(IState<T> state);
    }
}
