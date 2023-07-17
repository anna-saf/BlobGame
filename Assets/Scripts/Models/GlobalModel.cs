using NaughtyAttributes;//Библиотека для управления параметрами скрипта в инспекторе https://assetstore.unity.com/packages/tools/utilities/naughtyattributes-129996
using UnityEngine;

public class GlobalModel : MonoBehaviour
{
    
    [Header("Общий параметр времени, за которое изменяется значение на нужную велечину")]
    [SerializeField] private float modificationTime;

    [Header("----------------------")]

    [Header("Стартовая температура капли")]
    [SerializeField] private float startBlobTemp;
    
    [Header("Температура кипения капли")]
    [SerializeField] private float boilingTemp;
    
    [Header("Температура замерзания")]
    [SerializeField] private float freezingTemp;

    [Header("----------------------")]

    
    [Header("Скорость нагревания капли в секунду")]
    [SerializeField] private float blobHeatingSpeed;
    
    [Header("Скорость охлаждения капли в секунду")]
    [SerializeField] private float blobCoolingSpeed;

    [Header("----------------------")]

    
    [Header("Температура испарения капли")]
    [SerializeField] private float vaporizationTemp;
    
    [Header("Скорость испарения капли в секунду")]
    [SerializeField] private float vaporizationVolumeSpeed;

    [Header("----------------------")]

    
    [Header("Стартовый объем капли")]
    [SerializeField] private float startBlobVolume;

    [Header("Максимальный объем капли")]
    [SerializeField] private float maxBlobVolume;

    [Header("Минимальный объем капли")]
    [SerializeField] private float minBlobVolume;

    [Header("Стартовый размер капли")]
    [SerializeField] private float startBlobSize;

    [Header("----------------------")]

    [Header("Стартовая высота прыжка")]
    [SerializeField] private float startJumpHeight;

    [Header("----------------------")]

    [Header("Стартовая скорость капли")]
    [SerializeField] private float startBlobSpeed;

    [Header("Максимальная скорость капли")]
    [SerializeField] private float maxBlobSpeed;
    
    [Header("Минимальная скорость капли")]
    [SerializeField] private float minBlobSpeed;

    [Header("----------------------")]

    [Header("Максимальный размер капли (1.5)")]
    [SerializeField] private float maxBlobSize;
    [Header("Минимальный размер капли (0.5)")]   
    [SerializeField] private float minBlobSize;

    [Header("Значение объема капли при максимальном размере (75)")]
    [SerializeField] private float maxVolumeForBlobSize;
    [Header("Значение объема капли при минимальном размере (25)")]
    [SerializeField] private float minVolumeForBlobSize;
    [Header("Восполняемый объем капли от воды")]
    [SerializeField] private float blobVolumeFilled;

    [Header("----------------------")]

    [Header("Скорость скольжения на сухой наклонной поверхности")]
    [SerializeField] private float slideSpeed;

    [Header("Угол наклона поверхности, по которому капля может забираться")]
    [SerializeField] private float slopeLimit;

    [Header("----------------------")]

    [Header("Кол-во жизней на старте")]
    [SerializeField] private int livesCount;

    public float ModificationTime { get { return modificationTime; } }

    public float StartBlobTemp { get { return startBlobTemp; } }
    public float BoilingTemp { get { return boilingTemp; } }
    public float FreezingTemp { get { return freezingTemp; } }

    public float BlobHeatingSpeed { get { return blobHeatingSpeed; } }
    public float BlobCoolingSpeed { get { return blobCoolingSpeed; } }
    public float VaporizationTemp { get { return vaporizationTemp; } }
    public float VaporizationVolumeSpeed { get { return vaporizationVolumeSpeed; } }

    public float StartBlobVolume { get { return startBlobVolume; } }
    public float MaxBlobVolume { get { return maxBlobVolume; } }
    public float MinBlobVolume { get { return minBlobVolume; } }
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
    public float SlopeLimit { get { return slopeLimit; } }

    public int LivesCount { get { return livesCount; } }

    public static GlobalModel Instance;

    private void Awake()
    {
        Instance = this;
    }


}
