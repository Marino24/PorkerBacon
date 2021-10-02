using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyThisGuy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       if(GameObject.FindGameObjectsWithTag("MainMusic").Length > 1)
       {
           Destroy(gameObject);
       } 
       DontDestroyOnLoad(gameObject); 
    }

}
