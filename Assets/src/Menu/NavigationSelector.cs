
using UnityEngine;
using UnityEngine.UI;

public class NavigationSelector : MonoBehaviour
{
    public CharactersMenu CharacterManager;
    public SelectorManager SelectorManager;
    public SelectorViewer SelectorViewer;
    public Sprite ValidatedSprite;
    public int PlayerIndex;
    public string CurrentSelection;

    [System.NonSerialized]
    public bool Locked;
    [System.NonSerialized]
    public int X;
    [System.NonSerialized]
    public int Y;

    [SerializeField][HideInInspector]
    protected Image Image;
    [SerializeField][HideInInspector]
    protected Sprite NormalSprite;

#if UNITY_EDITOR
    void OnValidate()
    {
        Image = GetComponentInChildren<Image>();
        if (Image)
            NormalSprite = Image.sprite;
    }
#endif

    private void OnEnable()
    {
        OnCancel();
        MainMenuManager.Instance.Navigators[PlayerIndex] = this;
        MainMenuManager.Instance.NavigatorsCount++;
    }
    private void OnDisable()
    {
        MainMenuManager.Instance.Navigators[PlayerIndex] = this;
        MainMenuManager.Instance.NavigatorsCount--;
    }

    public void UpdateInput(MenuInput _input, int _controller)
    {
        if (_controller != PlayerIndex)
            return;

        if (_input == MenuInput.Validation)
            OnValidation();
        else if (_input == MenuInput.Cancel)
            OnCancel();
        else if (! Locked)
        {
            string curSelection = CurrentSelection;
            SelectorManager.MoveSelector(this, _input);
            if (curSelection != CurrentSelection)
                SelectorViewer.ShowModelFromDown(CurrentSelection, PlayerIndex);
        }
    }

    private void OnValidation()
    {
        Locked = true;
        CharacterManager.SetPlayerValidationState(PlayerIndex, true, CurrentSelection);
        Image.sprite = ValidatedSprite;
    }

    public void OnCancel()
    {
        Locked = false;
        CharacterManager.SetPlayerValidationState(PlayerIndex, false, CurrentSelection);
        Image.sprite = NormalSprite;
    }
}

