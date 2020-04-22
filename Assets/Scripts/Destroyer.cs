using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float lifeTime = 5f;
    void Update()
    {
        if (lifeTime > 0)
            lifeTime -= Time.deltaTime;
        else if (lifeTime <= 0)
            Destroy(this.gameObject);
    }
}
