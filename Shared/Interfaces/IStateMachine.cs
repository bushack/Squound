

namespace Shared.Interfaces
{
    public interface IState<T>
    {
        void Enter(T owner);
        void Exit(T owner);
        void Update(T owner);
    }


    public interface IStateMachine<T>
    {
        void ChangeState(IState<T> newState);
        void Update();
        IState<T> CurrentState { get; }
        IState<T> PreviousState { get; }
        bool IsInState(IState<T> state);
        bool WasInState(IState<T> state);
    }
}
