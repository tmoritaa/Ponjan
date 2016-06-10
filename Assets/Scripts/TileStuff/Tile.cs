using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Tile {
    private int type;

    private int number;

    Zone zone;
    public Zone Zone {
        get { return this.zone; }
        set { this.zone = value; }
    }
}
