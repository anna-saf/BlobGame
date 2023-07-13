using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdAreaView : MonoBehaviour
{
    private ColdAreaViewModel coldAreaViewModel;
    // Start is called before the first frame update
    void Start()
    {
        coldAreaViewModel = new ColdAreaViewModel();
    }

    private void OnTriggerEnter(Collider other)
    {
        coldAreaViewModel?.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        coldAreaViewModel?.OnTriggerExit(other);
    }
}
