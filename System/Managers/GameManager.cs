using System;
using System.Threading.Tasks;
using Skitter.Utilities.Singletons;
using Skitter.Utilities.Types;

namespace Skitter.Managers
{
    /// <summary> A singleton for the main game world. </summary>
    public partial class GameManager : SingletonNode<GameManager>
    {
        /// <summary> An event that is called at the beginning of a new turn. </summary>
        public EventPublisher TurnStart { get; init; } = new EventPublisher();

        /// <summary> An event that signals to listeners its time to act within the game world. </summary>
        /// <remarks> Both the player should provide input and the AI use their controllers. </remarks>
        public EventPublisher TurnProcess { get; init; } = new EventPublisher();

        /// <summary> An event that is called at the end of a complete turn. </summary>
        /// <remarks> This should be used to do a final clean up or check after the actions of the entities. </remarks>
        public EventPublisher TurnEnd { get; init; } = new EventPublisher();


        /// <summary> The current state of the present turn. </summary>
        public TurnState State { get; private set; } = TurnState.NONE;

        /// <summary> The current time in the game world. </summary>
        public DateTime CurrentTime { get; private set; } = DateTime.Now;


        /// <summary> Progress the current turn by first invoking entities to move, then clean up after them. </summary>
        public void ProgressTurn()
        {
            State = TurnState.START;
            Task startTask = Task.Run(() => TurnStart.InvokeAsync());
            startTask.GetAwaiter().GetResult();

            State = TurnState.PROCESS;
            Task processTask = Task.Run(() => TurnProcess.InvokeAsync());
            processTask.GetAwaiter().GetResult();

            State = TurnState.END;
            Task endTask = Task.Run(() => TurnEnd.InvokeAsync());
            endTask.GetAwaiter().GetResult();

            CurrentTime = CurrentTime.AddSeconds(1f);
        }
    }


    /// <summary> The possible states of the current turn. </summary>
    public enum TurnState
    {
        NONE,
        START,
        PROCESS,
        END
    }
}
