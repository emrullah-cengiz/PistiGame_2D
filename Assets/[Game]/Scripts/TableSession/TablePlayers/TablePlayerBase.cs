using MBehaviourTree;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public abstract class TablePlayerBase
{
    [Inject] protected readonly TableSession _tableSession;
    [Inject] public PlayerScoreHandler ScoreHandler { get; private set; }
    
    public TablePlayerView View { get; private set; }
    public CardPile Hand { get; private set; }
    public CardPile CollectedPile { get; private set; }
    public bool IsUser { get; }

    public virtual void Setup(TablePlayerView view)
    {
        View = view;

        Hand = new CardPile();
        Hand.Setup(View.HandPileView);

        CollectedPile = new CardPile();
        CollectedPile.Setup(View.CollectedPileView);
    }

    public bool HasCard() => Hand.Cards.Count > 0;

    public abstract UniTask<CardData> PlayCard();
}
