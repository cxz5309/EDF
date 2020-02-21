using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public int upOrdown;//0:상 1:하
    public int type;// 0:노트x 1:노트 2:양쪽노트 4:롱노트토글 5:롱노트 진행
    public int order;

    public Enemy(int upOrdown, int type, int order)
    {
        this.upOrdown = upOrdown;
        this.type = type;
        this.order = order;
    }
}
