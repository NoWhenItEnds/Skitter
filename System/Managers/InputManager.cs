#nullable disable warnings
using System;
using Godot;
using Skitter.Entities.Actors;
using Skitter.Entities.Actors.Actions;
using Skitter.Nodes;
using Skitter.Utilities.Singletons;

namespace Skitter.Managers
{
    /// <summary> The manager to translate the player's input into controlling the game world. </summary>
    public partial class InputManager : SingletonNode<InputManager>
    {
        /// <summary> The camera following the player's view. </summary>
        [ExportGroup("Nodes")]
        [Export] private GameCamera _playerCamera;


        /// <summary> A reference to world game manager. </summary>
        private GameManager _gameManager;

        /// <summary> A reference to world grid manager. </summary>
        private GridManager _gridManager;

        /// <summary> A reference to the current player entity. </summary>
        private ActorEntity _player;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _gameManager = GameManager.Instance;
            _gridManager = GridManager.Instance;

            _player = EntityManager.Instance.Player;
        }


        // TODO - THERE IS A BETTER WAY TO PROGRESS IF PLAYER ISN"T READY.
        public override void _Process(Double delta)
        {
            if (_player.QueuedAction != null)
            {
                _gameManager.ProgressTurn();
            }
        }



        /// <inheritdoc/>
        public override void _Input(InputEvent @event)
        {
            MoveAction? action = null;
            action = CheckMovement(@event);

            if (action != null)
            {
                Boolean wasAdded = _player.TryQueueAction(action);
                if (wasAdded)
                {
                    _gameManager.ProgressTurn();    // TODO - NOT LIKE THIS!

                    // TODO - Not like this either!
                    Vector3 playerPosition = _gridManager.CalculateRenderPosition(_player.GetPosition());
                    _playerCamera.GlobalPosition = new Vector2(playerPosition.X, playerPosition.Y);

                }
            }
        }


        private MoveAction? CheckMovement(InputEvent @event)
        {
            MoveAction? action = null;

            if (@event.IsAction("action_move_n"))
            {
                action = new MoveAction(_player, Vector3I.Down);
            }
            else if (@event.IsAction("action_move_e"))
            {
                action = new MoveAction(_player, Vector3I.Right);
            }
            else if (@event.IsAction("action_move_s"))
            {
                action = new MoveAction(_player, Vector3I.Up);
            }
            else if (@event.IsAction("action_move_w"))
            {
                action = new MoveAction(_player, Vector3I.Left);
            }
            else if (@event.IsAction("action_move_ne"))
            {
                action = new MoveAction(_player, new Vector3I(1, -1, 0));
            }
            else if (@event.IsAction("action_move_se"))
            {
                action = new MoveAction(_player, new Vector3I(1, 1, 0));
            }
            else if (@event.IsAction("action_move_sw"))
            {
                action = new MoveAction(_player, new Vector3I(-1, 1, 0));
            }
            else if (@event.IsAction("action_move_nw"))
            {
                action = new MoveAction(_player, new Vector3I(-1, -1, 0));
            }

            return action;
        }
    }
}
