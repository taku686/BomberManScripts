using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class ObjectsMove : MonoBehaviour
{
    [SerializeField] LocalClock localClock;
    private GlobalClock globalClock;
     // Start is called before the first frame update
    void Start()
    {
        globalClock = GameObject.Find("Timekeeper").GetComponent<GlobalClock>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Mathf.Sin(Time.realtimeSinceStartup) * 0.03f*localClock.localTimeScale*globalClock.localTimeScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FreezeEffect"))
        {
            StartCoroutine(FreezeMove());
        }
    }

    IEnumerator FreezeMove()
    {
        localClock.localTimeScale = 0;
        yield return new WaitForSeconds(2);
        localClock.localTimeScale = 1;
    }
}
