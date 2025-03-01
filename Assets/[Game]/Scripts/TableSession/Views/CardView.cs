using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class CardView : MonoBehaviour, IInitializablePoolable<CardData>, IPointerDownHandler
{
    [Inject] private readonly CardSettings _cardSettings;
    [Inject] private readonly TableSession _tableSession;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _collider;

    private CardData Data;
    private bool IsClosed;
    private Vector3 _defaultScale;

    public void OnCreated()
    {
        _defaultScale = transform.localScale;
        _collider.enabled = false;
    }

    public void OnSpawned(CardData data, params object[] additionalArgs)
    {
        Data = data;

        IsClosed = additionalArgs.Length > 0 && (bool)additionalArgs[0];

        RefreshView();
    }

    public void OnDespawned()
    {
    }

    private void RefreshView()
    {
        _spriteRenderer.sortingOrder = _tableSession.NextCardSortingOrder;

        _spriteRenderer.sprite = !IsClosed
            ? _cardSettings.CardDataSprites[Data]
            : _cardSettings.ClosedCardSprite;
    }


    public void OnPointerDown(PointerEventData eventData) => Event.OnCardSelected.Invoke(Data);

    public void SetInteractable(bool value) => _collider.enabled = value;


    private CancellationTokenSource _cts;

    public async UniTask MoveTo((Vector3 pos, Vector3 angles, Vector3 scale) target, bool isClosed, float? duration = null, CancellationTokenSource cts = null)
    {
        _cts?.Cancel();
        _cts = cts ?? new CancellationTokenSource();

        IsClosed = isClosed;

        RefreshView();

        var moveDuration = duration ?? _cardSettings.GeneralMoveDuration;

        //Shortest rotation for opposite direction
        if (Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.z, target.angles.z)) > 90)
            target.angles.z += 180;

        transform.TWScale(target.scale, moveDuration, TweenExtensions.EaseType.EaseOutQuint, _cts).Forget();
        transform.TWLocalRotate(target.angles, moveDuration, TweenExtensions.EaseType.EaseOutQuint, _cts).Forget();
        await transform.TWLocalMove(target.pos, moveDuration, TweenExtensions.EaseType.EaseOutQuint, _cts);
    }

    public void SetTransform((Vector3? pos, Vector3? angles, Vector3? scale)? initialWorldTransform)
    {
        if (!initialWorldTransform.HasValue) return;

        transform.position = initialWorldTransform.Value.pos ?? transform.position;
        transform.eulerAngles = initialWorldTransform.Value.angles ?? transform.eulerAngles;
        transform.localScale = initialWorldTransform.Value.scale ?? transform.localScale;
    }

    private void OnDisable() => _cts?.Cancel();
    private void OnDestroy() => _cts?.Cancel();
}