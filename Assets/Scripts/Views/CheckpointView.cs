using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointView : MonoBehaviour
{
    private CheckpointViewModel checkpointViewModel;
    // Start is called before the first frame update
    void Start()
    {
        checkpointViewModel = new CheckpointViewModel();
    }
         
    private void OnTriggerEnter(Collider other)
    {
        checkpointViewModel?.OnTriggerEnter(other, this);
    }
}
