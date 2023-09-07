using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] Transform prefabPlates;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private PlatesCounter platesCounter;
    private List<GameObject> plateVisualGameObjectList;


    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateDestroyed += PlatesCounter_OnPlateDestroyed;
    }

    

    private void PlatesCounter_OnPlateDestroyed(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
        
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
       Transform playeVisualTransform = Instantiate(prefabPlates,counterTopPoint);
        float plateOffsetY = .1f;
        playeVisualTransform.localPosition = new Vector3(0,plateOffsetY * plateVisualGameObjectList.Count,0);
        plateVisualGameObjectList.Add(playeVisualTransform.gameObject);

    }
}
