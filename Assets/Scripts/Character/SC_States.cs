using UnityEngine;

public class SC_States : MonoBehaviour {
    
    public enum States {
        STATE_RESET = 0,
        IDLE,
        WALK,
        RUN,
        RUN_END,
        PICK_ITEM,
        DROP_ITEM,
        OPEN_INVENTORY,
        CLOSE_INVENTORY,
        RECYCLE,
        NONO
    }
}