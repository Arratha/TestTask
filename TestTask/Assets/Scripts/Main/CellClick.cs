using UnityEngine;
using DG.Tweening;

public class CellClick : MonoBehaviour
{
    GameObject stars;

    LevelMaster lvlMaster;
    public bool isActive = true;

    [SerializeField]
    GameObject cellContent;

    private void OnMouseDown()
    {
        if (isActive)
        {
            if (gameObject.name == "Answer")
            {
                Bounce();
            }
            else
            {
                Shake();
            }
        }
    }

    private void Pause()
    {
        isActive = !isActive;
    }

    private void Bounce()
    {
        isActive = false;
        stars.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -2);

        var bounceSeq = DOTween.Sequence();

        bounceSeq.Append(cellContent.transform.DOScale(new Vector3(0.1f, 0.1f, 1), 0.5f));
        bounceSeq.Append(cellContent.transform.DOScale(new Vector3(0.3f, 0.3f, 1), 0.3f));
        bounceSeq.Append(cellContent.transform.DOScale(new Vector3(0.2f, 0.2f, 1), 0.5f));

        bounceSeq.OnComplete(() => { 
            lvlMaster.newLevel();
            isActive = true;
            stars.transform.position = new Vector3(0, 0, -10);
        });
    }
    private void Shake()
    {
        var shakeSeq = DOTween.Sequence();
        float startX = this.transform.position.x;

        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX + 0.6f, 0.1f), 0.1f));
        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX + 0.6f, 0.2f), 0.1f));
        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX + 0.6f, 0.3f), 0.1f));
        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX - 0.6f, 0.1f), 0.1f));
        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX - 0.6f, 0.2f), 0.1f));
        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX - 0.6f, 0.3f), 0.1f));
        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX, 0.1f), 0.1f));
        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX, 0.2f), 0.1f));
        shakeSeq.Append(cellContent.transform.DOMoveX(EaseInBounce(this.transform.position.x, startX, 0.3f), 0.1f));
    }

    private void Start()
    {
        stars = GameObject.Find("Stars");
        lvlMaster = GameObject.Find("GameManager").GetComponent<LevelMaster>();
        lvlMaster.PauseEvent.AddListener(Pause);
    }

    private float EaseOutBounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < (1 / 2.75f))
        {
            return end * (7.5625f * value * value) + start;
        }
        else if (value < (2 / 2.75f))
        {
            value -= (1.5f / 2.75f);
            return end * (7.5625f * (value) * value + .75f) + start;
        }
        else if (value < (2.5 / 2.75))
        {
            value -= (2.25f / 2.75f);
            return end * (7.5625f * (value) * value + .9375f) + start;
        }
        else
        {
            value -= (2.625f / 2.75f);
            return end * (7.5625f * (value) * value + .984375f) + start;
        }
    }

    private float EaseInBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        return end - EaseOutBounce(0, end, d - value) + start;
    }
}