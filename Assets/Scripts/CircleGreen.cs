using UnityEngine;
using UnityEngine.UI;

public class CircleGreen : Circle
{
    private Image fill;

    public void SetLifespan(float min = 2, float max = 4)
    {
        Lifespan = Random.Range(min, max);
    }

    private void Start()
    {
        fill = transform.GetChild(0).GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() => Destroy(gameObject));
    }

    private new void Update()
    {
        base.Update();
        SetFillAmount();
    }

    protected override void Die()
    {
        base.Die();
        Game.EndGame(transform.position);
    }

    private void SetFillAmount()
    {
        fill.fillAmount = Lifetime / Lifespan;
    }
}
