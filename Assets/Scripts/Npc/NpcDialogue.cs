using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
    private BoxCollider boxCollider;
    private Transform Canvas;

    public static bool IsEnter;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        Canvas = transform.Find("Canvas_Npc");
    }

    private void OnTriggerStay(Collider other) {

        if(other.CompareTag("Player")){
            Canvas.gameObject.SetActive(true);
            IsEnter = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            Canvas.gameObject.SetActive(false);
            IsEnter = false;
        }
    }
}
