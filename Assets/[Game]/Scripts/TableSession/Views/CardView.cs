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

    public void OnCreated() => _collider.enabled = false;

    public void OnSpawned(CardData data, params object[] additionalArgs)
    {
        Data = data;

        IsClosed = additionalArgs.Length > 0 && (bool)additionalArgs[0];

        UpdateView();
    }

    public void OnDespawned()
    {
    }

    private void UpdateView() =>
        _spriteRenderer.sprite = !IsClosed
            ? _cardSettings.CardDataSprites[Data]
            : _cardSettings.ClosedCardSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        //fire event..
    }

    public void SetInteractable(bool value) => _collider.enabled = value;

    public async UniTask MoveTo(Vector3 position)
    {
        _spriteRenderer.sortingOrder = _tableSession.NextCardSortingOrder;

        await transform.TWMove(position, .3f, TweenExtensions.EaseType.EaseOutQuint);
    }

    public async UniTask SetRotation(Quaternion rot) =>
        await transform.TWRotate(rot, .3f, TweenExtensions.EaseType.EaseOutQuint);
}