using RPG.Attributes;
using RPG.Combat;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter _fighter;
        private Health _health;

        private void Start()
        {
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_health.IsDead()) return;
            if(InteractWithCombat()) return;
            if(InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                CombatTarget target = hit.collider.GetComponent<CombatTarget>();
                
                if(target == null) continue;

                if (_fighter.CanAttack(target.gameObject))
                {
                    if (Input.GetMouseButton(0))
                    {
                        GetComponent<Fighter>().Attack(target.gameObject);
                    }
                }
                return true;

            }
            return false;
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
            
                    
            return true;
        }

        private static Ray GetMouseRay()
        {
            if (Camera.main == null) print("camera is missing");
            return Camera.main!.ScreenPointToRay(Input.mousePosition);
        }
    }
}
