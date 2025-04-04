using UnityEngine.UI;

public class CircleBlack : Circle
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Game.EndGame(transform.position));
    }
}
