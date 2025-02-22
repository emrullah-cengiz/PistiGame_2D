using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class CardView : MonoBehaviour, IInitializablePoolable<CardData>, IPointerDownHandler
{
    public bool IsClosed { get; private set; }

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _collider;

    [Inject] private readonly CardSettings _cardSettings;

    private CardData Data;

    public void OnCreated() => _collider.enabled = false;

    public void OnSpawned(CardData data, params object[] additionalArgs)
    {
        Data = data;

        if (additionalArgs.Length > 0)
            IsClosed = (bool)additionalArgs[0];

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

    public async UniTask MoveTo(Vector3 position) => 
        await transform.TWMove(position, .3f, TweenExtensions.EaseType.EaseOutQuint);
}