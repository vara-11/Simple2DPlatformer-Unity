using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Animator animator;
    private Transform itemPos;

    private bool itemPicked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Player" && !itemPicked)
        {
            itemPicked = true;
            PlayerInputHandler.diamondCount++;
            itemPos = collision.transform.Find("ItemPos");
            this.transform.SetParent(itemPos);
            this.transform.localPosition = Vector3.zero;
        }
    }
}
