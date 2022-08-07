using RPG.Attributes;
using RPG.Control;
using RPG.cotrol;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController controller)
        {
            Fighter _fighter = controller.GetComponent<Fighter>();
            if (!_fighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0))
            {
                GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}
