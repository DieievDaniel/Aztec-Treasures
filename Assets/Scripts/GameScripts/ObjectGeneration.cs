using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridSizeBackgroundInfo
{
    public int rows;
    public int columns;
    public Vector2 backgroundSize;
}

public class ObjectGeneration : MonoBehaviour
{
    [SerializeField] public Sprite[] objectSprite;
    [SerializeField] public Sprite[] backgroundSprites;
    [SerializeField] private float distance;
    [SerializeField] public float objectSize;
    [SerializeField] private FinancialController financialController;
    [SerializeField] private GridElementManager gridElementManager;
    public ButtonManager buttonManager;

    public static bool isRolling = false;
    private bool isBackgroundSpawned = false;

    public static ObjectGeneration instance { get; private set; }
    public Dictionary<Sprite, int> elementCounts = new Dictionary<Sprite, int>();
    public List<Vector3> emptyPositions = new List<Vector3>();

    private GameObject backgroundObject;
    [SerializeField] private List<GridSizeBackgroundInfo> gridSizeBackgroundInfos;
    [SerializeField] private int defaultSortingOrder = 10; // Приоритет рисования по умолчанию

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    private void Start()
    {
        SpawnBackgroundOnStart(); // Вызываем метод спавна бекграунда при старте игры
    }

    private void SpawnBackgroundOnStart()
    {
        int rows = GridSizeManager.Rows;
        int columns = GridSizeManager.Columns;

        GridSizeBackgroundInfo currentGridSizeBackgroundInfo = gridSizeBackgroundInfos.Find(info => info.rows == rows && info.columns == columns);

        if (currentGridSizeBackgroundInfo != null)
        {
            SpawnBackground(currentGridSizeBackgroundInfo);
        }
        else
        {
            Debug.LogError("Background size information not found for the selected grid size.");
        }
    }

    public void GenerateObjects()
    {
        if (isRolling) return;

        isRolling = true;
        ClearMesh();
        emptyPositions.Clear();
        financialController.prize = 0;

        int rows = GridSizeManager.Rows;
        int columns = GridSizeManager.Columns;

        Vector3 center = transform.position - new Vector3((columns - 1) * distance / 2f, (rows - 1) * distance / 2f, 0f);

        GridSizeBackgroundInfo currentGridSizeBackgroundInfo = gridSizeBackgroundInfos.Find(info => info.rows == rows && info.columns == columns);

        if (currentGridSizeBackgroundInfo != null)
        {
            StartCoroutine(SpawnAndRespawnObjectsSmoothly(rows, columns, center));
        }
        else
        {
            Debug.LogError("Background size information not found for the selected grid size.");
        }
    }

    private void SpawnBackground(GridSizeBackgroundInfo backgroundInfo)
    {
        backgroundObject = new GameObject("BackgroundObject");
        backgroundObject.transform.parent = transform;

        SpriteRenderer spriteRenderer = backgroundObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = backgroundSprites[0];

        // Устанавливаем приоритет рисования для спрайтов фона
        foreach (Sprite sprite in backgroundSprites)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = 10; // Устанавливаем приоритет 10 для всех спрайтов фона
        }

        backgroundObject.transform.position = new Vector3(1.5f, 46, 0);
        backgroundObject.transform.localScale = new Vector3(backgroundInfo.backgroundSize.x, backgroundInfo.backgroundSize.y, 1f);
    }



    IEnumerator SpawnAndRespawnObjectsSmoothly(int rows, int columns, Vector3 center)
    {
        GameObject parentObject = this.gameObject;
        bool spawnDownwards = true;

        for (int x = 0; x < columns; x++)
        {
            // Определяем порядок спавна в зависимости от направления
            int start = spawnDownwards ? 0 : rows - 1;
            int end = spawnDownwards ? rows : -1;
            int step = spawnDownwards ? 1 : -1;

            for (int y = start; y != end; y += step)
            {
                int randomIndex = Random.Range(0, objectSprite.Length);
                Sprite randomSprite = objectSprite[randomIndex];

                Vector3 spawnPos = center + new Vector3(x * distance, y * distance, 0);

                SpawnObject(spawnPos, randomSprite);

                yield return new WaitForSeconds(0.05f);
            }

            spawnDownwards = !spawnDownwards;
        }

        // После завершения замены объектов выполняем проверку наличия выигрышных комбинаций
        StartCoroutine(CheckForMultiplier());
    }

    private void SpawnObject(Vector3 spawnPos, Sprite sprite)
    {
        GameObject gridObject = new GameObject("GridObject");
        gridObject.transform.parent = transform;

        SpriteRenderer spriteRenderer = gridObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = defaultSortingOrder;
        gridObject.transform.position = spawnPos;
        gridObject.transform.localScale = new Vector3(objectSize, objectSize, 1f);

        if (elementCounts.ContainsKey(sprite))
        {
            elementCounts[sprite]++;
        }
        else
        {
            elementCounts.Add(sprite, 1);
        }
    }

    public void ClearMesh()
    {
        foreach (Transform child in transform)
        {
            // Проверяем, имеет ли объект компонент SpriteRenderer
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Проверяем, является ли имя объекта "backgroundGrid"
                if (child.gameObject.name == "BackgroundObject")
                {
                    // Если да, пропускаем удаление этого объекта
                    continue;
                }
            }

            // Удаляем объект, если он не является backgroundGrid
            Destroy(child.gameObject);
        }

        elementCounts.Clear();
    }


    private IEnumerator CheckForMultiplier()
    {
        yield return new WaitForSeconds(1f);

        // Получаем размеры сетки
        int rows = GridSizeManager.Rows;
        int columns = GridSizeManager.Columns;

        while (VictoryChecker.CheckForWin(elementCounts, objectSprite, rows, columns))
        {
            UpdateElementCounts();
            List<Sprite> keys = new List<Sprite>(elementCounts.Keys);

            foreach (var key in keys)
            {
                int elementIndex = GetElementIndex(key);
                int count = elementCounts[key];

                float coefficient = ElementMultiplier.GetMultiplier(elementIndex, count, rows, columns);

                // Проверяем, достаточно ли элементов для выигрышной комбинации
                if (count >= GetWinningCombinationThreshold(rows, columns))
                {
                    // Умножаем выигрыш на коэффициент
                    float winnings = financialController.Rate * coefficient;
                    financialController.prize += winnings;
                    financialController.money += winnings;
                    Debug.Log("Element: " + key.name + ", Counts: " + count + ", Coefficient: " + coefficient + ", Winnings: " + winnings);
                    financialController.InformationOutput();

                    // Удаляем элементы выигрышной комбинации
                    yield return StartCoroutine(RemoveObjectsOfType(key));
                    yield return new WaitForSeconds(1f);

                    UpdateElementCounts();
                    // Пересоздаем объекты на пустых позициях
                    yield return StartCoroutine(gridElementManager.RespawnObjectsSmoothly(emptyPositions, objectSprite, elementCounts, objectSize));

                    yield return new WaitForSeconds(2f);
                }
            }
        }

        isRolling = false;
    }
    private int GetElementIndex(Sprite sprite)
    {
        for (int i = 0; i < objectSprite.Length; i++)
        {
            if (objectSprite[i] == sprite)
            {
                return i;
            }
        }
        return -1;
    }

    private int GetWinningCombinationThreshold(int rows, int columns)
    {
        // В зависимости от размеров сетки определяем необходимое количество элементов для выигрышной комбинации
        if (rows == 3 && columns == 4)
        {
            return 5;
        }
        else if (rows == 4 && columns == 5)
        {
            return 7;
        }
        else if (rows == 5 && columns == 6)
        {
            return 8;
        }
        else if (rows == 6 && columns == 7)
        {
            return 9;
        }
        else
        {
            return 0;
        }
    }






    private IEnumerator RemoveObjectsOfType(Sprite spriteToRemove)
    {
        List<Transform> objectsToRemove = new List<Transform>();

        // Создаем словарь для хранения групп объектов по типу спрайта
        Dictionary<Sprite, List<Transform>> objectGroups = new Dictionary<Sprite, List<Transform>>();

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null && spriteRenderer.sprite == spriteToRemove)
            {
                // Добавляем объект в группу объектов данного типа спрайта
                if (objectGroups.ContainsKey(spriteToRemove))
                {
                    objectGroups[spriteToRemove].Add(child);
                }
                else
                {
                    objectGroups[spriteToRemove] = new List<Transform>() { child };
                }

                // Добавляем объект в список объектов для удаления
                ObjectGeneration.instance.emptyPositions.Add(child.transform.position);
                objectsToRemove.Add(child);
            }
        }

        // Потрясаем объекты перед их уничтожением
        foreach (var group in objectGroups)
        {
            foreach (var obj in group.Value)
            {
                StartCoroutine(ShakeObject(obj.gameObject)); // Запускаем анимацию потрясывания
            }
            yield return new WaitForSeconds(0.5f); // Задержка перед уничтожением группы объектов
                                                   // Уничтожаем группу объектов одновременно
            foreach (var obj in group.Value)
            {
                Destroy(obj.gameObject);
            }
        }

        yield return null;
    }

    private IEnumerator ShakeObject(GameObject obj)
    {
        float shakeDuration = 1f;
        float shakeAmount = 2f;
        Vector3 originalPos = obj.transform.position;

        float timer = 0f;
        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;
            float percentComplete = timer / shakeDuration;
            float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

            if (obj != null) // Проверяем, существует ли объект
            {
                // Применяем случайное смещение к позиции объекта
                float x = Random.value * 2.0f - 1.0f;
                float y = Random.value * 2.0f - 1.0f;
                x *= shakeAmount * damper;
                y *= shakeAmount * damper;

                obj.transform.position = originalPos + new Vector3(x, y, 0f);
            }

            yield return null;
        }

        if (obj != null) // Проверяем, существует ли объект перед возвратом на исходную позицию
        {
            obj.transform.position = originalPos;
        }
    }

    

    private void UpdateElementCounts()
    {
        elementCounts.Clear();

        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Sprite sprite = spriteRenderer.sprite;
                if (elementCounts.ContainsKey(sprite))
                {
                    elementCounts[sprite]++;
                }
                else
                {
                    elementCounts.Add(sprite, 1);
                }
            }
        }
    }
}
