using MBehaviourTree;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

public abstract class TablePlayer
{
    [Inject] protected readonly TableSession _tableSession;
    
    public int CurrentScore { get; set; }
    
    public TablePlayerView View { get; private set; }
    public CardPile Hand { get; private set; }
    public CardPile CollectedPile { get; private set; }

    public virtual bool IsUser => false;

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
