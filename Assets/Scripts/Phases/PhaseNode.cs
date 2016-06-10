using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class PhaseNode {
    public enum PhaseID {
        StartGame,
        BeginTurn,
        Draw,
        GainMaterial,
        Movement,
        Play,
        Ability,
        AttackCycle,
        Discard,
        EndTurn,
    };
    
    protected PhaseID phaseId;
    public PhaseID PhaseId {
        get { return this.phaseId; }
    }

    protected PhaseNode next;
    public PhaseNode Next {
        set { this.next = value; }
        get { return this.next; }
    }

    protected string name;
    public string Name {
        get { return this.name; }
    }

    public PhaseNode(PhaseID id, string name) {
        this.phaseId = id;
        this.name = name;
    }

    public abstract IEnumerator PerformPhase(Game game);
}
