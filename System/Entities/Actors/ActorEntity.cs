using System;
using System.Threading.Tasks;
using Skitter.Entities.Actors.Actions;
using Skitter.Models;

namespace Skitter.Entities.Actors
{
    /// <summary> Represents a thinking, controllable entity within the game world. </summary>
    public class ActorEntity : Entity
    {
        /// <summary> The actor's current given name. </summary>
        public ActorName Name { get; private set; } = ActorName.Empty;

        /// <summary> The current action the actor is planning to do when it has enough saved units to pay for it. </summary>
        public ActorAction? QueuedAction { get; private set; } = null;

        /// <summary> Every action requires a number of units to be spent to do it. When an action is queued, it starts accumulating units every turn to pay for it. </summary>
        public Int32 CachedTurnUnits { get; private set; } = 0;


        /// <summary> Represents a thinking, controllable entity within the game world. </summary>
        public ActorEntity() : base()
        {
            Name = ActorName.Random(NameGender.NONE);
        }


        /// <inheritdoc/>
        protected override async Task OnTurnStartAsync()
        {
            // If there is a queued action, begin accumulating units to pay for it.
            if (QueuedAction != null)
            {
                CachedTurnUnits++;
            }
        }


        /// <inheritdoc/>
        protected override async Task OnTurnProcessAsync()
        {
            if (QueuedAction != null)
            {
                // If we have enough units to pay for the action...
                Int32 actionCost = QueuedAction.GetCost();
                if (CachedTurnUnits >= actionCost)
                {
                    // If the action was successful, remove the cost of the action from the cache.
                    Boolean isSuccess = QueuedAction.TryInvoke();
                    QueuedAction = null;    // Success or not, the action is no longer valid and needs to be removed.
                    if (isSuccess)
                    {
                        CachedTurnUnits -= actionCost;
                    }
                }
            }
        }


        /// <inheritdoc/>
        protected override async Task OnTurnEndAsync()
        {
            // If there is no queued action, remove any cached units.
            if (QueuedAction == null)
            {
                CachedTurnUnits = 0;
            }
        }


        /// <inheritdoc/>
        public override String GetUId() => Name.ToString();


        /// <summary> Attempt to add an action to the actor's queue. </summary>
        /// <param name="action"> The action to add. </param>
        /// <param name="forceOverwrite"> Whether the given action should overwrite one that is currently in progress. </param>
        /// <returns> Whether the action was successfully added. </returns>
        public Boolean TryQueueAction(ActorAction action, Boolean forceOverwrite = false)
        {
            Boolean isSuccess = false;
            if (QueuedAction == null || forceOverwrite)
            {
                QueuedAction = action;
                isSuccess = true;
            }
            return isSuccess;
        }
    }
}
