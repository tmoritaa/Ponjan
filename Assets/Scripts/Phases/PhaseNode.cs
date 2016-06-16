using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class PhaseNode {
    public enum PhaseID {
        StartGame,
        Draw,
        CompleteHand,
        CompleteHandWithSteal,
        Discard,
        Steal,
        ChangeActivePlayer,
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

    public PhaseNode(PhaseID id) {
        this.phaseId = id;
    }

    public abstract IEnumerator PerformPhase(Game game);
}
