using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class generate_level : MonoBehaviour
{
    public Image[] boxes;

    List<int> indexes;
    List<int> currentIndexes;

    void Start()
    {
        do
        {
            reset();
            for (int i = 0; i < 15; ++i)
            {
                int tmp = Random.Range(0, indexes.Count);
                boxes[i].gameObject.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = indexes[tmp].ToString();
                boxes[i].name = indexes[tmp].ToString();
                currentIndexes.Add(indexes[tmp]);
                indexes.RemoveAt(tmp);
            }
        } while (!isSolved());

        gameController.emptyBoxRow = 4;
        gameController.emptyBoxCol = 4;
    }

    void reset()
    {
        indexes = new List<int>();
        currentIndexes = new List<int>(); ;

        for (int i = 0; i < 15; ++i)
            indexes.Add(i + 1);
    }

    bool isSolved()
    {
        int countInversions = 0;

        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (currentIndexes[j] > currentIndexes[i])
                    countInversions++;
            }
        }

        return countInversions % 2 == 0 ;
    }
}
