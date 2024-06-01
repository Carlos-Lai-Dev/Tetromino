using UnityEngine;

public class Move : MonoBehaviour
{
    private float previousTime;
    private float fallTime = 0.8f;
    private static int Width = 10;
    private static int Height = 20;
    //����һ��������������
    private static Transform[,] grid = new Transform[Width, Height];
    //����������һ����ά���� ��������������ĵ� ����ת���ģ�0��0��0��
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
            //transform.Translate() �ǳ������ĳһ�̶������ƶ�  ���������������ǹ̶���
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
            //transform.RotateAround���� �����尴��ĳ����������  ĳ��������   ��תָ���ĽǶ�
            //transform.TransformPoint���� ���������任Ϊ��Ӧ����������
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
        }

        //ÿ0.8s ִ��һ���������µĵ�λ�˶�  ����������°���  ���ÿ0.08s ִ��һ��
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;

            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddGrid();
                CheckForLine();
                //����ǰ���disable
                enabled = false;
                FindObjectOfType<SpawnTetromino>().Spawn();
            }
        }



    }

    private void CheckForLine()
    {
        //�������濪ʼ����ÿһ�� i = 19,18,17,.......3,2,1,0;
        for (int i = Height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }

        }

    }

    //�������ÿһ�У�����Ƿ���������
    private bool HasLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            if (grid[j, i] == null)
                return false;


        }
        return true;
    }
    //ɾ�������е����������壬���������е�����Ҳһ������
    private void DeleteLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            //
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }
    //����
    private void RowDown(int i)
    {
        //�������ű� �����鷢�ֿո� �ͽ���һ������
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
    //******����******
    bool ValidMove()
    {
        //�������ص�ǰ�ű������������
        foreach (Transform childTrans in transform)
        {
            //print(childTrans.position.x);
            //�������������������Ƿ��ڹ涨��Χ��
            int RoundX = Mathf.RoundToInt(childTrans.position.x);
            int RoundY = Mathf.RoundToInt(childTrans.position.y);
            //int RoundX = (int)childTrans.position.x;
            //int RoundY = (int)childTrans.position.y;

            if (RoundX < 0 || RoundX >= Width || RoundY < 0 || RoundY > Height)
                return false;
            //�������������  Ҳ����false
            if (grid[RoundX, RoundY] != null)
                return false;
        }
        return true;
    }
}
