using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ReactiveProperty<int> livesCount {get; private set;} = new ReactiveProperty<int>();

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        livesCount.Value = GlobalModel.Instance.LivesCount;
    }

    public void TryGameOver()
    {
        if (livesCount.Value == 0)
        {
            GameOver();
        }

        else
        {
            LostLife();
        }
    }

    private void GameOver()
    {

    }

    private void LostLife()
    {
        livesCount.Value--;
        CheckpointManager.Instance.GoToLastCheckpoint();
    }
}
