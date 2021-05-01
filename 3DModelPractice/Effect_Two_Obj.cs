using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Two_Obj : MonoBehaviour
{
    GameObject Player;
    float speed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.rotation.eulerAngles.y == 0)
        {
            transform.position += new Vector3(0, 0, speed);
        }
        else if (Player.transform.rotation.eulerAngles.y == 90)
        {
            transform.position += new Vector3(speed, 0, 0);
        }
        else if (Player.transform.rotation.eulerAngles.y == 180)
        {
            transform.position += new Vector3(0, 0, -speed);
        }
        else if (Player.transform.rotation.eulerAngles.y == 270)
        {
            transform.position += new Vector3(-speed, 0, 0);
        }
    }
}
