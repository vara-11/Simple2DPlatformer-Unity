using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform toPortposition;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.transform.name == "Player")
        {
            Invoke("SetAsEnter", 2f);
        }
    }
}
