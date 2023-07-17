using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryAreaViewModel 
{
    private int moveDir;

    public DryAreaViewModel(int moveDir)
    {
        this.moveDir = moveDir; 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BlobView>().BlobMovementViewModel.DryAreaTouchActions(moveDir);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<BlobView>().BlobMovementViewModel.DryAreaEndTouchActions();
        }
    }
}
