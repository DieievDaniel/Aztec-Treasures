using UnityEngine;
using UnityEngine.UI;

public class GridSelection : MonoBehaviour
{
    private bool gridSelected = false;
    private bool isPlayButtonInteractable = false;

    public Button playButton;
    public Button size3x4Button;
    public Button size4x5Button;
    public Button size5x6Button;
    public Button size6x7Button;

    public Sprite size3x4Sprite;
    public Sprite size4x5Sprite;
    public Sprite size5x6Sprite;
    public Sprite size6x7Sprite;

    private Sprite size3x4OriginalSprite;
    private Sprite size4x5OriginalSprite;
    private Sprite size5x6OriginalSprite;
    private Sprite size6x7OriginalSprite;

    private bool size3x4ButtonChanged = false;
    private bool size4x5ButtonChanged = false;
    private bool size5x6ButtonChanged = false;
    private bool size6x7ButtonChanged = false;

    private void Start()
    {
        playButton.interactable = false;

        // Сохраняем оригинальные спрайты кнопок
        size3x4OriginalSprite = size3x4Button.image.sprite;
        size4x5OriginalSprite = size4x5Button.image.sprite;
        size5x6OriginalSprite = size5x6Button.image.sprite;
        size6x7OriginalSprite = size6x7Button.image.sprite;
    }
    private void Update()
    {
        CheckGridSelected();
    }

    public void OnButtonSize3x4Clicked()
    {
        SetGridSize(3, 4);
        ToggleButtonSprite(size3x4Button, size3x4Sprite);
        size3x4ButtonChanged = !size3x4ButtonChanged;
    }

    public void OnButtonSize4x5Clicked()
    {
        SetGridSize(4, 5);
        ToggleButtonSprite(size4x5Button, size4x5Sprite);
        size4x5ButtonChanged = !size4x5ButtonChanged;
    }

    public void OnButtonSize5x6Clicked()
    {
        SetGridSize(5, 6);
        ToggleButtonSprite(size5x6Button, size5x6Sprite);
        size5x6ButtonChanged = !size5x6ButtonChanged;
    }

    public void OnButtonSize6x7Clicked()
    {
        SetGridSize(6, 7);
        ToggleButtonSprite(size6x7Button, size6x7Sprite);
        size6x7ButtonChanged = !size6x7ButtonChanged;
    }

    private void SetGridSize(int rows, int columns)
    {
        GridSizeManager.Rows = rows;
        GridSizeManager.Columns = columns;
        gridSelected = true;
        CheckGridSelected();
    }

    private void CheckGridSelected()
    {
        // Проверяем, были ли изменены спрайты кнопок
        if (size3x4ButtonChanged || size4x5ButtonChanged || size5x6ButtonChanged || size6x7ButtonChanged)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
    }

    private void ToggleButtonSprite(Button button, Sprite sprite)
    {
        Image buttonImage = button.image;
        if (buttonImage != null)
        {
            // Сохраняем текущее значение preserveAspect
            bool preserveAspect = buttonImage.preserveAspect;

            // Проверяем текущий спрайт кнопки и меняем его
            if (buttonImage.sprite == sprite)
            {
                buttonImage.sprite = GetOriginalSprite(button);
            }
            else
            {
                buttonImage.sprite = sprite;
            }

            buttonImage.SetNativeSize();

            buttonImage.preserveAspect = preserveAspect;
        }
    }

    private Sprite GetOriginalSprite(Button button)
    {
        if (button == size3x4Button) return size3x4OriginalSprite;
        if (button == size4x5Button) return size4x5OriginalSprite;
        if (button == size5x6Button) return size5x6OriginalSprite;
        if (button == size6x7Button) return size6x7OriginalSprite;
        return null;
    }
}
