using System.Collections.Generic;
using UnityEngine;

public class CommandGraph {
    
    private Dictionary<Ability, List<Edge>> commandGraph;

    public CommandGraph() {
        commandGraph = new Dictionary<Ability, List<Edge>>();
    }

    // add a directed edge between abilities in the graph
    public void AddAbilityTransition(Ability from, string inputCommand, Ability to)
    {
        if (!commandGraph.ContainsKey(from)) {
            commandGraph[from] = new List<Edge>();
        }

        commandGraph[from].Add(new Edge(inputCommand, to));
    }

    // add a directed edge between abilities in the graph indicating if released or pressed
    public void AddAbilityTransition(Ability from, string inputCommand, Ability to, bool isPressedCondition)
    {
        if (!commandGraph.ContainsKey(from)) {
            commandGraph[from] = new List<Edge>();
        }

        TransitionConditionType transitionCondition;
        if (isPressedCondition) {
            transitionCondition = TransitionConditionType.KeyPressed;
        }
        else {
            transitionCondition = TransitionConditionType.KeysReleased;
        }

        commandGraph[from].Add(new Edge(inputCommand, to, transitionCondition));
    }

    // add a directed edge between abilities in the graph with reference to timed ability interface
    public void AddAbilityTransition(Ability from, ITimedAbility reference, Ability to)
    {
        if (!commandGraph.ContainsKey(from)) {
            commandGraph[from] = new List<Edge>();
        }

        commandGraph[from].Add(new Edge(reference, to));
    }
    
    // add a directed edge between abilities in the graph with reference from IBidrectionaAbility
    public void AddAbilityTransition(Ability from, IBidirectionalAbility reference, string inputCommand)
    {
        if (!commandGraph.ContainsKey(from)) {
            commandGraph[from] = new List<Edge>();
        }

        commandGraph[from].Add(new Edge(reference, inputCommand));
    }

    // checks next ability based on available options
    public Ability GetNextAbility(Ability current)
    {
        if (current == null)
        {
            return null;
        }

        if (commandGraph.ContainsKey(current))
        {
            foreach (var edge in commandGraph[current])
            {
                switch (edge.ConditionType)
                {
                    case TransitionConditionType.KeyPressedOnce:
                        if (InputManager.Instance.IsKeyPressedOnce(edge.InputCommand))
                        {
                            //Debug.Log(edge.ToAbility);
                            return edge.ToAbility;
                        }
                        break;

                    case TransitionConditionType.KeyPressed:
                        if (InputManager.Instance.IsKeyPressed(edge.InputCommand))
                        {
                            //Debug.Log(edge.ToAbility);
                            return edge.ToAbility;
                        }
                        break;

                    case TransitionConditionType.Timed:
                        if ((edge.TimedReference != null) && edge.TimedReference.IsComplete())
                        {
                            //Debug.Log(edge.ToAbility);
                            return edge.ToAbility;
                        }
                        break;

                    case TransitionConditionType.KeysReleased:
                        if (!InputManager.Instance.IsKeyPressed(edge.InputCommand))
                        {
                            //Debug.Log(edge.ToAbility);
                            return edge.ToAbility;
                        }
                        break;

                    case TransitionConditionType.Bidirectional:
                        if ((edge.BidirectionalReference != null) && edge.BidirectionalReference.IsComplete())
                        {
                            return edge.BidirectionalReference.GetReturnAbility();
                        }
                        break;
                }
            }
        }
        return null;
    }

    // enum to define types of conditions
    private enum TransitionConditionType
    {
        KeyPressedOnce,
        KeyPressed,     // based on user input
        Timed,          // based on a timer
        KeysReleased,   // based on character movement (Vector2.zero)
        Bidirectional   // returns to ability that directed to it
    }

    // class to represent a directed edge in the ability graph
    private class Edge
    {
        public string InputCommand { get; }
        public Ability ToAbility { get; }
        public TransitionConditionType ConditionType { get; }
        public ITimedAbility TimedReference { get; }
        public IBidirectionalAbility BidirectionalReference { get; }

        public Edge(IBidirectionalAbility reference, string inputCommand)
        {
            InputCommand = inputCommand;
            ToAbility = null;
            BidirectionalReference = reference;

            ConditionType = TransitionConditionType.Bidirectional;
        }

        public Edge(string inputCommand, Ability toAbility)
        {
            InputCommand = inputCommand;
            ToAbility = toAbility;
            ConditionType = TransitionConditionType.KeyPressedOnce;
        }

        public Edge(ITimedAbility reference, Ability toAbility)
        {
            ToAbility = toAbility;
            TimedReference = reference;
            ConditionType = TransitionConditionType.Timed;
        }

        public Edge(string inputCommand, Ability toAbility, TransitionConditionType transitionCondition)
        {
            InputCommand = inputCommand;
            ToAbility = toAbility;
            ConditionType = transitionCondition;
        }
    }
}
