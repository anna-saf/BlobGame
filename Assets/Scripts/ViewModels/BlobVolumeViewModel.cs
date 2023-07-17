using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobVolumeViewModel
{
    public ReactiveProperty<float> blobVolume { get; private set; } = new ReactiveProperty<float>();
    private float blobVolumeValue;

    public ReactiveProperty<float> blobSize { get; private set; } = new ReactiveProperty<float>();

    private IEnumerator volumeDecreaseFromHighTemp;

    private CoroutineRunner coroutineRunner;

    public BlobVolumeViewModel(ReactiveProperty<float> blobTemp)
    {
        coroutineRunner = CoroutineRunner.Instance;
        blobSize.Value = GlobalModel.Instance.StartBlobSize;
        blobVolume.Value = GlobalModel.Instance.StartBlobVolume;
        blobVolumeValue = blobVolume.Value;
        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManager_OnGoToLastCheckpoint;
        blobTemp.Subscribe(_ => CheckVaporizeTemp(_));
    }

    private void OnBlobVolumeChange()
    {
        //�������, ������� ���������� ������ ���, ����� �������� �����������
        CheckMinBlobVolume();
        CheckMaxBlobVolume();
        ChangeBlobVolumeReactiveProperty(blobVolumeValue);
        CheckSizeChangeFromVolume(blobVolumeValue);
    }

    private void CheckpointManager_OnGoToLastCheckpoint(object sender, System.EventArgs e)
    {
        blobVolumeValue = CheckpointManager.Instance.BlobVolumeCheckpoint; 
        if (volumeDecreaseFromHighTemp != null)
        {
            coroutineRunner.StopOneCoroutine(volumeDecreaseFromHighTemp);
            volumeDecreaseFromHighTemp = null;
        }
        OnBlobVolumeChange();
    }

    public void WaterTouchActions()
    {
        //������� ���������� ��� ������� � �����
        blobVolumeValue += GlobalModel.Instance.BlobVolumeFilled;
        OnBlobVolumeChange();
    }

    private void CheckVaporizeTemp(float blobTemp)
    {
        //������� ���������� ������ ��� ��� ��������� �����������. ���������, ����� �� ��������� ����� ����� ������.
        if(blobTemp >= GlobalModel.Instance.VaporizationTemp && blobTemp < GlobalModel.Instance.BoilingTemp && volumeDecreaseFromHighTemp == null && blobVolume.Value > 0)
        {
            volumeDecreaseFromHighTemp = VolumeDecreaseCoroutine();
            coroutineRunner.RunCoroutine(volumeDecreaseFromHighTemp);
        }
        else if ((blobTemp < GlobalModel.Instance.VaporizationTemp && volumeDecreaseFromHighTemp != null))
        {
            coroutineRunner.StopOneCoroutine(volumeDecreaseFromHighTemp);
            volumeDecreaseFromHighTemp = null;
        }
    }

    private void CheckMinBlobVolume()
    {
        //��������, ��� ����� �� ���� ������ 0. ���������� �����, ��� ����������� �����
        if(blobVolumeValue <= GlobalModel.Instance.MinBlobVolume)
        {
            if(volumeDecreaseFromHighTemp != null)
            {
                coroutineRunner.StopOneCoroutine(volumeDecreaseFromHighTemp);
                volumeDecreaseFromHighTemp = null;
            }
            GameManager.Instance.TryGameOver();
        }
    }

    private void CheckMaxBlobVolume()
    {
        //��������, ��� ����� �� ���� ������ 100. ���������� �����, ��� ������������� �����������
        if (blobVolumeValue > GlobalModel.Instance.MaxBlobVolume)
        {
            blobVolumeValue = GlobalModel.Instance.MaxBlobVolume;
        }
    }

    private void CheckSizeChangeFromVolume(float blobVolume)
    {
        //������� ��� ��������� ������� ����� � ����������� �� ������
        if (blobVolume >= GlobalModel.Instance.MaxVolumeForBlobSize)
        {
            blobSize.Value = GlobalModel.Instance.MaxBlobSize;
        }
        else if (blobVolume <= GlobalModel.Instance.MinVolumeForBlobSize)
        {
            blobSize.Value = GlobalModel.Instance.MinBlobSize;
        }
        else
        {
            blobSize.Value = blobVolume / GlobalModel.Instance.StartBlobVolume;
        }        
    }

    private IEnumerator VolumeDecreaseCoroutine()
    {
        //�������� ��� ���������� ������ �����
        while (true)
        {
            blobVolumeValue -= GlobalModel.Instance.VaporizationVolumeSpeed;
            OnBlobVolumeChange();
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    private void ChangeBlobVolumeReactiveProperty(float blobVolumeVal)
    {
        blobVolume.Value = blobVolumeVal;
    }
}
