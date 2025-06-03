using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private void OnTriggerStay(Collider other) {

        if(this.gameObject.activeSelf == true){
            if(Input.GetKeyDown(KeyCode.F) && other.CompareTag("Player")){

                SceneLoadManager.Instance.AsyncLoadScene("Game01",()=>{               
                    GameManager.Instance.CreatePlayer();
                });
                
            }
        }    
    }

    
}
