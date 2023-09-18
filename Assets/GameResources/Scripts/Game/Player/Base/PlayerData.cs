using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] MovementDataUnit movement;

        public MovementDataUnit Movement => movement;

        [System.Serializable]
        public class MovementDataUnit
        {
            [SerializeField] float speed;
            [SerializeField] float fallSpeed;


            public float Speed=>speed;
            public float FallSpeed=>fallSpeed;
        }

        //--------------------------------------------------
    }
}