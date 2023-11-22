using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{

    private static Controls _controls;
    public static void Init(Player myPlayer)
    {
        _controls = new Controls();

        _controls.Game.Movement.performed += ctx =>
        {

            myPlayer.SetMovementDirection(ctx.ReadValue<Vector3>());

        };
        _controls.Game.Shoot.performed += ctxS =>
        {
            myPlayer.Shoot();
        };
        _controls.Game.Reload.started += ctxR =>
        {
            myPlayer.Reload();
        };

        _controls.Game.Jump.started += jumpCTX =>
       {
           Debug.Log("wack");
           myPlayer.Jump();
       };
        _controls.Game.Look.performed += ctx =>
        {
            myPlayer.SetLookDirection(ctx.ReadValue<Vector2>());
        };

        _controls.Game.WeaponSwitch.performed += ctx =>
        {
           
            myPlayer.SwapWeapon(ctx.ReadValue<float>());
        };

        _controls.Game.Zoom.performed += ctx =>
        {
            myPlayer.Zoom();
        };

        _controls.Permanent.Enable();
    }

    public static void GameMode()
    {
        _controls.Game.Enable();
        _controls.UI.Disable();
    }

    public static void UIMode()
    {
        _controls.Game.Disable();
        _controls.UI.Enable();

    }



}
