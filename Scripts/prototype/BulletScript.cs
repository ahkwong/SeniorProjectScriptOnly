using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    void Start()
    {
        Destroy(gameObject, 3);
    }
	void OnTriggerEnter(Collider col)
    {
        if(col.tag=="Monster")
        {
            Destroy(gameObject);
        }
    }
}
