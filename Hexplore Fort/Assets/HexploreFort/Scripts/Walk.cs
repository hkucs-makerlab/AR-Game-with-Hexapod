using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(transform.GetChild(0).GetChild(0).forward.x * 0.1f, 0, transform.GetChild(0).GetChild(0).forward.z * 0.1f);
        }
    }
}
