using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    // Get Car Container 

    public GameObject carContainer;
    private int carCount;
    private int currentCar;
    private GameObject mainCamera;
    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        //initialize our values
        mainCamera = gameObject;
        carCount = carContainer.transform.childCount;

        // attach camera to random car
        AttachToCar();
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        //print(counter);
        if (counter > 60)
        {
            counter = 0;
            AttachToCar();
        }
    }

    void AttachToCar()
    {
        currentCar = Random.Range(0, carCount - 1);
        GameObject car = carContainer.transform.GetChild(currentCar).gameObject;
        var test = car.name;
        
        while (car.name.Contains("Bus") || car.name.Contains("GranFury") || car.name.Contains("Gontijo") || car.name.Contains("Furgao") || car.name.Contains("Truck"))
        {
            currentCar = Random.Range(0, carCount - 1);
            car = carContainer.transform.GetChild(currentCar).gameObject;
            test = car.name;

        }




        mainCamera.transform.SetParent(car.transform);
        mainCamera.transform.localPosition = new Vector3(0, 1.3f, 1f);
        mainCamera.transform.localEulerAngles = new Vector3(-5f, 0, 0);
    }
}
