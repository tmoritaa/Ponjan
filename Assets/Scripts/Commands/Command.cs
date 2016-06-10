using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Command {
    public abstract IEnumerator PerformCommand(Game game);

    // TODO: going to have to implement this once state tracker begins to get implemented.
    //public abstract void UndoCommand();
}
