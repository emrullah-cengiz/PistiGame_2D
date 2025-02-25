using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TableSessionLifetimeScopeInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private List<GameObject> _objectsNeedToBeInjected;
    [SerializeField] private TableView _tableView;

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

        builder.RegisterComponentInNewPrefab(_tableView, Lifetime.Scoped).AsSelf();

        builder.Register<TableSession>(Lifetime.Scoped).AsSelf();
        builder.Register<ScoreHandler>(Lifetime.Scoped).AsSelf();

        //States
        builder.Register<TableSessionStateManager>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();

        builder.Register<TablePreparation_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<SessionStart_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<DealingCards_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<CardPlayingLoop_TableSessionState>(Lifetime.Scoped).AsSelf();
        builder.Register<SessionEnd_TableSessionState>(Lifetime.Scoped).AsSelf();

        builder.RegisterBuildCallback(InjectAdditionalObjects);

        // builder.RegisterDisposeCallback(OnDisposed);
        Event.OnTableSessionEnd += OnTableSessionEnd;

        return;

        void InjectAdditionalObjects(IObjectResolver resolver)
        {
            resolver.InjectGameObject(resolver.Resolve<TableView>().gameObject);

            if (_objectsNeedToBeInjected == null) return;

            foreach (var obj in _objectsNeedToBeInjected)
                if (obj)
                    resolver.InjectGameObject(obj);
        }
    }

    private void OnTableSessionEnd()
    {
        Event.OnTableSessionEnd -= OnTableSessionEnd;

        Destroy(GameObject.FindAnyObjectByType<TableView>().gameObject);
    }
}