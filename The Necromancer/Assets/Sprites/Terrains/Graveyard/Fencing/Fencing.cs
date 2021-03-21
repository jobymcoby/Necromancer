using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Fencing : RuleTile<Fencing.Neighbor> {

    public bool customField;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int bottomLeft = 3;
        public const int topRight = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.bottomLeft: return tile != null;
            case Neighbor.topRight: return tile != null;
        }
        return base.RuleMatch(neighbor, tile);
    }
}
