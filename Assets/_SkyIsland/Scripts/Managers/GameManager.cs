using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private GameObject currentPlayer;

    void Start()
    {
        SpawnPlayer(true);
    }

    public void SpawnPlayer(bool playIntro)
    {
        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        currentPlayer.SetActive(true);

        if (playIntro)
            cameraFollow.Init(currentPlayer.transform);
        else
            cameraFollow.SetTargetInstant(currentPlayer.transform);
    }

    public void PlayerDied()
    {
        Destroy(currentPlayer);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);

        SpawnPlayer(false); // intro yok, direkt follow
    }
}
