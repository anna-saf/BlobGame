using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryAreaView : MonoBehaviour
{
    [SerializeField]
    int moveDir;

    private DryAreaViewModel dryAreaViewModel;
    // Start is called before the first frame update
    void Start()
    {
        dryAreaViewModel = new DryAreaViewModel(moveDir);
    }

    private void OnTriggerEnter(Collider other)
    {
        dryAreaViewModel?.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        dryAreaViewModel?.OnTriggerExit(other);
    }
}
