using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
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
        builder.Register(typeof(Pool<>), Lifetime.Scoped).AsSelf();

        //Table
        builder.RegisterInstance(_currentTableData).AsSelf();

        builder.Register<TableSession>(Lifetime.Scoped).AsSelf();
        builder.Register<ScoreHandler>(Lifetime.Scoped).AsSelf();
        builder.RegisterComponentInHierarchy<TableView>().AsSelf();

        //States
        builder.Register<TableSessionStateManager>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();

        builder.Register<TablePreparation_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<SessionStart_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<DealingCards_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<CardPlayingLoop_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<SessionEnd_TableSessionState>(Lifetime.Scoped).AsSelf();

        builder.RegisterBuildCallback(InjectAdditionalObjects);
        return;

        void InjectAdditionalObjects(IObjectResolver b)
        {
            foreach (var obj in _objectsNeedToBeInjected)
                if (obj)
                    b.InjectGameObject(obj);
        }
    }

}