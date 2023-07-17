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
        //Функция, которая вызывается каждый раз, когда меняется температура
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
        //Функция вызывается при касании с водой
        blobVolumeValue += GlobalModel.Instance.BlobVolumeFilled;
        OnBlobVolumeChange();
    }

    private void CheckVaporizeTemp(float blobTemp)
    {
        //Функция вызывается каждый раз при изменении температуры. Проверяет, нужно ли уменьшать объем капли сейчас.
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
        //Проверка, что объем не стал меньше 0. Вызывается везде, где уменьшается объем
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
        //Проверка, что объем не стал больше 100. Вызывается везде, где увеличивается температура
        if (blobVolumeValue > GlobalModel.Instance.MaxBlobVolume)
        {
            blobVolumeValue = GlobalModel.Instance.MaxBlobVolume;
        }
    }

    private void CheckSizeChangeFromVolume(float blobVolume)
    {
        //Функция для пересчета размера капли в зависимости от объема
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
        //Корутина для уменьшения объема капли
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
