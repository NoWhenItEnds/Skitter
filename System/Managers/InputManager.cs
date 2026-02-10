#nullable disable warnings
using Godot;
using Skitter.Utilities.Singletons;
using System;

namespace Skitter.Managers
{
    /// <summary> The manager to translate the player's input into controlling the game world. </summary>
    public partial class InputManager : SingletonNode<InputManager>
    {
        /// <summary> A reference to world game manager. </summary>
        private GameManager _gameManager;

        /// <summary> The direction currently being input by the player. </summary>
        private Vector2 _inputDirection = Vector2.Zero;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _gameManager = GameManager.Instance;
        }


        /// <inheritdoc/>
        public override void _Input(InputEvent @event)
        {
            Boolean isInput = false;
            isInput = CheckMovement(@event);

            if (isInput)
            {
                _gameManager.ProgressTurn();
            }
        }


        private Boolean CheckMovement(InputEvent @event)
        {
            Boolean isInput = false;

            if (@event.IsAction("action_move_n"))
            {
                isInput = true;
                EntityManager.Instance.Player.TryMove(Vector3I.Down);
            }
            else if (@event.IsAction("action_move_e"))
            {
                isInput = true;
                EntityManager.Instance.Player.TryMove(Vector3I.Right);
            }
            else if (@event.IsAction("action_move_s"))
            {
                isInput = true;
                EntityManager.Instance.Player.TryMove(Vector3I.Up);
            }
            else if (@event.IsAction("action_move_w"))
            {
                isInput = true;
                EntityManager.Instance.Player.TryMove(Vector3I.Left);
            }
            else if (@event.IsAction("action_move_ne"))
            {
                isInput = true;
                EntityManager.Instance.Player.TryMove(new Vector3I(1, -1, 0));
            }
            else if (@event.IsAction("action_move_se"))
            {
                isInput = true;
                EntityManager.Instance.Player.TryMove(new Vector3I(1, 1, 0));
            }
            else if (@event.IsAction("action_move_sw"))
            {
                isInput = true;
                EntityManager.Instance.Player.TryMove(new Vector3I(-1, 1, 0));
            }
            else if (@event.IsAction("action_move_nw"))
            {
                isInput = true;
                EntityManager.Instance.Player.TryMove(new Vector3I(-1, -1, 0));
            }

            return isInput;
        }
    }
}
