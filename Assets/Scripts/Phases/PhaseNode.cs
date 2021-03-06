﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class PhaseNode {
    public enum PhaseID {
        StartRound,
        Draw,
        CompleteHand,
        Discard,
        Steal,
        ChangeActivePlayer,
        EndRound,
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
