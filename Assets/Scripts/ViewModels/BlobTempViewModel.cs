using System;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobTempViewModel
{
    public ReactiveProperty<float> blobTemp { get; private set; } = new ReactiveProperty<float>();
    private float blobTempValue;

    private bool onHotColdArea = false;

    private CoroutineRunner coroutineRunner;

    private IEnumerator tempIncreaseOnArea;
    private IEnumerator tempDecreaseOnArea;
    private IEnumerator tempDecreaseFromHighTemp;

    public BlobTempViewModel()
    {
        coroutineRunner = CoroutineRunner.Instance;
        GlobalModel model = GlobalModel.Instance;
        blobTemp.Value = GlobalModel.Instance.StartBlobTemp;
        blobTempValue = blobTemp.Value;

        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManager_OnGoToLastCheckpoint; 
    }

    private void CheckpointManager_OnGoToLastCheckpoint(object sender, System.EventArgs e)
    {
        blobTempValue = CheckpointManager.Instance.BlobTempCheckpoint;
        if (tempDecreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
            tempDecreaseOnArea = null;
        }
        if (tempIncreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
            tempIncreaseOnArea = null;
        }
        OnBlobTempChange();
    }

    public void HotAreaTouchActions()
    {
        onHotColdArea = true;
        //���������� �����������
        tempIncreaseOnArea = TempIncreaseCoroutine();
        coroutineRunner.RunCoroutine(tempIncreaseOnArea);

    }

    public void HotAreaEndTouchActions()
    {
        onHotColdArea = false;
        //���������� ���������� �����������
        if (tempIncreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
        }
        OnBlobTempChange();
    }

    private IEnumerator TempIncreaseCoroutine()
    {
        //�������� ��� ���������� �����������
        while (true)
        {
            blobTempValue += GlobalModel.Instance.BlobHeatingSpeed;
            OnBlobTempChange();
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    public void ColdAreaTouchActions()
    {
        onHotColdArea = true;
        //���������� �����������
        tempDecreaseOnArea = TempDecreaseOnAreaCoroutine();
        coroutineRunner.RunCoroutine(tempDecreaseOnArea);

    }

    public void ColdAreaEndTouchActions()
    {
        onHotColdArea = false;
        //���������� ���������� �����������
        if (tempDecreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
            tempDecreaseOnArea = null;
        }
        OnBlobTempChange();
    }

    private IEnumerator TempDecreaseOnAreaCoroutine()
    {
        //�������� ��� ���������� �����������
        while (true)
        {
            blobTempValue -= GlobalModel.Instance.BlobCoolingSpeed;
            OnBlobTempChange();
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    private void OnBlobTempChange()
    {
        //�������, ������� ���������� ������ ���, ����� �������� �����������
        CheckMinBlobTemp();
        CheckMaxBlobTemp();
        �oolingDown(blobTempValue);
        ChangeBlobVolumeReactiveProperty(blobTempValue);
    }

    public void �oolingDown(float temp)
    {
        //��������, ����� �� ��������� �����������, ���� ��� ������ ��������� �����������(25)
        if (!onHotColdArea)
        {
            if (temp > GlobalModel.Instance.StartBlobTemp && tempDecreaseFromHighTemp == null)
            {
                tempDecreaseFromHighTemp = TempDecreaseOnAreaCoroutine();
                coroutineRunner.RunCoroutine(tempDecreaseFromHighTemp);
            }
            else if(temp <= GlobalModel.Instance.StartBlobTemp && tempDecreaseFromHighTemp != null )
            {
                coroutineRunner.StopOneCoroutine(tempDecreaseFromHighTemp);
                tempDecreaseFromHighTemp = null;
            }
        }
        else if(tempDecreaseFromHighTemp != null)
        {
            coroutineRunner.StopOneCoroutine(tempDecreaseFromHighTemp);
            tempDecreaseFromHighTemp = null;
        }
    }

    private void CheckMinBlobTemp()
    {
        //��������, ��� ����������� �� ����� ������ 0. ���������� �����, ��� ����������� �����������
        if (blobTempValue <= GlobalModel.Instance.FreezingTemp)
        {
            onHotColdArea = false;
            //����� ����
            if (tempDecreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
                tempDecreaseOnArea = null;
            }
            GameManager.Instance.TryGameOver();
        }
    }
    private void CheckMaxBlobTemp()
    {
        //��������, ��� ����������� �� ����� ������ 100. ���������� �����, ��� ������������� �����������
        if (blobTempValue >= GlobalModel.Instance.BoilingTemp)
        {
            onHotColdArea = false;
            //����� ����
            if (tempIncreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
                tempIncreaseOnArea = null;
            }
            GameManager.Instance.TryGameOver();
        }
    }
    private void ChangeBlobVolumeReactiveProperty(float blobTempVal)
    {
        blobTemp.Value = blobTempVal;
    }
}
