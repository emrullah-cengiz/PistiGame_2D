using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MBehaviourTree;
using NUnit.Framework;
using VContainer;

public class BotPlayer : TablePlayerBase
{
    // private BotPlayerAI _behaviourTree;
    [Inject] private readonly IObjectResolver _objectResolver;
    [Inject] private readonly CardSettings _cardSettings;

    public override async UniTask<CardData> PlayCard()
    {
        var cardToDiscard = GetBestCardToPlay();

        await Hand.TransferTo(_tableSession.DiscardPile, cardToDiscard, CardTransferOptions.Default);
        
        return cardToDiscard;
    }

    private CardData GetBestCardToPlay()
    {
        if (_tableSession.DiscardPile.HasAnyCard &&
            (Hand.HasAny(_tableSession.DiscardPile.LastAddedCard!.Value.Value, out var card) ||
             Hand.HasAnyJack(out card)))
            return card!.Value;

        return Hand.GetLowestSpecialOrAnyNonSpecialCard();
    }
}
//
// public class BotPlayerAI : BehaviourTree
// {
//     [Inject] private readonly IObjectResolver _objectResolver;
//     [Inject] protected readonly TableSession _tableSession;
//
//     private BotPlayer _player;
//
//     private void Initialize(BotPlayer player)
//     {
//         base.Initialize();
//
//         _player = player;
//     }
//
//     protected override Node SetupTree()
//     {
//         return new Selector(new Node[]
//         {
//             new Sequence(new Node[]
//             {
//                 //Check if DiscardPile has card
//                 new Condition(@params => new object[] { _tableSession.DiscardPile.HasAnyCard }), //Condition
//                 new Selector(new Node[]
//                 {
//                     new Sequence(new Node[]
//                     {
//                         //Check can play matching card
//                         new Condition(@params => //Condition
//                         {
//                             _player.Hand.HasAny(_tableSession.DiscardPile.LastAddedCard.Value, out var card);
//                             return new object[] { card };
//                         }),
//                         InjectNode(new PlayMatchingCard(_player)),
//                     }),
//                     new Sequence(new Node[]
//                     {
//                         new Condition(@params =>
//                                           Task.FromResult(NodeResult.Success(_player.Hand.HasAnyJack(out var card), card))), //Condition
//                         InjectNode(new PlayJackCard(_player)),
//                     }),
//                     InjectNode(new PlaySafeCard()),
//                 })
//             }),
//
//             new Selector(new Node[]
//             {
//                 new Sequence(new Node[]
//                 {
//                     InjectNode(new CheckHasSpecialCards()), //condition
//                     InjectNode(new PlayLowestNonSpecialCard()),
//                 }),
//
//                 new Sequence(new Node[]
//                 {
//                     InjectNode(new CheckCanBluff()), //condition
//                     InjectNode(new PlayBluffCard()),
//                 }),
//
//                 InjectNode(new PlayMediumValueCard()),
//             })
//         });
//     }
//
//     private TNode InjectNode<TNode>(TNode node)
//     {
//         _objectResolver.Inject(node);
//         return node;
//     }
// }