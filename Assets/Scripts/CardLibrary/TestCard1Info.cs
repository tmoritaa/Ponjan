using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TestCard1Info : UnitCardInfo {
    public TestCard1Info() : base("TestCard1", new Cost(new List<Cost.CostEntry>() { new Cost.CostEntry(Cost.CostType.Material, 1) })) {
        this.SetHealth(5);
        this.SetAttackParameters(2, 2, BoardSquareZone.RangeType.Adjacent);
    }
}
