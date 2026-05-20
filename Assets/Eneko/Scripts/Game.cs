using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Main";

    [SerializeField] private float winPuntution = 100f;
    [SerializeField] public float currentPuntution = 0f;
    [SerializeField] private float activationDuration = 3f;
    [SerializeField] private float currentActivationDuration = 3f;
    [SerializeField] private float timeBetweenActivations = 0f;

    [SerializeField] private GameObject[] cubes;

    [SerializeField] private TextMeshProUGUI puntuation;

    private Color idelColor = Color.white;
    private Color activateColor = Color.red;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeBetweenActivations = Random.Range(3f, 5f);
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<Renderer>().material.color = idelColor;
        }
    }

    void ActivateRandomCube()
    {
        if (timeBetweenActivations <= 0f)
        {
            CheckWinCondition();
            int randomIndex = Random.Range(0, cubes.Length);
            GameObject cubeToActivate = cubes[randomIndex];
            cubeToActivate.GetComponent<Cube>().isActivated = true;
            cubeToActivate.GetComponent<Renderer>().material.color = activateColor;
            StartCoroutine(CubeActivationCorrutine(cubeToActivate));
            timeBetweenActivations = Random.Range(3f, 5f);
        }
        else
        {
            timeBetweenActivations -= Time.deltaTime;
        }
    }

    private IEnumerator CubeActivationCorrutine(GameObject Cube)
    {
        WaitForSeconds wait = new WaitForSeconds(currentActivationDuration);
        yield return wait;
        Cube.GetComponent<Cube>().isActivated = false;
        Cube.GetComponent<Renderer>().material.color = idelColor;
        currentActivationDuration = activationDuration;
    }

    private void CheckWinCondition()
    {
        if (currentPuntution >= winPuntution)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActivateRandomCube();
        puntuation.text = "Puntuation: " + currentPuntution.ToString();
    }
}
