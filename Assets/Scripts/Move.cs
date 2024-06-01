using UnityEngine;

public class Move : MonoBehaviour
{
    private float previousTime;
    private float fallTime = 0.8f;
    private static int Width = 10;
    private static int Height = 20;
    //定义一个存放坐标的数组
    private static Transform[,] grid = new Transform[Width, Height];
    //物体中声明一个三维向量 就是其物体的中心的 及旋转中心（0，0，0）
    public Vector3 rotationPoint;

    private void Start()
    {
        //print(grid[0, 0]); 
        //print((int)(1.7f));//--0
        //print(Mathf.RoundToInt(1.7f));//--1
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //transform.Translate() 是朝物体的某一固定方向移动  方向对于物体而言是固定的
            //transform.Translate(new Vector2(-1, 0));
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            //transform.Translate(new Vector2(1, 0));
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //transform.RotateAround（） 让物体按照某个世界坐标  某个坐标轴   旋转指定的角度
            //transform.TransformPoint（） 将相对坐标变换为对应的世界坐标
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
        }

        //每0.8s 执行一次物体向下的单位运动  如果按下向下按键  变成每0.08s 执行一次
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;

            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddGrid();
                CheckForLine();
                //将当前组件disable
                enabled = false;
                FindObjectOfType<SpawnTetromino>().Spawn();
            }
        }



    }

    private void CheckForLine()
    {
        //从最上面开始遍历每一行 i = 19,18,17,.......3,2,1,0;
        for (int i = Height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }

        }

    }

    //遍历表的每一列，检查是否有完整行
    private bool HasLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            if (grid[j, i] == null)
                return false;


        }
        return true;
    }
    //删除完整列的所有子物体，并将数组中的数据也一并清理
    private void DeleteLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            //
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }
    //下移
    private void RowDown(int i)
    {
        //遍历整张表 如果检查发现空格 就将上一格下移
        for (int y = i; y < Height; y++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= Vector3.up;
                }

            }
        }
    }



    void AddGrid()
    {
        foreach (Transform childTrans in transform)
        {
            //print(childTrans.position.y);
            int RoundX = Mathf.RoundToInt(childTrans.position.x);
            int RoundY = Mathf.RoundToInt(childTrans.position.y);

            //int RoundX = (int)childTrans.position.x;
            //int RoundY = (int)childTrans.position.y;
            grid[RoundX, RoundY] = childTrans;
        }
    }
    //******核心******
    bool ValidMove()
    {
        //遍历挂载当前脚本物体的子物体
        foreach (Transform childTrans in transform)
        {
            //print(childTrans.position.x);
            //检查子物体的中心坐标是否在规定范围内
            int RoundX = Mathf.RoundToInt(childTrans.position.x);
            int RoundY = Mathf.RoundToInt(childTrans.position.y);
            //int RoundX = (int)childTrans.position.x;
            //int RoundY = (int)childTrans.position.y;

            if (RoundX < 0 || RoundX >= Width || RoundY < 0 || RoundY > Height)
                return false;
            //如果表中有数据  也返回false
            if (grid[RoundX, RoundY] != null)
                return false;
        }
        return true;
    }
}
