using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;

public static class DOTweenExtensions
{
    public static async Task AsyncWaitForCompletion(this Tween tween, CancellationToken cancellationToken)
    {
        while (tween.active && !tween.IsComplete())
        {
            if (cancellationToken.IsCancellationRequested)
            {
                tween.Kill(); 
                cancellationToken.ThrowIfCancellationRequested();
            }
            await Task.Yield(); 
        }
    }
}