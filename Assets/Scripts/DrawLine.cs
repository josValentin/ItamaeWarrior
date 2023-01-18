using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Transform Origen;
    public Transform Destino;

    void OnDrawGizmosSelected(){

        if(Origen != null && Destino != null){

            Gizmos.color = Color.red;
            Gizmos.DrawLine(Origen.position, Destino.position);
            Gizmos.DrawSphere(Origen.position, 0.15f);
            Gizmos.DrawSphere(Destino.position, 0.15f);

        }

    }
}
