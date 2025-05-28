using UnityEngine;
using Unity.Netcode;

public class CubeSpawner : NetworkBehaviour
{
    public GameObject redCubePrefab;
    public GameObject greenCubePrefab;
    public GameObject blueCubePrefab;
    public GameObject yellowCubePrefab;

    public int cubeCountPerColor = 10;
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnCubes(redCubePrefab);
            SpawnCubes(greenCubePrefab);
            SpawnCubes(blueCubePrefab);
            SpawnCubes(yellowCubePrefab);
        }
    }

    private void SpawnCubes(GameObject prefab)
    {
        Collider dropBox = GameObject.FindGameObjectWithTag("DropBox").GetComponent<Collider>();

        for (int i = 0; i < cubeCountPerColor; i++)
        {
            Vector3 spawnPosition;
            int maxAttempts = 20;
            int attempts = 0;

            // Keep generating a position until it doesn't overlap the DropBox
            do
            {
                spawnPosition = spawnAreaCenter + new Vector3(
                    Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                    1f,
                    Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
                );

                attempts++;

            } while (dropBox.bounds.Contains(spawnPosition) && attempts < maxAttempts);

            GameObject cube = Instantiate(prefab, spawnPosition, Quaternion.identity);
            cube.GetComponent<NetworkObject>().Spawn();
        }
    }
}
