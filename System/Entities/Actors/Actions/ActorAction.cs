using System;

namespace Skitter.Entities.Actors.Actions
{
    /// <summary> A discrete action an actor can do. </summary>
    public abstract class ActorAction
    {
        /// <summary> The actor performing the action. </summary>
        protected readonly ActorEntity PERFORMER;


        /// <summary> A discrete action an actor can do. </summary>
        /// <param name="performer"> The actor performing the action. </param>
        public ActorAction(ActorEntity performer)
        {
            PERFORMER = performer;
        }


        /// <summary> Get how much the action will cost, in turn units, to complete. </summary>
        public abstract Int32 GetCost();


        /// <summary> Attempt to invoke the action. </summary>
        /// <returns> Whether the action was successful / valid. If this is false, its cost should be refunded for another action instantly. </returns>
        public abstract Boolean TryInvoke();
    }
}
