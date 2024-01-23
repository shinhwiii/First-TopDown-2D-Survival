using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = this.player.transform.position; // 플레이어가 자리를 바꿀 때마다 따라가겠다는 뜻
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z); // 플레이어의 x와 y의 위치를 따라감
    }
}
