using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTooltip : MonoBehaviour {

    InteractionType tooltipType;

    void Update() {

    }

    public void setInteractionType(InteractionType type) {
        tooltipType = type;
    }
}
