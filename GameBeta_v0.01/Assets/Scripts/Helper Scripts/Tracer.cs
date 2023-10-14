using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracer : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public float zedPosition;
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zedPosition);
    }
}
