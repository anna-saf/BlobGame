using NaughtyAttributes;//���������� ��� ���������� ����������� ������� � ���������� https://assetstore.unity.com/packages/tools/utilities/naughtyattributes-129996
using UnityEngine;

public class GlobalModel : MonoBehaviour
{
    
    [Header("����� �������� �������, �� ������� ���������� �������� �� ������ ��������")]
    [SerializeField] private float modificationTime;

    [Header("----------------------")]

    [Header("��������� ����������� �����")]
    [SerializeField] private float startBlobTemp;
    
    [Header("����������� ������� �����")]
    [SerializeField] private float boilingTemp;
    
    [Header("����������� ����������")]
    [SerializeField] private float freezingTemp;

    [Header("----------------------")]

    
    [Header("�������� ���������� ����� � �������")]
    [SerializeField] private float blobHeatingSpeed;
    
    [Header("�������� ���������� ����� � �������")]
    [SerializeField] private float blobCoolingSpeed;

    [Header("----------------------")]

    
    [Header("����������� ��������� �����")]
    [SerializeField] private float vaporizationTemp;
    
    [Header("�������� ��������� ����� � �������")]
    [SerializeField] private float vaporizationVolumeSpeed;

    [Header("----------------------")]

    
    [Header("��������� ����� �����")]
    [SerializeField] private float startBlobVolume;

    [Header("������������ ����� �����")]
    [SerializeField] private float maxBlobVolume;

    [Header("����������� ����� �����")]
    [SerializeField] private float minBlobVolume;

    [Header("��������� ������ �����")]
    [SerializeField] private float startBlobSize;

    [Header("----------------------")]

    [Header("��������� ������ ������")]
    [SerializeField] private float startJumpHeight;

    [Header("----------------------")]

    [Header("��������� �������� �����")]
    [SerializeField] private float startBlobSpeed;

    [Header("������������ �������� �����")]
    [SerializeField] private float maxBlobSpeed;
    
    [Header("����������� �������� �����")]
    [SerializeField] private float minBlobSpeed;

    [Header("----------------------")]

    [Header("������������ ������ ����� (1.5)")]
    [SerializeField] private float maxBlobSize;
    [Header("����������� ������ ����� (0.5)")]   
    [SerializeField] private float minBlobSize;

    [Header("�������� ������ ����� ��� ������������ ������� (75)")]
    [SerializeField] private float maxVolumeForBlobSize;
    [Header("�������� ������ ����� ��� ����������� ������� (25)")]
    [SerializeField] private float minVolumeForBlobSize;
    [Header("������������ ����� ����� �� ����")]
    [SerializeField] private float blobVolumeFilled;

    [Header("----------------------")]

    [Header("�������� ���������� �� ����� ��������� �����������")]
    [SerializeField] private float slideSpeed;

    [Header("���� ������� �����������, �� �������� ����� ����� ����������")]
    [SerializeField] private float slopeLimit;

    [Header("----------------------")]

    [Header("���-�� ������ �� ������")]
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
