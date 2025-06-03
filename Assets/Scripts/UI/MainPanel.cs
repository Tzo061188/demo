using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Image hpImage;
    private RectTransform tiredCount;
    public PlayerHealthSystem playerHealthSystem;
    private void Awake() {
        hpImage = gameObject.transform.Find("HP/HP_Count").GetComponent<Image>();
        tiredCount = gameObject.transform.Find("Tired/Tired_Count").GetComponent<RectTransform>();
       
    }
    private void Start() {
        playerHealthSystem = GameManager.Instance.player.GetComponent<PlayerHealthSystem>(); 
    }
    // Update is called once per frame
    Vector3 value;
    void Update()
    {   
        if(playerHealthSystem != null){

            value.Set(playerHealthSystem.GetTiredScale(),tiredCount.localScale.y,tiredCount.localScale.z);
            hpImage.fillAmount = playerHealthSystem.GetHPScale();
            tiredCount.localScale = value;
        }
    }
}
