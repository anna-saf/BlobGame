using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWallViewModel 
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BlobView>().BlobMovementViewModel.WaterWallTouchAction();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BlobView>().BlobMovementViewModel.WaterWallEndTouchAction();
        }
    }
}
