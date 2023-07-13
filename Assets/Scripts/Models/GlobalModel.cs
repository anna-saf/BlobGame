using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GlobalModel : MonoBehaviour
{
    //Общий параметр времени, за которое изменяется значение на нужную велечину
    [SerializeField] private float modificationTime;

    [Header("----------------------")]

    //Стартовая температура капли
    [SerializeField] private float startBlobTemp;
    //Температура кипения капли
    [SerializeField] private float boilingTemp;
    //Температура замерзания
    [SerializeField] private float freezingTemp;

    [Header("----------------------")]

    //Скорость нагревания капли в секунду
    [SerializeField] private float blobHeatingSpeed;
    //Скорость охлаждения капли в секунду
    [SerializeField] private float blobCoolingSpeed;

    [Header("----------------------")]

    //Температура испарения капли
    [SerializeField] private float vaporizationTemp;
    //Скорость испарения капли в секунду
    [SerializeField] private float vaporizationVolumeSpeed;

    [Header("----------------------")]

    //Стартовый объем капли
    [SerializeField] private float startBlobVolume;
    //Стартовый размер капли
    [SerializeField] private float startBlobSize;

    [SerializeField] private float speedIncreaseFactor;
    [SerializeField] private float maxJumpHeight;
    //Стартовая высота прыжка
    [SerializeField] private float startJumpHeight;

    //Стартовая скорость капли
    [SerializeField] private float startBlobSpeed;
    //Максимальная скорость капли
    [SerializeField] private float maxBlobSpeed;
    //Минимальная скорость капли
    [SerializeField] private float minBlobSpeed;

    //Максимальный размер капли (1.5)
    [SerializeField] private float maxBlobSize;
    //Минимальный размер капли (0.5)
    [SerializeField] private float minBlobSize;
    //Значение объема капли при максимальном размере (75)
    [SerializeField] private float maxVolumeForBlobSize;
    //Значение объема капли при минимальном размере (25)
    [SerializeField] private float minVolumeForBlobSize;
    //Восполняемый объем капли от воды
    [SerializeField] private float blobVolumeFilled;

    [Header("----------------------")]
    //Скорость скольжения на сухой наклонной поверхности
    [SerializeField] private float slideSpeed;

    [Header("----------------------")]

    //Кол-во жизней на старте
    [SerializeField] private int livesCount;

    public float ModificationTime { get { return modificationTime; } }

    public float StartBlobTemp { get { return startBlobTemp; } }
    public float BoilingTemp { get { return boilingTemp; } }
    public float FreezingTemp { get { return freezingTemp; } }

    public float BlobHeatingSpeed { get { return blobHeatingSpeed; } }
    public float BlobCoolingSpeed { get { return blobCoolingSpeed; } }
    public float VaporizationTemp { get { return vaporizationTemp; } }
    public float VaporizationVolumeSpeed { get { return vaporizationVolumeSpeed; } }

    public float SpeedIncreaseFactor { get { return speedIncreaseFactor; } }
    public float MaxJumpHeight { get { return maxJumpHeight; } }
    public float StartBlobVolume { get { return startBlobVolume; } }
    public float StartBlobSize{ get { return startBlobSize; } }
    public float StartJumpHeight { get { return startJumpHeight; } }
    public float StartBlobSpeed { get { return startBlobSpeed; } }
    public float MaxBlobSpeed { get { return maxBlobSpeed; } }
    public float MinBlobSpeed { get { return minBlobSpeed; } }
    public float MaxBlobSize { get { return maxBlobSize; } }
    public float MinBlobSize { get { return minBlobSize; } }
    public float MaxVolumeForBlobSize { get { return maxVolumeForBlobSize; } }
    public float MinVolumeForBlobSize { get { return minVolumeForBlobSize; } }
    public float BlobVolumeFilled { get { return blobVolumeFilled; } }

    public float SlideSpeed { get { return slideSpeed; } }
    public int LivesCount { get { return livesCount; } }

    public static GlobalModel Instance;

    private void Awake()
    {
        Instance = this;
    }


}
