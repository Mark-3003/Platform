using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public float cameraDampening;
    
    void Update()
    {
        Vector3 _playerPos = new Vector3(player.position.x, player.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, _playerPos, cameraDampening);
    }
}
