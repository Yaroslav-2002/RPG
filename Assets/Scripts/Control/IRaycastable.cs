using Control;
using RPG.Control;
using UnityEngine;

namespace RPG.cotrol{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerController callingController);
        CursorType GetCursorType();
    }
}

