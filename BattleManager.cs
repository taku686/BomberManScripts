using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BattleManager : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
       
            GManager.Instance.PlayerInstantiate();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
