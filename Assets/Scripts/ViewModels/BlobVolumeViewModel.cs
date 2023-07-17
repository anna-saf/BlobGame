using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobVolumeViewModel
{
    public ReactiveProperty<float> blobVolume { get; private set; } = new ReactiveProperty<float>();

    public ReactiveProperty<float> blobSize { get; private set; } = new ReactiveProperty<float>();

    private IEnumerator volumeDecreaseFromHighTemp;

    private CoroutineRunner coroutineRunner;

    private bool isGameEnd = false;

    public BlobVolumeViewModel(ReactiveProperty<float> blobTemp)
    {
        coroutineRunner = CoroutineRunner.Instance;
        blobSize.Value = GlobalModel.Instance.StartBlobSize;
        blobVolume.Value = GlobalModel.Instance.StartBlobVolume;
        blobVolume.Subscribe(_ => OnBlobVolumeChange(_));
        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManager_OnGoToLastCheckpoint;
        blobTemp.Subscribe(_ => CheckVaporizeTemp(_));
    }


    public void Update()
    {
        if (isGameEnd)
        {
            isGameEnd = false;
            GameManager.Instance.TryGameOver();
        }
    }
    private void OnBlobVolumeChange(float volume)
    {
        //�������, ������� ���������� ������ ���, ����� �������� �����������
        CheckMinBlobVolume();
        CheckMaxBlobVolume();
        CheckSizeChangeFromVolume(volume);
    }

    private void CheckpointManager_OnGoToLastCheckpoint(object sender, System.EventArgs e)
    {
        blobVolume.Value = CheckpointManager.Instance.BlobVolumeCheckpoint;
    }

    public void WaterTouchActions()
    {
        //������� ���������� ��� ������� � �����
        blobVolume.Value += GlobalModel.Instance.BlobVolumeFilled;
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
        if(blobVolume.Value <= GlobalModel.Instance.MinBlobVolume)
        {
            //blobVolume.Value = GlobalModel.Instance.StartBlobVolume;//����� ����
            if(volumeDecreaseFromHighTemp != null)
            {
                coroutineRunner.StopOneCoroutine(volumeDecreaseFromHighTemp);
                volumeDecreaseFromHighTemp = null;
            }
            isGameEnd = true;
        }
    }

    private void CheckMaxBlobVolume()
    {
        //��������, ��� ����� �� ���� ������ 100. ���������� �����, ��� ������������� �����������
        if (blobVolume.Value >= GlobalModel.Instance.MaxBlobVolume)
        {
            blobVolume.Value = 100;
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
            blobVolume.Value -= GlobalModel.Instance.VaporizationVolumeSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }
}
