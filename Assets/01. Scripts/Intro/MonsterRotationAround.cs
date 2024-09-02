using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

public class MonsterRotationAround : MonoBehaviour
{
    private int _endPointZ = 55;
    private CancellationTokenSource _cancellationTokenSource;

    private void Start()
    {
        StartMonsterMoving();
    }

    private async void StartMonsterMoving()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = _cancellationTokenSource.Token;

        this.transform.localPosition = new Vector3(Random.Range(59, 75), -16, -55);
        await Task.Delay(20000);
        await this.transform.DOLocalMoveZ(_endPointZ, 30).OnComplete(() => {
            StartMonsterMoving();
        }).AsyncWaitForCompletion(cancellationToken);
    }

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }
}
