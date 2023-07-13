using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryAreaView1 : MonoBehaviour
{
    private bool isMustSlide = false;
    private CharacterController _playerColl;
    [SerializeField]
    private float speed = 0.05f;
    [SerializeField]
    int chMoveDir; 

    private void Update()
    {
      //  print(gameObject.transform.localRotation.eulerAngles);

        if (isMustSlide)
        {
            Slide();
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            isMustSlide = true;
           _playerColl = other.GetComponent<CharacterController>();
          //  print("pl");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isMustSlide = false;
    }

    void Slide()
    {
        _playerColl.Move(new Vector3(speed * Time.deltaTime * chMoveDir, 0, 0));
       // print("sliding");
    }



}
