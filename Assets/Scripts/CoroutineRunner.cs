using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("CoroutineRunner");
                _instance = go.AddComponent<CoroutineRunner>();
            }
            return _instance;
        }
    }

    // Запустить корутину
    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    // Остановить корутину
    public void StopOneCoroutine(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }
}
