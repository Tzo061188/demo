using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyDown : MonoBehaviour
{
    private bool isAnykeydown = false;
    private GameObject buttonGroup;
    // Start is called before the first frame update
    void Start()
    {
        buttonGroup = this.transform.parent.Find("ButtonGroup").gameObject;
    }

    // Update is called once per frame


}
