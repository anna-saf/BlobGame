using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;

public class HotAreaViewModel
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<BlobView>().BlobTempViewModel.HotAreaTouchActions();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BlobView>().BlobTempViewModel.HotAreaEndTouchActions();
        }
    }
}
