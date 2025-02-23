using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class TweenExtensions
{
    private static async UniTask TweenLerp(float duration, EaseType ease, Action<float> onUpdate)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            t = ApplyEase(t, ease);

            onUpdate?.Invoke(t);

            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        onUpdate?.Invoke(1f);
    }

    public static async UniTask TWColor(this SpriteRenderer sprite, Color endColor, float duration, EaseType ease = EaseType.Linear)
    {
        var startColor = sprite.color;
        await TweenLerp(duration, ease, t => sprite.color = Color.Lerp(startColor, endColor, t));
    }

    public static async UniTask TWFade(this SpriteRenderer sprite, float target, float duration, EaseType ease = EaseType.Linear)
    {
        var endColor = sprite.color;
        endColor.a = target;
        await TWColor(sprite, endColor, duration, ease);
    }

    public static async UniTask TWFade(this CanvasGroup canvasGroup, float target, float duration, EaseType ease = EaseType.Linear)
    {
        float startAlpha = canvasGroup.alpha;
        await TweenLerp(duration, ease, t => canvasGroup.alpha = Mathf.Lerp(startAlpha, target, t));
    }

    public static async UniTask TWMove(this Transform transform, Vector3 target, float duration, EaseType ease = EaseType.Linear)
    {
        var startPos = transform.position;
        await TweenLerp(duration, ease, t => transform.position = Vector3.Lerp(startPos, target, t));
    }

    public static async UniTask TWScale(this Transform transform, Vector3 target, float duration, EaseType ease = EaseType.Linear)
    {
        var startScale = transform.localScale;
        await TweenLerp(duration, ease, t => transform.localScale = Vector3.Lerp(startScale, target, t));
    }

    public static async UniTask TWPunchScale(this Transform transform, float intensity, float duration, EaseType ease = EaseType.EaseOut)
    {
        var startScale = transform.localScale;
        var peakScale = startScale * (1f + intensity);

        await TweenLerp(duration / 2, ease, t => transform.localScale = Vector3.Lerp(startScale, peakScale, t));
        await TweenLerp(duration / 2, ease, t => transform.localScale = Vector3.Lerp(peakScale, startScale, t));
    }

    public static async UniTask TWRotate(this Transform transform, Quaternion target, float duration, EaseType ease = EaseType.Linear)
    {
        var startRot = transform.rotation;
        await TweenLerp(duration, ease, t => transform.rotation = Quaternion.Slerp(startRot, target, t));
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