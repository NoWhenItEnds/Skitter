#nullable disable warnings
using System.Threading.Tasks;
using Godot;
using Skitter.Managers;

namespace Skitter.Entities.Nodes
{
    /// <summary> A node representing an actor entity within the game world. </summary>
    public partial class ActorNode : Node2D
    {
        /// <summary> The actor's sprite. </summary>
        [ExportGroup("Nodes")]
        [Export] private AnimatedSprite2D _sprite;

        /// <summary> A label to display the actor's name. </summary>
        [Export] private RichTextLabel _nameLabel;  // TODO - A better solution. Just for debug purposes.


        /// <summary> The data entity this node represents in the game world. </summary>
        private Entity? _entity = null;


        /// <summary> A reference to the game manager singleton. </summary>
        private GameManager _gameManager;

        /// <summary> A reference to the entity manager singleton. </summary>
        private EntityManager _entityManager;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _gameManager = GameManager.Instance;
            _entityManager = EntityManager.Instance;

            _gameManager.TurnRender.Subscribe(OnTurnRenderAsync);
        }



        /// <summary> Initialise the node. </summary>
        /// <param name="entity"> The data entity this node represents in the game world. </param>
        public void Initialise(Entity entity)
        {
            _entity = entity;
            //_nameLabel.Text = $"{data.FirstName} {data.LastName}";
        }


        /// <summary> Update the node to accurately reflect the state of its data. </summary>
        private async Task OnTurnRenderAsync()
        {
            if (_entity != null)
            {
                Vector3 rawPosition = _entityManager.CalculateRenderPosition(_entity.Position);
                GlobalPosition = new Vector2(rawPosition.X, rawPosition.Y);
            }
        }


        /// <summary> Perform any clean up needed when unlinking the actor from it. </summary>
        public void CleanUp()
        {
            _entity = null;
        }


        /// <inheritdoc/>
        public override void _ExitTree()
        {
            GameManager.Instance.TurnRender.Unsubscribe(OnTurnRenderAsync);
        }
    }
}
