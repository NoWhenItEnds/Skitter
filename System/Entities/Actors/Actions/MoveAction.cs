using System;
using Godot;

namespace Skitter.Entities.Actors.Actions
{
    /// <summary> Move the actor within the game world. </summary>
    public class MoveAction : ActorAction
    {
        /// <summary> The direction of the actor's desired movement. </summary>
        private Vector3I _moveDirection = Vector3I.Zero;

        /// <summary> Move the actor within the game world. </summary>
        /// <param name="performer"> The actor performing the action. </param>
        /// <param name="direction"> The direction of the actor's desired movement. </param>
        public MoveAction(ActorEntity performer, Vector3I direction) : base(performer)
        {
            _moveDirection = direction;
        }


        /// <inheritdoc/>
        public override Int32 GetCost()
        {
            // TODO - Calculate from unit speed / tile kind.
            return 3;
        }


        /// <inheritdoc/>
        public override Boolean TryInvoke()
        {
            Vector3I destination = PERFORMER.GetPosition() + _moveDirection;
            return PERFORMER.TrySetPosition(destination);
        }
    }
}
