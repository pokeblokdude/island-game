using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputQueue {
    
    Queue<Action> queue;

    public InputQueue() {
        queue = new Queue<Action>();
    }

    public Action Read() {
        if(queue.Count > 0) {
            return queue.Dequeue();
        }
        else {
            return null;
        }
    }

    public void Add(Action action) {
        queue.Enqueue(action);
    }

    public void ClearOne() {
        queue.Dequeue();
    }

    public Action Check() {
        if(queue.Count > 0) {
            return queue.Peek();
        }
        else {
            return null;
        }
    }

}

public class Action {
    public ActionType actionType { get; private set; }
    public float enqueueTime { get; private set; }

    public Action(ActionType actionType) {
        this.actionType = actionType;
        this.enqueueTime = Time.time;
    }

    public enum ActionType {
        JUMP
    }
}