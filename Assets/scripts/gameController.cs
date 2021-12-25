using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    public GameObject[] boxes;
    public GameObject emptyBox;
    bool finished_game = false;
    public float speed;
    Vector3[,] positions;
    public static int emptyBoxCol;
    public static int emptyBoxRow;
    Vector3[] tmpPositions;
    public GameObject sound;
    public static bool letClick;
    public GameObject button;

    private void Start()
    {
        letClick = true;

        positions = new Vector3[4, 4];
        for (int i = 0; i < 4; ++i)
            for (int j = 0; j < 4; ++j)
                positions[i, j] = boxes[i*4+j].transform.position;

        tmpPositions = new Vector3[16];
        for (int i = 0; i < 16; ++i)
            tmpPositions[i] = boxes[i].transform.position;
    }

    public void changePosition(GameObject box)
    {
        if (!letClick)
            return;

        if (box.name == "empty" || finished_game)
            return;


        int boxRow = box.GetComponent<boxInfo>().row;
        int boxCol = box.GetComponent<boxInfo>().col;

        if (boxCol == emptyBoxCol && boxRow != emptyBoxRow || boxCol != emptyBoxCol && boxRow == emptyBoxRow)
        {
            Destroy(Instantiate(sound), 2);
            letClick = false;

            if (boxRow < emptyBoxRow)
            {
                StartCoroutine(move_down(box));
                return;
            }

            if (boxRow > emptyBoxRow)
            {
                StartCoroutine(move_up(box));
                return;
            }

            if (boxCol > emptyBoxCol)
            {
                StartCoroutine(move_left(box));
                return;
            }

            if (boxCol < emptyBoxCol)
            {
                StartCoroutine(move_right(box));
                return;
            }
        }
    }

    IEnumerator move_down(GameObject box)
    {
        float goal = positions[box.GetComponent<boxInfo>().row, box.GetComponent<boxInfo>().col -1].y;
        int boxRow = box.GetComponent<boxInfo>().row;
        int boxCol = box.GetComponent<boxInfo>().col;

        while (box.transform.position.y - speed * Time.deltaTime >= goal)
        {
            for(int i = boxRow; i < emptyBoxRow; ++i)
                boxes[4 * (i - 1) + boxCol - 1].transform.position += Vector3.down * speed * Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        string[] symbols = new string[16];

        for (int i = 0; i < 16; ++i)
            symbols[i] = boxes[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text;

        for (int i = boxRow; i < emptyBoxRow; ++i)
        {
            boxes[4 * i + boxCol - 1].name = symbols[4 * (i) + boxCol - 1 - 4];
            boxes[4 * i + boxCol - 1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = symbols[4 * i + boxCol - 1 - 4];
        }

        Color imgColor = box.GetComponent<Image>().color;

        boxes[4 * (emptyBoxRow - 1) + emptyBoxCol - 1].GetComponent<Image>().color = imgColor;
        boxes[4 * (emptyBoxRow - 1) + emptyBoxCol - 1].transform.GetChild(0).GetComponent<Image>().color = imgColor;
        boxes[4 * (boxRow - 1) + boxCol - 1].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        boxes[4 * (boxRow - 1) + boxCol - 1].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
       
        boxes[4 * (boxRow - 1) + boxCol - 1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
        
        boxes[4 * (boxRow - 1) + boxCol - 1].name = box.name;
        box.name = "empty";

        emptyBoxCol = boxCol;
        emptyBoxRow = boxRow;

        checkCombination();
    }

    IEnumerator move_up(GameObject box)
    {
        float goal = positions[box.GetComponent<boxInfo>().row - 2, box.GetComponent<boxInfo>().col - 1].y;
        int boxRow = box.GetComponent<boxInfo>().row;
        int boxCol = box.GetComponent<boxInfo>().col;

        while (box.transform.position.y + speed * Time.deltaTime < goal)
        {
            for (int i = boxRow; i > emptyBoxRow; --i)
                boxes[4 * (i - 1) + boxCol - 1].transform.position += Vector3.up * speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        string[] symbols = new string[16];

        for (int i = 0; i < 16; ++i)
            symbols[i] = boxes[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text;



        for (int i = boxRow; i > emptyBoxRow; --i)
            boxes[4 * (i - 1) + boxCol - 1].transform.position = tmpPositions[4 * (i - 2) + boxCol - 1];
       
        for (int i = boxRow; i > emptyBoxRow; --i)
        {
            boxes[4 * (i - 2) + boxCol - 1].name = symbols[4 * i + boxCol - 1-4];
            boxes[4 * (i - 2) + boxCol - 1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = symbols[4 * i + boxCol - 4 - 1];
        }


        Color imgColor = box.GetComponent<Image>().color;

        boxes[4 * (emptyBoxRow - 1) + emptyBoxCol - 1].GetComponent<Image>().color = imgColor;
        boxes[4 * (emptyBoxRow - 1) + emptyBoxCol - 1].transform.GetChild(0).GetComponent<Image>().color = imgColor;
        boxes[4 * (boxRow - 1) + boxCol - 1].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        boxes[4 * (boxRow - 1) + boxCol - 1].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);

        boxes[4 * (boxRow - 1) + boxCol - 1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";

        boxes[4 * (boxRow-1) + boxCol - 1].name = box.name;
        box.name = "empty";

        emptyBoxCol = boxCol;
        emptyBoxRow = boxRow;

        checkCombination();
    }

    IEnumerator move_left(GameObject box)
    {
        float goal = positions[box.GetComponent<boxInfo>().row - 1, box.GetComponent<boxInfo>().col - 2].x;
        int boxRow = box.GetComponent<boxInfo>().row;
        int boxCol = box.GetComponent<boxInfo>().col;

        while (box.transform.position.x - speed * Time.deltaTime > goal)
        {
            for (int i = boxCol; i > emptyBoxCol; --i)
                boxes[4 * (boxRow - 1) + i - 1].transform.position += Vector3.left * speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        string[] symbols = new string[16];

        for (int i = 0; i < 16; ++i)
            symbols[i] = boxes[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text;


        for (int i = boxCol; i > emptyBoxCol; --i)
        {
            boxes[4 * (boxRow - 1) + i - 2].name = symbols[4 * (boxRow - 1) + i - 1];
            boxes[4 * (boxRow - 1) + i - 2].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = symbols[4 * (boxRow - 1) + i - 1];
        }


        Color imgColor = box.GetComponent<Image>().color;

        boxes[4 * (emptyBoxRow - 1) + emptyBoxCol - 1].GetComponent<Image>().color = imgColor;
        boxes[4 * (emptyBoxRow - 1) + emptyBoxCol - 1].transform.GetChild(0).GetComponent<Image>().color = imgColor;


        boxes[4 * (boxRow - 1) + boxCol - 1].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        boxes[4 * (boxRow - 1) + boxCol - 1].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);

        boxes[4 * (boxRow - 1) + boxCol - 1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";

        boxes[4 * (boxRow - 1) + boxCol - 1].name = box.name;
        box.name = "empty";

        emptyBoxCol = boxCol;
        emptyBoxRow = boxRow;

        checkCombination();
    }

    IEnumerator move_right(GameObject box)
    {
        float goal = positions[box.GetComponent<boxInfo>().row - 1, box.GetComponent<boxInfo>().col].x;
        int boxRow = box.GetComponent<boxInfo>().row;
        int boxCol = box.GetComponent<boxInfo>().col;

        while (box.transform.position.x + speed * Time.deltaTime < goal)
        {
            for (int i = boxCol; i < emptyBoxCol; ++i)
                boxes[4 * (boxRow - 1) + i - 1].transform.position += Vector3.right * speed * Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        string[] symbols = new string[16];

        for (int i = 0; i < 16; ++i)
            symbols[i] = boxes[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text;

        for (int i = boxCol; i < emptyBoxCol; ++i)
        {
            boxes[4 * (boxRow - 1) + i].name = symbols[4 * (boxRow - 1) + i - 1];
            boxes[4 * (boxRow - 1) + i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = symbols[4 * (boxRow - 1) + i - 1];
        }

        Color imgColor = box.GetComponent<Image>().color;

        boxes[4 * (emptyBoxRow - 1) + emptyBoxCol - 1].GetComponent<Image>().color = imgColor;
        boxes[4 * (emptyBoxRow - 1) + emptyBoxCol - 1].transform.GetChild(0).GetComponent<Image>().color = imgColor;


        boxes[4 * (boxRow - 1) + boxCol - 1].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        boxes[4 * (boxRow - 1) + boxCol - 1].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);

        boxes[4 * (boxRow - 1) + boxCol - 1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";

        boxes[4 * (boxRow - 1) + boxCol - 1].name = box.name;
        box.name = "empty";

        emptyBoxCol = boxCol;
        emptyBoxRow = boxRow;

        checkCombination();
    }

    void checkCombination()
    {
        for (int i = 0; i < 16; ++i)
        {
            boxes[i].GetComponent<RectTransform>().offsetMax = Vector2.zero;
            boxes[i].GetComponent<RectTransform>().offsetMin = Vector2.zero;
        }

        letClick = true;
        if (boxes[boxes.Length - 1].transform.GetChild(0).GetChild(0).GetComponent<Text>().text != "")
            return;

        for (int i = 0; i < boxes.Length - 1; ++i)
            if (boxes[i].transform.GetChild(0).GetChild(0).GetComponent<Text>().text != (i + 1).ToString())
                return;

        button.SetActive(true);
        finished_game = true;
    }
}
