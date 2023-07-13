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
        //Функция, которая вызывается каждый раз, когда меняется температура
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
        //Функция вызывается при касании с водой
        blobVolume.Value += GlobalModel.Instance.BlobVolumeFilled;
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
        if(blobVolume.Value <= 0)
        {
            //blobVolume.Value = GlobalModel.Instance.StartBlobVolume;//Конец игры
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
        //Проверка, что объем не стал больше 100. Вызывается везде, где увеличивается температура
        if (blobVolume.Value >= 100)
        {
            blobVolume.Value = 100;
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
            blobVolume.Value -= GlobalModel.Instance.VaporizationVolumeSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }
}
