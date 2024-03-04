using UnityEngine;

public class GridSpriteProvider : MonoBehaviour
{
    // Статическая ссылка на экземпляр класса
    private static GridSpriteProvider _instance;

    // Массивы спрайтов для разных размеров сетки
    public Sprite[] gridSprites3x4;
    public Sprite[] gridSprites4x5;
    public Sprite[] gridSprites5x6;
    public Sprite[] gridSprites6x7;

    // Свойства для доступа к массивам спрайтов
    public static Sprite[] GridSprites3x4 => _instance.gridSprites3x4;
    public static Sprite[] GridSprites4x5 => _instance.gridSprites4x5;
    public static Sprite[] GridSprites5x6 => _instance.gridSprites5x6;
    public static Sprite[] GridSprites6x7 => _instance.gridSprites6x7;

    private void Awake()
    {
        // Установка ссылки на текущий экземпляр
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
