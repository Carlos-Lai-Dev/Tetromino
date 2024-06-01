using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public GameObject[] tetrominoes;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn()
    {
        Instantiate(tetrominoes[Random.Range(0, tetrominoes.Length)], transform.position, Quaternion.identity);

    }
}
