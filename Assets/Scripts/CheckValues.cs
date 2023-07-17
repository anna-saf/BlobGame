using UniRx;
using TMPro;
using UnityEngine;

public class CheckValues : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tempText;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private TextMeshProUGUI sizeText;

    [SerializeField] private BlobView blob;

    private void Start()
    {
        ReactiveProperty<float> blolbTemp = blob.BlobTempViewModel.blobTemp;
        blob.BlobTempViewModel.blobTemp.Subscribe(_ =>
        {
            tempText.text = "Temp: " + _.ToString();
        });
        blob.BlobVolumeViewModel.blobVolume.Subscribe(_ =>
        {
            volumeText.text = "Volume: " + _.ToString();
        });
        blob.BlobVolumeViewModel.blobSize.Subscribe(_ =>
        {
            sizeText.text = "Size: " + _.ToString();
        });
    }
}
