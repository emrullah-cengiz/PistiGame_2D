using System.Threading.Tasks;
using MBehaviourTree;
using VContainer;

public abstract class BotPlayerNodeBase : Node
{
    [Inject] protected readonly TableSession _tableSession;
    
    protected BotPlayer _player;
    
    public BotPlayerNodeBase(BotPlayer player) => _player = player;
}

public class PlayMatchingCard : BotPlayerNodeBase
{
    public PlayMatchingCard(BotPlayer player) : base(player)
    {
    }

    public override async Task<NodeResult> Evaluate(object[] inputData)
    {
        var matchedCard = (CardData)inputData[0];
        
        await _player.Hand.TransferTo(_tableSession.DiscardPile, matchedCard, CardTransferOptions.Default);
        
        return default;
    }
}
//
// public class PlayJackCard : BotPlayerNodeBase
// {
//     public PlayJackCard(BotPlayer player) : base(player)
//     {
//     }
//
//     public override Task<NodeResult> Evaluate(object[] inputData)
//     {
//         var matchedCard = (CardData)inputData[0];
//         
//         await _player.Hand.TransferTo(_tableSession.DiscardPile, matchedCard, CardTransferOptions.Default);
//
//     }
// }
//
// public class PlaySafeCard : BotPlayerNodeBase
// {
//     public PlaySafeCard(BotPlayer player) : base(player)
//     {
//     }
//
//     public override Task<NodeResult> Evaluate(object[] inputData)
//     {
//         return default;
//     }
// }
//
// public class PlayLowestNonSpecialCard : BotPlayerNodeBase
// {
//     public PlayLowestNonSpecialCard(BotPlayer player) : base(player)
//     {
//     }
//
//     public override Task<NodeResult> Evaluate(object[] inputData)
//     {
//         return default;
//     }
// }
//
// public class PlayBluffCard : BotPlayerNodeBase
// {
//     public PlayBluffCard(BotPlayer player) : base(player)
//     {
//     }
//
//     public override Task<NodeResult> Evaluate(object[] inputData)
//     {
//         return default;
//     }
// }
//
// public class PlayMediumValueCard : BotPlayerNodeBase
// {
//     public PlayMediumValueCard(BotPlayer player) : base(player)
//     {
//     }
//
//     public override Task<NodeResult> Evaluate(object[] inputData)
//     {
//         return default;
//     }
// }