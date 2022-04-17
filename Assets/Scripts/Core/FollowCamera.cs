using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform player;
   
        private void LateUpdate()
        {
            transform.position = player.position;
      
        }
    }
}
