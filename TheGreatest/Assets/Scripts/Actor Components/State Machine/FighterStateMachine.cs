using UnityEngine;

namespace Actor_Components.State_Machine
{
    public class FighterStateMachine : MonoBehaviour
    {
        public enum States
        {
            Idle,
            MoveToIdle,
            Sweep,
            Track,
            Evade,
            Flee
        }
    }
}
