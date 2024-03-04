using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridElementManager : MonoBehaviour
{
    public IEnumerator RespawnObjectsSmoothly(List<Vector3> emptyPositions, Sprite[] gridSprites, Dictionary<Sprite, int> elementCounts, float objectSize)
    {
        ShuffleList(ObjectGeneration.instance.emptyPositions);

        foreach (Vector3 position in emptyPositions)
        {
            float xPos = position.x;
            float yPos = position.y;
            Sprite gridSprite = gridSprites[Random.Range(0, gridSprites.Length)];

            if (elementCounts.ContainsKey(gridSprite))
            {
                elementCounts[gridSprite]++;
            }
            else
            {
                elementCounts.Add(gridSprite, 1);
            }

            GameObject gridObject = new GameObject("GridObject");
            gridObject.transform.position = new Vector3(xPos, yPos, 0f);
            gridObject.transform.localScale = new Vector3(objectSize, objectSize, 1.0f);
            gridObject.transform.SetParent(transform); // Родительский объект - объект, на котором висит этот скрипт

            SpriteRenderer spriteRenderer = gridObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.sortingOrder = 11; // Устанавливаем приоритет отрисовки

            StartCoroutine(BlinkEffect(spriteRenderer)); // Запускаем анимацию мерцания

            yield return null; // Ждем один кадр перед созданием следующего объекта
        }

        emptyPositions.Clear();
    }

    // Метод для анимации мерцания
    private IEnumerator BlinkEffect(SpriteRenderer spriteRenderer)
    {
        Color originalColor = spriteRenderer.color;
        float duration = 1f; // Длительность анимации мерцания
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.PingPong(Time.time, 1); // Изменяем значение альфа-канала для создания эффекта мерцания
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor; // Восстанавливаем исходный цвет после завершения анимации
    }



    // Метод для перемешивания списка
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
