namespace Skitter.Entities
{
    /// <summary> A controller / brain for an actor entity. </summary>
    public class ActorController
    {
        /// <summary> The actor this controller manipulates. </summary>
        public Actor ControlledActor { get; private set; }


        /// <summary> A controller / brain for an actor entity. </summary>
        /// <param name="controlledActor"> The actor this controller manipulates. </param>
        public ActorController(Actor controlledActor)
        {
            ControlledActor = controlledActor;
        }
    }
}
