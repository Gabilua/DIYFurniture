using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole")
        {
            if (Vector3.Angle(transform.up, other.transform.up) < 1)
            {
                if (!GameManager.instance.canMisplace)
                {
                    if (GetComponentInParent<Piece>().type == other.GetComponent<ScrewHole>().type)
                    {
                        GetComponentInParent<Piece>().ScrewIn(other.transform, transform);
                        other.enabled = false;
                    }
                }
                else
                {
                    GetComponentInParent<Piece>().ScrewIn(other.transform, transform);
                    other.enabled = false;
                }
            }
        }
    }
}
