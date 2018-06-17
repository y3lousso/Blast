using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCubeSpawner : MonoBehaviour {

    [SerializeField] private TargetCube targetCubeBluePrefab;
    [SerializeField] private TargetCube targetCubeRedPrefab;
    [SerializeField] private Wall topWallPrefab;
    [SerializeField] private Wall sideWallPrefab;


    public void SpawnTargetCubes(List<TargetCubeData> targetCubesData)
    {
        foreach(TargetCubeData targetCubeData in targetCubesData)
        {
            Vector3 calculatedPosition = transform.position + new Vector3((float)targetCubeData.horizontalPosition * 0.25f, (float)targetCubeData.vecticalPosition * 0.25f, 0f);
            Vector3 orientation = new Vector3(0, 0, (float)targetCubeData.orientation * 90f);
            Quaternion calculatedRotation = Quaternion.Euler(orientation);
            TargetCube targetCube;
            if (targetCubeData.cubeColor == CubeColor.Blue)
            {
                targetCube = Instantiate(targetCubeBluePrefab, calculatedPosition, calculatedRotation);               
            }
            else
            {
                targetCube = Instantiate(targetCubeRedPrefab, calculatedPosition, calculatedRotation);
            }
            targetCube.cubeColor = targetCubeData.cubeColor;

            GameManager.Instance.results.MaxHit++;
        }
    }

    public void SpawnWall(WallData wallData)
    {
        if (wallData != null)
        {
            Wall wall;
            if (wallData.position == WallPosition.Top)
            {
                wall = Instantiate(topWallPrefab, transform.position + new Vector3(0, 1f, 0), transform.rotation);
            }
            else
            {
                Vector3 calculatedPosition = transform.position + new Vector3((float)wallData.position * 0.25f, .8f, 0f);
                wall = Instantiate(sideWallPrefab, calculatedPosition, transform.rotation);
            }

            wall.transform.localScale += new Vector3(0,0, 2*wallData.length);
            wall.transform.Translate(0, 0, wallData.length);
        }
    }


}


