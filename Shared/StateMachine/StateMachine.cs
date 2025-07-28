using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shared.Interfaces;


namespace Shared.StateMachine
{
    public sealed class StateMachine<T> : IStateMachine<T>
    {
        private readonly T owner;

        private IState<T>? currentState = null;
        private IState<T>? previousState = null;


        /// <summary>
        /// Parameterized constructor for the StateMachine class.
        /// </summary>
        /// <param name="owner"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public StateMachine(T owner)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner), "Owner cannot be null.");
        }


        /// <summary>
        /// Transitions the state machine from the current state to a new state.
        /// </summary>
        /// <param name="newState"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ChangeState(IState<T> newState)
        {
            if (newState == null)
                throw new ArgumentNullException(nameof(newState), "New state cannot be null.");

            previousState = currentState;

            currentState?.Exit(owner);

            currentState = newState;

            currentState?.Enter(owner);
        }


        /// <summary>
        /// Updates the current state of the owner by invoking the state's update logic.
        /// </summary>
        /// <remarks>This method delegates the update operation to the current state, if one is set.
        /// If <c>currentState</c> is <see langword="null"/>, the method performs no action.</remarks>
        public void Update()
        {
            currentState?.Update(owner);
        }


        /// <summary>
        /// Returns the current state of the state machine.
        /// </summary>
        public IState<T> CurrentState
        {
            get => currentState ?? throw new InvalidOperationException("Current state is not set.");
        }


        /// <summary>
        /// Returns the previous state of the state machine.
        /// </summary>
        public IState<T> PreviousState
        {
            get => previousState ?? throw new InvalidOperationException("Previous state is not set.");
        }


        /// <summary>
        /// Determines whether the current state is equal to the specified state.
        /// </summary>
        /// <param name="state">The state to compare with the current state. Cannot be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the current state is equal to the specified state; otherwise, <see
        /// langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
        public bool IsInState(IState<T> state)
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state), "State cannot be null.");

            return currentState == state;
        }


        /// <summary>
        /// Determines whether the object was previously in the specified state.
        /// </summary>
        /// <param name="state">The state to compare against the previous state. Cannot be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the object was previously in the specified state; otherwise, <see
        /// langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="state"/> is <see langword="null"/>.</exception>
        public bool WasInState(IState<T> state)
        {
            if (state is null)
                throw new ArgumentNullException(nameof(state), "State cannot be null.");

            return previousState == state;
        }
    }
}
