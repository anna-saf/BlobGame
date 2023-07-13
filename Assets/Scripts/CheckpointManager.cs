
using System;
using UniRx;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private BlobView blob;
    private CheckpointView lastCheckpoint;

    public event EventHandler OnGoToLastCheckpoint;
    public event EventHandler OnGoToCheckpointComplete;

    public float BlobTempCheckpoint { get; private set; }
    public float BlobVolumeCheckpoint { get; private set; } 
    public float BlobMoveSpeedCheckpoint { get; private set; }


    public static CheckpointManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }



    public void SetLastCheckpoint(CheckpointView checkpointView)
    {
        lastCheckpoint = checkpointView;

        BlobTempCheckpoint = GlobalModel.Instance.StartBlobTemp;
        BlobVolumeCheckpoint = GlobalModel.Instance.StartBlobVolume;
        BlobMoveSpeedCheckpoint = GlobalModel.Instance.StartBlobSpeed;
    }

    public void GoToLastCheckpoint()
    {
        if (lastCheckpoint != null)
        {
            OnGoToLastCheckpoint?.Invoke(this, EventArgs.Empty);
            blob.transform.position = new Vector3(lastCheckpoint.transform.position.x, lastCheckpoint.transform.position.y + blob.transform.localScale.y, lastCheckpoint.transform.position.z);
            OnGoToCheckpointComplete?.Invoke(this,EventArgs.Empty);
        }
    }
}
