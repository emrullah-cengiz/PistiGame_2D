using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MBehaviourTree
{
    public struct NodeResult
    {
        public NodeState State;
        public object[] Data;

        public NodeResult(NodeState state, params object[] data)
        {
            State = state;
            Data = data;
        }

        public static NodeResult Failure => new NodeResult(NodeState.FAILURE);
        public static NodeResult Success(params object[] data) => new NodeResult(NodeState.SUCCESS, data);
        public static NodeResult Running => new NodeResult(NodeState.RUNNING);
    }

    public class Node
    {
        // protected CancellationTokenSource _cancellationTokenSource;
        //
        // protected Node(CancellationTokenSource cts = null)
        // {
        //     _cancellationTokenSource = cts;
        // }
        protected Func<object[], Task<NodeResult>> _action;
        
        public Node(Func<object[], Task<NodeResult>> action = null)
        {
            _action = action;
        }

        public virtual Task<NodeResult> Evaluate(object[] inputData) => _action?.Invoke(inputData);
    }
    
    public class Condition : Node
    {
        private readonly Func<object[], object[]> _predicate;

        public Condition(Func<object[], object[]> action = null)
        {
            _predicate = action;
        }  
        
        public override Task<NodeResult> Evaluate(object[] inputData) => Task.FromResult(NodeResult.Success(_predicate?.Invoke(inputData)));
        
        public object[] Result(params object[] data) => data;

    }

    public abstract class CompositeNode : Node
    {
        protected Node[] _childNodes { get; private set; }

        protected CompositeNode(params Node[] nodes) => _childNodes = nodes;
    }

    public enum NodeState { RUNNING, SUCCESS, FAILURE }

    public class Parallel : CompositeNode
    {
        public Parallel(Node[] nodes) : base(nodes)
        {
        }

        public override async Task<NodeResult> Evaluate(object[] inputData)
        {
            bool isAnyRunning = false, isAnySuccess = false;
            object[] lastSiblingNodeOutput = null;

            foreach (var node in _childNodes)
            {
                var nodeResult = await node.Evaluate(lastSiblingNodeOutput);
                lastSiblingNodeOutput = nodeResult.Data;

                switch (nodeResult.State)
                {
                    case NodeState.RUNNING:
                        isAnyRunning = true;
                        continue;
                    case NodeState.SUCCESS:
                        isAnySuccess = true;
                        continue;
                }
            }

            return isAnyRunning ? NodeResult.Running : isAnySuccess ? NodeResult.Success(lastSiblingNodeOutput) : NodeResult.Failure;
        }
    }

    public class Selector : CompositeNode
    {
        public Selector(Node[] nodes) : base(nodes)
        {
        }

        public override async Task<NodeResult> Evaluate(object[] inputData)
        {
            foreach (var node in _childNodes)
            {
                var nodeResult = await node.Evaluate(null);
                if (nodeResult.State is NodeState.SUCCESS or NodeState.RUNNING)
                    return nodeResult;
            }

            return NodeResult.Failure;
        }
    }

    public class Sequence : CompositeNode
    {
        public Sequence(Node[] nodes) : base(nodes)
        {
        }

        public override async Task<NodeResult> Evaluate(object[] inputData)
        {
            object[] lastSiblingNodeOutput = null;

            foreach (var node in _childNodes)
            {
                var nodeResult = await node.Evaluate(lastSiblingNodeOutput);
                lastSiblingNodeOutput = nodeResult.Data;

                if (nodeResult.State is NodeState.FAILURE or NodeState.RUNNING)
                    return nodeResult;
            }

            return NodeResult.Success();
        }
    }

    public abstract class BehaviourTree
    {
        protected CancellationTokenSource _taskCancellationTokenSource = new();
        
        private Node _root = null;

        public void Initialize() => _root = SetupTree();

        public async Task Evaluate() => await _root?.Evaluate(null);

        public void Stop() => _taskCancellationTokenSource.Cancel();

        protected abstract Node SetupTree();
    }
}
