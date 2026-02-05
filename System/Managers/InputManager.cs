#nullable disable warnings
using Administrator.Utilities.Singletons;
using Godot;
using System;

namespace Skitter.Managers
{
    /// <summary> The manager to translate the player's input into controlling the game world. </summary>
    public partial class InputManager : SingletonNode<InputManager>
    {
        /// <summary> A reference to world game manager. </summary>
        private GameManager _gameManager;

        /// <summary> A reference to the manager that holds actor entities. </summary>
        private ActorManager _actorManager;

        /// <summary> The direction currently being input by the player. </summary>
        private Vector2 _inputDirection = Vector2.Zero;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _gameManager = GameManager.Instance;
            _actorManager = ActorManager.Instance;
        }


        /// <inheritdoc/>
        public override void _PhysicsProcess(Double delta)
        {
            // Build the direction.
            Single ns = Input.GetAxis("action_north", "action_south");
            Single ew = Input.GetAxis("action_west", "action_east");
            _inputDirection = new Vector2(ew, ns);

            // TODO - Ensure we aren't in an UI.
            //_actorManager.PlayerController.TryMove(_inputDirection, out IEntityNode? _);
        }


        /// <inheritdoc/>
        public override void _Input(InputEvent @event)
        {
            _gameManager.ProgressTurn();
        }
    }
}
