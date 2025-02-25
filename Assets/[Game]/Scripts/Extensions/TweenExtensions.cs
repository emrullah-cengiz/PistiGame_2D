using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class TweenExtensions
{
    private static async UniTask TweenLerp(float duration, EaseType ease, Action<float> onUpdate, CancellationTokenSource cts = null)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            t = ApplyEase(t, ease);

            onUpdate?.Invoke(t);

            if (cts != null)
                await UniTask.Yield(PlayerLoopTiming.Update, cts.Token, cancelImmediately: true);
            else
                await UniTask.Yield(PlayerLoopTiming.Update);
            
            if(cts is { IsCancellationRequested: true })
                return;
        }

        onUpdate?.Invoke(1f);
    }

    public static async UniTask TWColor(this SpriteRenderer sprite, Color endColor, float duration, EaseType ease = EaseType.Linear,
                                        CancellationTokenSource cts = null)
    {
        var startColor = sprite.color;
        await TweenLerp(duration, ease, t => sprite.color = Color.Lerp(startColor, endColor, t), cts);
    }

    public static async UniTask TWFade(this SpriteRenderer sprite, float target, float duration, EaseType ease = EaseType.Linear,
                                       CancellationTokenSource cts = null)
    {
        var endColor = sprite.color;
        endColor.a = target;
        await TWColor(sprite, endColor, duration, ease, cts);
    }

    public static async UniTask TWFade(this CanvasGroup canvasGroup, float target, float duration, EaseType ease = EaseType.Linear,
                                       CancellationTokenSource cts = null)
    {
        float startAlpha = canvasGroup.alpha;
        await TweenLerp(duration, ease, t => canvasGroup.alpha = Mathf.Lerp(startAlpha, target, t), cts);
    }

    public static async UniTask TWLocalMove(this Transform transform, Vector3 target, float duration, EaseType ease = EaseType.Linear,
                                            CancellationTokenSource cts = null)
    {
        var startPos = transform.localPosition;
        await TweenLerp(duration, ease, t => transform.localPosition = Vector3.Lerp(startPos, target, t), cts);
    }

    public static async UniTask TWLocalRotate(this Transform transform, Vector3 target, float duration, EaseType ease = EaseType.Linear,
                                              CancellationTokenSource cts = null)
    {
        var startRot = transform.localEulerAngles;
        await TweenLerp(duration, ease, t => transform.localRotation = Quaternion.Euler(Vector3.Slerp(startRot, target, t)), cts);
    }

    public static async UniTask TWScale(this Transform transform, Vector3 target, float duration, EaseType ease = EaseType.Linear,
                                        CancellationTokenSource cts = null)
    {
        var startScale = transform.localScale;
        await TweenLerp(duration, ease, t => transform.localScale = Vector3.Lerp(startScale, target, t), cts);
    }

    public static async UniTask TWPunchScale(this Transform transform, float intensity, float duration, EaseType ease = EaseType.EaseOut,
                                             CancellationTokenSource cts = null)
    {
        var startScale = transform.localScale;
        var peakScale = startScale * (1f + intensity);

        await TweenLerp(duration / 2, ease, t => transform.localScale = Vector3.Lerp(startScale, peakScale, t), cts);
        await TweenLerp(duration / 2, ease, t => transform.localScale = Vector3.Lerp(peakScale, startScale, t), cts);
    }

    private static float ApplyEase(float t, EaseType ease)
    {
        return ease switch
        {
            EaseType.Linear => t,
            EaseType.EaseIn => t * t,
            EaseType.EaseOut => 1 - (1 - t) * (1 - t),
            EaseType.EaseInOut => t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2,
            EaseType.EaseOutQuint => 1 - Mathf.Pow(1 - t, 5),
            _ => t
        };
    }

    public enum EaseType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        EaseOutQuint
    }
}