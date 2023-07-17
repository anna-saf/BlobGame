using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWallView : MonoBehaviour
{
    private WaterWallViewModel waterWallViewModel;
    // Start is called before the first frame update
    void Start()
    {
        waterWallViewModel = new WaterWallViewModel();
    }

    private void OnTriggerEnter(Collider other)
    {
        waterWallViewModel?.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        waterWallViewModel?.OnTriggerExit(other);
    }
}
