using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointViewModel
{

    public void OnTriggerEnter(Collider other, CheckpointView checkpointView)
    {
        if (other.tag == "Player")
        {
            CheckpointManager.Instance.SetLastCheckpoint(checkpointView);
        }
    }


}
