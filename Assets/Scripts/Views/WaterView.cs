using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterView : MonoBehaviour
{
    private WaterViewModel waterViewModel;
    // Start is called before the first frame update
    void Start()
    {
        waterViewModel = new WaterViewModel();
    }

    private void OnTriggerEnter(Collider other)
    {
        waterViewModel?.OnTriggerEnter(other);
    }
}
