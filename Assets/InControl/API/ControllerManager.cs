
using UnityEngine;
using XInputDotNetPure;

public enum MenuInput
{
    None,
    Up,
    Down,
    Left,
    Right,
    Validation,
    Cancel,
    Start,
}

public class GamePadStateHolder
{
    public GamePadState State;
}

public class ControllerManager : MonoBehaviour
{
    // Singleton
    public static ControllerManager Instance;

    public System.Action<MenuInput, int> OnMenuInput;

    public const int c_maxControllers = 4;
    public int maxControllers = c_maxControllers;

    public bool UpdateMenuInput;
    public int ControllersCount;
    public float RepeatMenuEventDelayFirst = 1f;
    public float RepeatMenuEventDelay = 0.2f;

    public GamePadStateHolder[] GamePadStates = new GamePadStateHolder[c_maxControllers];
    public GamePadStateHolder[] LastGamePadStates = new GamePadStateHolder[c_maxControllers];

    MenuInput[] m_lastInput = new MenuInput[c_maxControllers];

    private float[] m_repeatDelay = new float[c_maxControllers];
    private float[] m_timer = new float[c_maxControllers];

    void Awake()
    {
        if (!enabled)
            return;
        Instance = this;

        for (int i = 0; i < maxControllers; ++i)
            m_repeatDelay[i] = RepeatMenuEventDelayFirst;
        maxControllers = Input.GetJoystickNames().Length;

        for (int i = 0; i < c_maxControllers; ++i)
        {
            GamePadStates[i] = new GamePadStateHolder();
            LastGamePadStates[i] = new GamePadStateHolder();
        }
    }

    void Update()
    {
        ControllersCount = 0;

        for (int i = 0; i < maxControllers; ++i)
        {
            LastGamePadStates[i].State = GamePadStates[i].State;
            var state = GamePad.GetState((PlayerIndex)i);
            GamePadStates[i].State = state;
            if (state.IsConnected)
                ++ControllersCount;
        }

        if (!UpdateMenuInput)
            return;

        MenuInput input;

        for (int i = 0; i < maxControllers; ++i)
        {
            input = MenuInput.None;
            //var lastState = LastGamePadStates[i].State;
            var state = GamePadStates[i].State;
            if (state.DPad.Up == ButtonState.Pressed)
            {
                input = MenuInput.Up;
            }
            else if (state.DPad.Down == ButtonState.Pressed)
            {
                input = MenuInput.Down;
            }
            else if (state.DPad.Right == ButtonState.Pressed)
            {
                input = MenuInput.Right;
            }
            else if (state.DPad.Left == ButtonState.Pressed)
            {
                input = MenuInput.Left;
            }
            else if (state.Buttons.A == ButtonState.Pressed)
            {
                input = MenuInput.Validation;
            }
            else if (state.Buttons.B == ButtonState.Pressed)
            {
                input = MenuInput.Cancel;
            }
            else if (state.Buttons.Start == ButtonState.Pressed)
            {
                input = MenuInput.Start;
            }
            else if (i == 0)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    input = MenuInput.Up;
                else if (Input.GetKey(KeyCode.DownArrow))
                    input = MenuInput.Down;
                else if (Input.GetKey(KeyCode.RightArrow))
                    input = MenuInput.Right;
                else if (Input.GetKey(KeyCode.LeftArrow))
                    input = MenuInput.Left;
                else if (Input.GetKey(KeyCode.KeypadEnter))
                    input = MenuInput.Validation;
                else if (Input.GetKey(KeyCode.Return))
                    input = MenuInput.Validation;
                else if (Input.GetKey(KeyCode.Backspace))
                    input = MenuInput.Cancel;
                else if (Input.GetKey(KeyCode.Escape))
                    input = MenuInput.Start;
            }
            if (input != MenuInput.None && OnMenuInput != null)
            {
                if (input != m_lastInput[i])
                {
                    m_lastInput[i] = input;
                    OnMenuInput(input, i);
                }
                else
                {
                    m_timer[i] += Time.deltaTime;

                    if (m_timer[i] > m_repeatDelay[i])
                    {
                        m_timer[i] = 0;
                        OnMenuInput(input, i);
                        m_repeatDelay[i] = RepeatMenuEventDelay;
                    }
                }
            }
            else
            {
                m_lastInput[i] = input;
                m_timer[i] = 0;
                m_repeatDelay[i] = RepeatMenuEventDelayFirst;
            }
        }
    }

    [ContextMenu("P2 Validation")]
    void ReplicateForP2()
    {
        OnMenuInput(MenuInput.Validation, 1);
    }

    public bool GetYDown(int _index)
    {
        return GamePadStates[_index].State.Buttons.Y == ButtonState.Pressed && LastGamePadStates[_index].State.Buttons.Y == ButtonState.Released;
    }
    public bool GetYUp(int _index)
    {
        return GamePadStates[_index].State.Buttons.Y == ButtonState.Released && LastGamePadStates[_index].State.Buttons.Y == ButtonState.Pressed;
    }
}
