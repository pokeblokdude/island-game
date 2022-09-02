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
    public bool back = false;
    public bool menuLeft = false;
    public bool menuRight = false;
    // Input System bug - this would be one Vector2 input, but it breaks other maps
    public bool up;
    public bool down;
    public bool left;
    public bool right;
}

public class GeneralInput {
    public bool reset = false;
    public bool quit = false;
    public bool pause = false;
    public bool map = false;
}

public class DebugInput {
    public bool toggleDebugRays = false;
}

public static class GameInput {
    
    private static InputManager im;

    public static PlayerInput Player { get; private set; } = null;
    public static UIInput UI { get; private set; } = null;
    public static GeneralInput Game { get; private set; } = null;
    public static DebugInput _Debug { get; private set; } = null;

    static GameInput() {
        im = new InputManager();
        Player = new PlayerInput();
        UI = new UIInput();
        Game = new GeneralInput();
        _Debug = new DebugInput();

        im.Enable();

        #region ============== PLAYER ======================
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
        im.Player.ActionUp.performed += ctx => {
            Player.actionUp = true;
        };
        im.Player.ActionUp.canceled += ctx => {
            Player.actionUp = false;
        };
        #endregion

        #region ============ UI =======================
        im.UI.Confirm.performed += ctx => {
            UI.confirm = true;
        };
        im.UI.Confirm.canceled += ctx => {
            UI.confirm = false;
        };
        im.UI.Back.performed += ctx => {
            UI.back = true;
        };
        im.UI.Back.canceled += ctx => {
            UI.back = false;
        };
        im.UI.MenuLeft.performed += ctx => {
            UI.menuLeft = true;
        };
        im.UI.MenuLeft.canceled += ctx => {
            UI.menuLeft = false;
        };
        im.UI.MenuRight.performed += ctx => {
            UI.menuRight = true;
        };
        im.UI.MenuRight.canceled += ctx => {
            UI.menuRight = false;
        };
        im.UI.Up.performed += ctx => {
            UI.up = true;
        };
        im.UI.Up.canceled += ctx => {
            UI.up = false;
        };
        im.UI.Down.performed += ctx => {
            UI.down = true;
        };
        im.UI.Down.canceled += ctx => {
            UI.down = false;
        };
        im.UI.Left.performed += ctx => {
            UI.left = true;
        };
        im.UI.Left.canceled += ctx => {
            UI.left = false;
        };
        im.UI.Right.performed += ctx => {
            UI.right = true;
        };
        im.UI.Right.canceled += ctx => {
            UI.right = false;
        };
        #endregion

        #region ============ GAME =====================
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
        im.Game.Pause.performed += ctx => {
            Game.pause = true;
        };
        im.Game.Pause.canceled += ctx => {
            Game.pause = false;
        };
        im.Game.Map.performed += ctx => {
            Game.map = true;
        };
        im.Game.Map.canceled += ctx => {
            Game.map = false;
        };
        #endregion

        #region ========= DEBUG =========================
        im.Debug.ToggleDebugRays.performed += ctx => {
            if(im.Debug.ToggleDebugRays.WasPressedThisFrame()) {
                _Debug.toggleDebugRays = true;
            }
        };
        im.Debug.ToggleDebugRays.canceled += ctx => {
            _Debug.toggleDebugRays = false;
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

