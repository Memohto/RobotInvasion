using System.Collections;
using System;
using System.Collections.Generic;

public class AIState {
    public string Name {
        private set;
        get;
    }
    
    public Type Behaviour {
        private set;
        get;
    }

    private Dictionary<AISymbol, AIState> transitions;

    public AIState(string name, Type behaviour) {
        Name = name;
        Behaviour = behaviour;
        transitions = new Dictionary<AISymbol, AIState>();
    }

    public void AddTransition(AISymbol symbol, AIState state) {
        transitions.Add(symbol, state);
    }

    public AIState ApplySymbol(AISymbol symbol) {
        if (!transitions.ContainsKey(symbol)) {
            return this;
        }
        return transitions[symbol];
    }

}
