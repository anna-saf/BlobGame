using System;
using UniRx;
using UnityEngine;

public class BlobView : MonoBehaviour
{
    public BlobTempViewModel BlobTempViewModel { get; private set; }
    public BlobVolumeViewModel BlobVolumeViewModel { get; private set; }
    //public BlobMovementViewModel1 blobMovementViewModel { get; private set; }
    public BlobMovementViewModel BlobMovementViewModel { get; private set; }

    private Vector3 blobStartScale;
    CharacterController characterController;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        blobStartScale = transform.localScale;

        BlobTempViewModel = new BlobTempViewModel();
      

        BlobVolumeViewModel = new BlobVolumeViewModel(BlobTempViewModel.blobTemp);

        //blobMovementViewModel = new BlobMovementViewModel(blobTempViewModel.blobTemp, characterController);
        BlobMovementViewModel = new BlobMovementViewModel(BlobTempViewModel.blobTemp, BlobVolumeViewModel.blobSize, characterController);

        BlobVolumeViewModel.blobSize.Subscribe(_ => ChangeBlobSize(_));
        ChangeBlobSize(BlobVolumeViewModel.blobSize.Value);        

    }

    private void Update()
    {
        BlobMovementViewModel.Update();
        BlobTempViewModel?.Update();
        BlobVolumeViewModel?.Update();
    }



    private void ChangeBlobSize(float blobSize)
    {
        transform.localScale = new Vector3(blobStartScale.x * blobSize, blobStartScale.y * blobSize, blobStartScale.z * blobSize);
    }
}
