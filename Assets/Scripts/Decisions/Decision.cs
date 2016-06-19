using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class Decision {
    protected Player controller;
    protected Game game;
    protected List<object> response;
    public List<object> Response {
        get { return this.response; }
    }

    public Decision(Player controller, Game game) {
        this.controller = controller;
        this.game = game;
    }

    protected bool CanBeCastTo(object obj, Type type) {
        return obj.GetType().IsAssignableFrom(type);
    }

    public virtual IEnumerator PerformDecision() {
        IEnumerator ienum = null;

        if (this.controller != null) {
            switch (this.controller.ControllerType) {
                case Player.PlayerType.AI:
                    ienum = this.HandleAI();
                    break;
                case Player.PlayerType.Human:
                    ienum = this.HandlePlayer();
                    break;
                default:
                    break;
            }
        } else {
            ienum = this.HandlePlayer();
        }

        bool hasNext = true;
        do {
            hasNext = ienum.MoveNext();
            yield return 0;
        } while (hasNext);

        yield break;
    }

    protected void SaveCompleteUIResponse(List<object> response) {
        this.response = response;
    }

    public abstract bool HandleUIResponse(object response);
    public abstract IEnumerator HandlePlayer();
    public abstract IEnumerator HandleAI();
}

