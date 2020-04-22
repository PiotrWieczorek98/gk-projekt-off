using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{

    void Update()
    {
        //Change rotation only in Y axis
        Vector3 destination = new Vector3(transform.parent.GetComponent<EnemyStates>().navMeshAgent.destination.x,
                                          this.transform.position.y,
                                          transform.parent.GetComponent<EnemyStates>().navMeshAgent.destination.z);

        transform.LookAt(destination);
    }
}
