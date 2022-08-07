using RPG.Attributes;
using RPG.Combat;
using RPG.cotrol;
using UnityEngine;
using RPG.Movement;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health _health;

        enum CursorType
        {
            None,
            Movement,
            Combat,
            OverUI
        }
        
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public UnityEngine.Vector2 hotspot;
            public Texture2D texture;
        }

        
        [SerializeField] private CursorMapping[] cursorMappings = null;
        private void Awake()
        {
            _health = GetComponent<Health>();
            GetComponent<Fighter>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (_health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.OverUI);
                return true;
            }
            return false;
        }
        public bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var component in raycastables)
                {
                    if (component.HandleRaycast(this))
                    {
                        SetCursor(CursorType.Combat);
                        return true;
                    }
                }
                return true;
            }
            return false;
        }
        

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            CursorMapping defaultCursormapping = cursorMappings[0];
            foreach (var cursorMapping in cursorMappings)
            {
                if (cursorMapping.type == type) return cursorMapping;
            }

            return defaultCursormapping;
        }

        private bool InteractWithMovement()
        {
            bool hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f );
                }
            }
            
            SetCursor(CursorType.Movement);
            return true;
        }

        private static Ray GetMouseRay()
        {
            if (Camera.main == null) print("camera is missing");
            return Camera.main!.ScreenPointToRay(Input.mousePosition);
        }

       
    }
}
