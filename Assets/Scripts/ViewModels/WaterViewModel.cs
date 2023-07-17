using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterViewModel
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BlobView>().BlobVolumeViewModel.WaterTouchActions();
           
        }
    }
}
