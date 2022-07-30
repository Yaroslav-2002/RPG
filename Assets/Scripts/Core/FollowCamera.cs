using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private Vector3 Startposition { get; set; }
        private Vector3 PlayerStartposition { get; set; }

        private void Awake()
        {
            Startposition = transform.position;
            PlayerStartposition = player.position;
        }  
        
        private void LateUpdate()
        {
            var position = player.position;
            transform.position = new Vector3((Startposition.x+ + position.x-  PlayerStartposition.x),(Startposition.y+ + position.y-PlayerStartposition.y),(Startposition.z+ + position.z - PlayerStartposition.z) );
        }
    }
}
