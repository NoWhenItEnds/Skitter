using System;

namespace Skitter.Entities.Actors.Actions
{
    /// <summary> The actor eats another entity. </summary>
    public class ConsumeAction : ActorAction
    {
        /// <summary> The actor eats another entity. </summary>
        /// <param name="performer"> The actor performing the action. </param>
        public ConsumeAction(ActorEntity performer) : base(performer) { }


        /// <inheritdoc/>
        public override Int32 GetCost()
        {
            return 10;
        }


        /// <inheritdoc/>
        public override Boolean TryInvoke()
        {
            return false;
        }
    }
}
