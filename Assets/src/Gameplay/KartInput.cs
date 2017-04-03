
using XInputDotNetPure;

[System.Serializable]
public class KartInput
{
    public KartInput(GamePadStateHolder _state, GamePadStateHolder _lastState)
    {
        gamePadState = _state;
        lastGamePadState = _lastState;
    }
    public int ControllerIndex;
    public GamePadStateHolder gamePadState;
    public GamePadStateHolder lastGamePadState;

    public bool moveForward;
    public bool stop;
    public bool fire;
    public bool jump;
    public float horizontal;
    public float vertical;
    private float lTrigger;
    private bool lTriggerBool;
    public bool lTriggerUp;

    public void UpdateInput()
    {
        moveForward = gamePadState.State.Buttons.A == ButtonState.Pressed;
        stop = gamePadState.State.Buttons.X == ButtonState.Pressed;
        fire = gamePadState.State.Buttons.B == ButtonState.Pressed && lastGamePadState.State.Buttons.B == ButtonState.Released;
        jump = gamePadState.State.Buttons.RightShoulder == ButtonState.Pressed;

        horizontal = 0;
        horizontal = gamePadState.State.DPad.Right == ButtonState.Pressed ? 1 : gamePadState.State.DPad.Left == ButtonState.Pressed ? -1 : 0;
        if (horizontal == 0)
            horizontal = gamePadState.State.ThumbSticks.Left.X;
        vertical = 0;
        vertical = gamePadState.State.DPad.Up == ButtonState.Pressed ? 1 : gamePadState.State.DPad.Down == ButtonState.Pressed ? -1 : 0;
        if (vertical == 0)
            vertical = gamePadState.State.ThumbSticks.Left.Y;

        lTriggerUp = false;
        lTrigger = gamePadState.State.Triggers.Left;
        if (lTriggerBool)
        {
            if (lTrigger < 0.35f)
                lTriggerBool = false;
        }
        else
        {
            if (lTrigger > 0.75f)
            {
                lTriggerBool = true;
                lTriggerUp = true;
            }
        }
    }
}
