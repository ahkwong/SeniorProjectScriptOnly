using System;
using System.Collections.Generic;
using UnityEngine;

namespace Senior.Globals
{
    public sealed class StateMachine<T>
    {
        private readonly T context;
        private readonly Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

        public State<T> CurrentState { get; private set; }

        public State<T> PreviousState { get; private set; }
        private float elapsedTimeInState = 0f;

        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;

            AddState(initialState);
            CurrentState = initialState;
            CurrentState.Start();
        }

        public void AddState(State<T> state)
        {
            state.SetUp(this, context);
            states[state.GetType()] = state;
        }

        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;
            CurrentState.DetermineNextState();
            CurrentState.Update(deltaTime);
        }

        public R ChangeState<R>() where R : State<T>
        {
            // avoid changing to the same state
            var newType = typeof(R);
            if (CurrentState.GetType() == newType)
                return CurrentState as R;

            // only call end if we have a currentState
            if (CurrentState != null)
                CurrentState.Finish();

#if UNITY_EDITOR

            // Ensure we have the given state in our state list
            if (!states.ContainsKey(newType))
            {
                var error = GetType() + ": state " + newType + " does not exist. Did you forget to add it by calling AddState?";
                Debug.LogError(error);
                throw new Exception(error);
            }
#endif

            // Swap states and call start
            PreviousState = CurrentState;
            CurrentState = states[newType];
            CurrentState.Start();
            elapsedTimeInState = 0f;

            return CurrentState as R;
        }
    }
}