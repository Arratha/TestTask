using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


public class Restart : MonoBehaviour
{
    [SerializeField]
    LevelMaster lvlMaster;

    public UnityEvent RestartEvent = new UnityEvent();

    [SerializeField]
    GameObject background;

    private void OnMouseDown()
    {
        RestartEvent.Invoke();

        background.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        background.SetActive(false);
    }

    private void FadeIn()
    {
        background.GetComponent<SpriteRenderer>().DOFade(0.5f, 2);
    }

    private void Start()
    {
        lvlMaster.PauseEvent.AddListener(FadeIn);
        background.SetActive(false);
    }
}
