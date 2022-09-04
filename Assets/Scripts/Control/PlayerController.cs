using System;
using Control;
using RPG.Attributes;
using RPG.Combat;
using RPG.cotrol;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CursorMapping[] cursorMappings = null;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float MaxNavPathLength = 40f;
        private Health _health;
        
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Vector2 hotspot;
            public Texture2D texture;
        }
        
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
            RaycastHit[] hits = RaycastAllSorted();
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var component in raycastables)
                {
                    if (component.HandleRaycast(this))
                    {
                        SetCursor(component.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
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

        private bool RaycastNavmesh(out Vector3 target)
        {
            target = new Vector3();
            bool hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit) return false;

            bool hasCastToNavmesh = NavMesh.SamplePosition(hit.point, out var navMeshHit, 
                maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavmesh) return false;
            
            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;

            if (GetPathLength(path) > MaxNavPathLength) return false;
            return true;
        }

        private double GetPathLength(NavMeshPath path)
        {
            float total = 0f;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        private bool InteractWithMovement()
        {
            bool hasHit = RaycastNavmesh(out var target);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f );
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
