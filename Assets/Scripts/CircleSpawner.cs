using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private GameObject circleGreenPrefab;
    [SerializeField] private GameObject circleBlackPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform HUD;
    private float spawnTimer = 0;
    private List<Vector2> spawnPositions = new List<Vector2>();
    private float SpawnDelay { get => 3 / (game.Score - (-1f)) + 0.2f; }
    private float MinLifespanGreen { get => 100f / (game.Score - (-66f)) + 0.5f; }
    private Vector2 resolutionScale;

    private void Start()
    {
        resolutionScale = canvas.GetComponent<RectTransform>().localScale;

        for (int i = 0; i < parent.childCount; i++)
        {
            var circle = parent.GetChild(i);
            Vector2 position = circle.GetComponent<RectTransform>().position;
            spawnPositions.Add(position);
            StartCoroutine(ClearSpawnPosition(position, circle.GetComponent<Circle>().Lifespan));
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnDelay && Time.deltaTime != 0)
        {
            GameObject circlePrefab = Random.Range(0, 10) > 0 ? circleGreenPrefab : circleBlackPrefab;
            Vector2 circleSize = circlePrefab.GetComponent<RectTransform>().rect.size;
            float posX = Random.Range(0 + circleSize.x / 2, canvas.rect.width - circleSize.x / 2) * resolutionScale.x;
            float posY = Random.Range(0 + circleSize.y / 2, canvas.rect.height - HUD.rect.height - circleSize.y / 2) * resolutionScale.y;
            Vector3 newPosition = new Vector2(posX, posY);

            foreach (Vector2 position in spawnPositions)
                if (Vector2.Distance(position, newPosition) < circleSize.x)
                    return;

            GameObject newCircle = Instantiate(circlePrefab, parent);
            newCircle.GetComponent<CircleGreen>()?.SetLifespan(MinLifespanGreen, MinLifespanGreen + 2);

            spawnPositions.Add(newPosition);
            StartCoroutine(ClearSpawnPosition(newPosition, newCircle.GetComponent<Circle>().Lifespan));
            newCircle.GetComponent<RectTransform>().position = newPosition;
            
            spawnTimer = 0;
        }
    }

    private IEnumerator ClearSpawnPosition(Vector2 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        spawnPositions.Remove(position);
    }
}
