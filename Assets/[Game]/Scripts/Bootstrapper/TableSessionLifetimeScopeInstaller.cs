using System.Collections.Generic;
using UnityEngine;using VContainer;
using VContainer.Unity;

public class TableSessionLifetimeScopeInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private List<GameObject> _objectsNeedToBeInjected;

    [Inject] private readonly IObjectResolver _objectResolver;
    [Inject] private readonly CardSettings _cardSettings;

    private TableData _currentTableData;

    public void SetScopeData(TableData data) => _currentTableData = data;

    public void Install(IContainerBuilder builder)
    {
        //Pool
        builder.Register(typeof(Pool<>), Lifetime.Singleton);
        builder.RegisterInstance(new Pool<CardView>.PoolProperties()
        {
            ExpansionSize = 5,
            FillOnInit = true,
            Prefab = _cardSettings.CardPrefab,
        }).AsSelf();

        //Table
        builder.RegisterInstance(_currentTableData).AsSelf();

        builder.Register<TableSession>(Lifetime.Scoped).AsSelf();

        builder.Register<TableSessionStateManager>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();

        builder.Register<TablePreparation_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<SessionStart_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<DealingCards_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<CardPlayingLoop_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<SessionEnd_TableSessionState>(Lifetime.Scoped).AsSelf();
        //..
    }

    public void InjectAdditionalTableObjects()
    {
        foreach (var obj in _objectsNeedToBeInjected) 
            _objectResolver.Inject(obj);
    }
}