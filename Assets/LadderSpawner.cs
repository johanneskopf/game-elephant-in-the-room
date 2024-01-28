using UnityEngine;

public class LadderSpawner : MonoBehaviour
{
    public GameObject ladderPrefab;
    public Vector3 spawnPosition;
    public float scale = 1f;

    public void Spawn()
    {
        var l = Instantiate(ladderPrefab);
        l.transform.position = spawnPosition + transform.position;
        l.transform.localScale = new Vector3(scale, scale, scale);
    }
}