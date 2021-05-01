using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpGrid: MonoBehaviour
{
    private GameObject[] warpGrids;
    public static bool isEnableWarp=true;
    // Start is called before the first frame update
    void Start()
    {
        warpGrids = GameObject.FindGameObjectsWithTag("WarpGrid");
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&isEnableWarp)
        {
            isEnableWarp = false;
            other.transform.position = warpGrids[Random.Range(0, warpGrids.Length)].transform.position;
            StartCoroutine(Warp());
        }
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isEnableWarp)
        {
            isEnableWarp = false;
            int randomNum = Random.Range(1, 5);
            Vector3 targetPos = warpGrids[Random.Range(0, warpGrids.Length)].transform.position;
            if (randomNum == 1)
            {
                collision.transform.position = new Vector3(targetPos.x + 1, 0, targetPos.z);
                StartCoroutine(Warp());
            }
            else if (randomNum == 2)
            {
                collision.transform.position = new Vector3(targetPos.x, 0, targetPos.z + 1);
                StartCoroutine(Warp());
            }
            else if (randomNum == 3)
            {
                collision.transform.position = new Vector3(targetPos.x - 1, 0, targetPos.z);
                StartCoroutine(Warp());
            }
            else if (randomNum == 4)
            {
                collision.transform.position = new Vector3(targetPos.x, 0, targetPos.z - 1);
                StartCoroutine(Warp());
            }
        }
    }

    IEnumerator Warp()
    {
        yield return new WaitForSeconds(0.5f);
        isEnableWarp = true;
    }
}
