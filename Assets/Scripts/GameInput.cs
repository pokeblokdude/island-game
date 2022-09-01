using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// credit to https://gist.github.com/brihernandez/VectorFuryPlayerInput.cs for starting point

public class PlayerInput {
    public float moveDir = 0;
    public bool jump = false;
    public bool crouch = false;
    public bool attack = false;
    public bool action = false;
    public bool actionUp = false;
}

public class UIInput {
    public bool confirm = false;
}

public class GeneralInput {
    public bool reset = false;
    public bool quit = false;
}

public class DebugInput {
    public bool toggleDebugRays = false;
}

public static class GameInput {
    
    private static InputManager im;

    public static PlayerInput Player { get; private set; } = null;
    public static UIInput UI { get; private set; } = null;
    public static GeneralInput Game { get; private set; } = null;
    public static DebugInput Debug { get; private set; } = null;

    static GameInput() {
        im = new InputManager();
        Player = new PlayerInput();
        UI = new UIInput();
        Game = new GeneralInput();
        Debug = new DebugInput();

        im.Enable();

        #region PLAYER
        im.Player.Move.performed += ctx => {
            Player.moveDir = Mathf.Sign(ctx.ReadValue<float>());
        };
        im.Player.Move.canceled += ctx => {
            Player.moveDir = 0;
        };
        im.Player.Jump.performed += ctx => {
            Player.jump = true;
        };
        im.Player.Jump.canceled += ctx => {
            Player.jump = false;
        };
        im.Player.Crouch.performed += ctx => {
            Player.crouch = true;
        };
        im.Player.Crouch.canceled += ctx => {
            Player.crouch = false;
        };
        im.Player.Attack.performed += ctx => {
            Player.attack = true;
        };
        im.Player.Attack.canceled += ctx => {
            Player.attack = false;
        };
        // im.Player.Action.performed += ctx => {
        //     action = true;
        // };
        // im.Player.Action.canceled += ctx => {
        //     action = false;
        // };
        im.Player.ActionUp.performed += ctx => {
            Player.actionUp = true;
        };
        im.Player.ActionUp.canceled += ctx => {
            Player.actionUp = false;
        };
        #endregion

        #region UI
        im.UI.Confirm.performed += ctx => {
            UI.confirm = true;
        };
        im.UI.Confirm.canceled += ctx => {
            UI.confirm = false;
        };
        #endregion

        #region GAME
        im.Game.Reset.performed += ctx => {
            Game.reset = true;
        };
        im.Game.Reset.canceled += ctx => {
            Game.reset = false;
        };
        im.Game.Quit.performed += ctx => {
            Game.quit = true;
        };
        im.Game.Quit.canceled += ctx => {
            Game.quit = false;
        };
        #endregion

        #region DEBUG
        im.Debug.ToggleDebugRays.performed += ctx => {
            if(im.Debug.ToggleDebugRays.WasPressedThisFrame()) {
                Debug.toggleDebugRays = true;
            }
        };
        im.Debug.ToggleDebugRays.canceled += ctx => {
            Debug.toggleDebugRays = false;
        };
        #endregion
    }

    public static void EnablePlayerControls() {
        im.Player.Enable();
    }
    public static void DisablePlayerControls() {
        im.Player.Disable();
    }
    public static void EnableUIControls() {
        im.UI.Enable();
    }
    public static void DisableUIControls() {
        im.UI.Disable();
    }

}

