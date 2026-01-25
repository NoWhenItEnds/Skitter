#nullable disable warnings
using Administrator.Utilities.Singletons;
using Godot;

namespace Skitter.Managers
{
    /// <summary> A manager singleton for the player-controlled camera within the game world. </summary>
    public partial class CameraManager : SingletonNode2D<CameraManager>
    {
        [ExportGroup("Nodes")]
        [Export] private Camera2D _mainCamera;
    }
}
