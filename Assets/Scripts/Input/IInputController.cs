using System;
using UnityEngine;

namespace Input
{
    public interface IInputController
    {
        event Action<Vector2> OnLeftMouseButtonClicked;
    }
}