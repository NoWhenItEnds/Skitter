#nullable disable warnings
using Godot;
using Skitter.Utilities.Singletons;

namespace Skitter.Managers
{
    /// <summary> A manager singleton for the player-controlled camera within the game world. </summary>
    public partial class CameraManager : SingletonNode2D<CameraManager>
    {
        [ExportGroup("Nodes")]
        [Export] private Camera2D _mainCamera;
    }
}
