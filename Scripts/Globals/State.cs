namespace Senior.Globals
{
    public abstract class State<T>
    {
        protected T context;
        protected StateMachine<T> machine;

        public State() { }

        internal void SetUp(StateMachine<T> machine, T context)
        {
            this.context = context;
            this.machine = machine;

            Initialize();
        }

        /// <summary>
        /// Called during the set up phase. Use this to bind any initial attributes needed for this state
        /// to function. Ex. if the state needs the input controller, or movement manager.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Called once at the beginning of the state. Use this to change bool variables in animators, etc.
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Put all logic that determines the next state here.
        /// </summary>
        public virtual void DetermineNextState() { }

        /// <summary>
        /// Anything that is called every update frame is put here. Ex. Movement.
        /// </summary>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        /// Called once at the end ofthe state. Use this to change bool variables, etc.
        /// </summary>
        public virtual void Finish() { }

    }
}