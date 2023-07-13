using System;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BlobTempViewModel
{
    public ReactiveProperty<float> blobTemp { get; private set; } = new ReactiveProperty<float>();   

    

    private bool onHotColdArea = false;
    private float startBlobTemp;
    private float minBlobTemp;
    private float maxBlobTemp;

    private CoroutineRunner coroutineRunner;

    private IEnumerator tempIncreaseOnArea;
    private IEnumerator tempDecreaseOnArea;
    private IEnumerator tempDecreaseFromHighTemp;

    private bool isGameEnd = false;

    public BlobTempViewModel()
    {
        coroutineRunner = CoroutineRunner.Instance;
        GlobalModel model = GlobalModel.Instance;
        startBlobTemp = GlobalModel.Instance.StartBlobTemp;
        minBlobTemp = GlobalModel.Instance.FreezingTemp;
        maxBlobTemp = GlobalModel.Instance.BoilingTemp;
        blobTemp.Value = GlobalModel.Instance.StartBlobTemp;

        blobTemp.Subscribe(_ => OnBlobTempChange());
        CheckpointManager.Instance.OnGoToLastCheckpoint += CheckpointManager_OnGoToLastCheckpoint; 
    }

    public void Update()
    {
        if (isGameEnd)
        {
            isGameEnd = false;
            GameManager.Instance.TryGameOver();
        }
    } 

    private void CheckpointManager_OnGoToLastCheckpoint(object sender, System.EventArgs e)
    {
        blobTemp.Value = CheckpointManager.Instance.BlobTempCheckpoint;
    }

    public void HotAreaTouchActions()
    {
        onHotColdArea = true;
        //Увеличение температуры
        tempIncreaseOnArea = TempIncreaseCoroutine();
        coroutineRunner.RunCoroutine(tempIncreaseOnArea);

    }

    public void HotAreaEndTouchActions()
    {
        onHotColdArea = false;
        //Завершение увеличения температуры
        if (tempIncreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
        }
        OnBlobTempChange();
    }

    private IEnumerator TempIncreaseCoroutine()
    {
        //Корутина для увеличения температуры
        while (true)
        {
            //Debug.Log("blobTemp: " + blobTemp.Value.ToString());
            blobTemp.Value += GlobalModel.Instance.BlobHeatingSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    public void ColdAreaTouchActions()
    {
        onHotColdArea = true;
        //Уменьшение температуры
        tempDecreaseOnArea = TempDecreaseOnAreaCoroutine();
        coroutineRunner.RunCoroutine(tempDecreaseOnArea);

    }

    public void ColdAreaEndTouchActions()
    {
        onHotColdArea = false;
        //Завершение уменьшения температуры
        if (tempDecreaseOnArea != null)
        {
            coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
            tempDecreaseOnArea = null;
        }
        OnBlobTempChange();
    }

    private IEnumerator TempDecreaseOnAreaCoroutine()
    {
        //Корутина для уменьшения температуры
        while (true)
        {
            blobTemp.Value -= GlobalModel.Instance.BlobCoolingSpeed;
            yield return new WaitForSeconds(GlobalModel.Instance.ModificationTime);
        }
    }

    private void OnBlobTempChange()
    {
        //Функция, которая вызывается каждый раз, когда меняется температура
        CheckMinBlobTemp();
        CheckMaxBlobTemp();
        СoolingDown(blobTemp.Value);
    }

    public void СoolingDown(float temp)
    {
        //Проверка, нужно ли уменьшать температуру, если она больше стартовой температуры(25)
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
        //Проверка, что температура не стала меньше 0. Вызывается везде, где уменьшается температура
        if (blobTemp.Value <= 0)
        {
            onHotColdArea = false;
            //Конец игры
            if (tempDecreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempDecreaseOnArea);
                tempDecreaseOnArea = null;
            }
            isGameEnd = true;
        }
    }
    private void CheckMaxBlobTemp()
    {
        //Проверка, что температура не стала больше 100. Вызывается везде, где увеличивается температура
        if (blobTemp.Value >= GlobalModel.Instance.BoilingTemp)
        {
            onHotColdArea = false;
            //Конец игры
            if (tempIncreaseOnArea != null)
            {
                coroutineRunner.StopOneCoroutine(tempIncreaseOnArea);
                tempIncreaseOnArea = null;
            }
            isGameEnd = true;
        }
    }
}
