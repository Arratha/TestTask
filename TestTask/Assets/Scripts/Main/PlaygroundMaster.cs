using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System.Collections;

public class PlaygroundMaster : MonoBehaviour
{
    public UnityEvent newGame = new UnityEvent();

    [SerializeField]
    SpriteRenderer loadScreen;

    [SerializeField]
    Text textField;

    [SerializeField]
    GameObject cellPref;

    [SerializeField]
    Restart res;

    string answer;

    Collection currentSet;

    int columns;
    int lines;

    List<GameObject> cells = new List<GameObject>();
    List<SpriteRenderer> cellSprites = new List<SpriteRenderer>();

    Vector3 middlePoint = new Vector3(0, 0, 0);
    public void Init(string answer, Collection currentSet, int columns, int lines)
    {
        this.answer = answer;
        this.currentSet = currentSet;
        this.columns = columns;
        this.lines = lines;

        if (cells.Count == columns * lines)
        {
            UpdatePlayground();
        }
        else
        {
            if (cells.Count == 0)
            {
                CreateCells();
                Fade();
            }
            else
            {
                CreateAdditionalCells(columns * lines - cells.Count);
            }
        }
    }

    private void UpdatePlayground()
    {
        textField.text = "Find " + answer;

        bool isExist = false;
        int cellContent;
        for (int i = 0; i < cells.Count; i++)
        {
            cellContent = Random.Range(0, currentSet.elements.Count - 1);

            if (currentSet.elements[cellContent].name == answer)
            {
                if (isExist)
                {
                    do
                    {
                        cellContent = Random.Range(0, currentSet.elements.Count - 1);
                    }
                    while (currentSet.elements[cellContent].name == answer);
                    cells[i].name = currentSet.elements[cellContent].name;
                }
                else
                {
                    isExist = true;

                    cells[i].name = "Answer";
                }
            }
            else
            {
                cells[i].name = currentSet.elements[cellContent].name;
            }

            cellSprites[i].sprite = currentSet.elements[cellContent].image;
        }

        if (!isExist)
        {
            int targetCell;
            targetCell = Random.Range(0, cells.Count - 1);

            cells[targetCell].name = "Answer";
            for (int i = 0; i < currentSet.elements.Count; i++)
            {
                if (currentSet.elements[i].name == answer)
                {
                    cellSprites[targetCell].sprite = currentSet.elements[i].image;
                }
            }
        }
    }

    private void CreateCells()
    {
        Vector3 startPoint = FindPoint();
        float startX = startPoint.x;

        for (int i = 0; i < lines; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject newCell = Instantiate(cellPref, startPoint, new Quaternion(0, 0, 0, 0));
                newCell.transform.localScale = new Vector3(0, 0, 1);

                newCell.GetComponent<CellClick>().isActive = false;

                cells.Add(newCell);
                cellSprites.Add(newCell.GetComponentInChildren<SpriteRenderer>());

                startPoint.x += cellPref.transform.localScale.x;
            }
            startPoint.y -= cellPref.transform.localScale.y;
            startPoint.x = startX;
        }

        Bounce();
        UpdatePlayground();
    }

    private void CreateAdditionalCells(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newCell = Instantiate(cellPref, new Vector3(0,0,0), new Quaternion(0, 0, 0, 0));

            cells.Add(newCell);
            cellSprites.Add(newCell.GetComponentInChildren<SpriteRenderer>());
        }


        Vector3 startPoint = FindPoint();
        float startX = startPoint.x;

        for (int i = 0; i < lines; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                cells[i * columns + j].transform.position = startPoint;
                startPoint.x += cellPref.transform.localScale.x;
            }
            startPoint.y -= cellPref.transform.localScale.y;
            startPoint.x = startX;
        }

        UpdatePlayground();
    }

    private Vector3 FindPoint()
    {
        Vector3 result;

        result.x = middlePoint.x - (columns - 1) * cellPref.transform.localScale.x / 2;
        result.y = middlePoint.y + (lines - 1) * cellPref.transform.localScale.y / 2;
        result.z = 0;

        Vector3[] point = new Vector3[4];
        textField.GetComponent<RectTransform>().GetWorldCorners(point);

        if (result.y > point[0].y - cellPref.transform.localScale.y / 2)
        {
            result.y = point[0].y - cellPref.transform.localScale.y / 2;
        }

        return result;
    }

    private void Endgame()
    {
        StartCoroutine(screenFade());
    }

    IEnumerator screenFade()
    {
        loadScreen.DOFade(1, 1);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < cells.Count; i++)
        {
            Destroy(cells[i]);
        }

        cells.Clear();
        cellSprites.Clear();

        textField.color = new Color(0, 0, 0, 0);

        yield return new WaitForSeconds(1f);

        loadScreen.DOFade(0, 1);

        yield return new WaitForSeconds(1f);

        newGame.Invoke();
    }

    private void Bounce()
    {
        var seq = DOTween.Sequence();

        for (int i = 0; i < cells.Count; i++)
        {
            seq.Append(cells[i].transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f));
            seq.Append(cells[i].transform.DOScale(new Vector3(0.7f, 0.7f, 1), 0.3f));
            seq.Append(cells[i].transform.DOScale(new Vector3(1f, 1f, 1), 0.5f));
        }

        seq.OnComplete(() => {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].GetComponent<CellClick>().isActive = true;
            }
        });
    }

    private void Fade()
    {
        textField.DOFade(1, 2f);
    }

    private void Start()
    {
        res.RestartEvent.AddListener(Endgame);
    }
}

