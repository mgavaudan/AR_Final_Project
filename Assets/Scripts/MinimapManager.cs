using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinimapManager : MonoBehaviour {

    private int arrowDirection = 0;
    public int ArrowDirection
    {
        get
        {
            return arrowDirection;
        }

        set
        {
            arrowDirection = value % 4;
            if (arrowDirection < 0)
                arrowDirection += 4;
            arrow.transform.localEulerAngles = new Vector3(0, 0, 180 - arrowDirection * 90);
        }
    }

    public Image boxPrefab;
    public Image arrowPrefab;

    public int boxHeight = 140;
    public int boxWidth = 135;

    private List<List<Image>> boxes = new List<List<Image>>();
    private Image arrow;

    private int currentX;
    private int currentY;

    public void Initialize(int height, int width, int startX, int startY)
    {
        for (int x = 0; x < width; x++)
        {
            List<Image> row = new List<Image>();
            for (int y = 0; y < height; y++)
            {
                Image box = Instantiate(boxPrefab) as Image;
                box.transform.parent = transform;
                box.transform.localPosition = new Vector3(boxWidth * (x - width / 2), boxHeight * (y - height / 2), 0);
                row.Add(box);
            }
            boxes.Add(row);
        }
        currentX = startX;
        currentY = startY;

        arrow = Instantiate(arrowPrefab) as Image;
        arrow.transform.parent = transform;
        arrow.transform.position = boxes[currentX][currentY].transform.position;
    }

    // Update is called once per frame
    void Update()
    { }

    public void crossHall()
    {
        Image nextBox;
        switch(ArrowDirection)
        {
            case 0:
                nextBox = boxes[currentX][++currentY];
                break;
            case 1:
                nextBox = boxes[++currentX][currentY];
                break;
            case 2:
                nextBox = boxes[currentX][--currentY];
                break;
            default:
                nextBox = boxes[--currentX][currentY];
                break;
        }
        arrow.transform.position = nextBox.transform.position;
    }
}
