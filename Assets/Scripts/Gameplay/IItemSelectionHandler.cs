using System;
using Gameplay.GameItems;

namespace Gameplay
{
    public interface IItemSelectionHandler
    {
        event Action<Chip, Point> OnPointToMoveSelected;
        void SetClickProcessing(bool enabled);
    }
}