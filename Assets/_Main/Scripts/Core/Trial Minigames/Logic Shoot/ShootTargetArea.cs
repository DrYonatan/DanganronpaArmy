using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootTargetArea : MonoBehaviour, IPointerClickHandler
{
    public List<RectTransform> holes = new List<RectTransform>();
    public bool isCorrect;
    private Image image;
    public TextMeshProUGUI answer;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform rt = GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Spawn the marker inside the object
        RectTransform hole = Instantiate(LogicShootManager.instance.animator.holePrefab, rt);
        hole.anchoredPosition = localPoint;

        holes.Add(hole);

        if (isCorrect)
            CheckShoot();
        else
        {
            WrongAnswer();
        }
    }

    private void CheckShoot()
    {
        image.color = Color.green;
        if (holes.Count == 5)
        {
            CalculateGrouping();
        }
    }

    private void WrongAnswer()
    {
        image.DOColor(Color.red, 0.05f).SetLoops(5, LoopType.Yoyo).OnComplete(() => image.color = Color.red);
    }

    private void CalculateGrouping()
    {
        float sum = 0;
        int count = 0;

        for (int i = 0; i < holes.Count; i++)
        {
            for (int j = i + 1; j < holes.Count; j++)
            {
                sum += Vector3.Distance(holes[i].localPosition, holes[j].localPosition);
                count++;
            }
        }

        float avgDistance = sum / count;
        LogicShootManager.instance.animator.ShowMikbazText((int)Mathf.Floor(avgDistance / 10));

        transform.parent.DOLocalRotate(new Vector3(0, 360, 0), 0.1f, RotateMode.FastBeyond360).SetLoops(4)
            .OnComplete(() => Destroy(transform.parent.gameObject));
    }
}