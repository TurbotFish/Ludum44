using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayZone : MonoBehaviour
{
    public float baseScale = 15;
    public float reductionSpeed = 0.1f;
    public float timeBeforeDeath = 3.5f;
    public float minX,maxX,minY,maxY;
    public Vector2 endCoordinates;
    public bool progressing;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (progressing && transform.localScale.x > 0.2f)
        {
            transform.localScale -= new Vector3(reductionSpeed * Time.deltaTime, reductionSpeed * Time.deltaTime, 0);
        }
    }

    public void Reset()
    {
        transform.localScale = new Vector3(baseScale, baseScale, 1);
        endCoordinates = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        transform.position = new Vector3(endCoordinates.x, 1, endCoordinates.y);
        //progressing = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("bonhomme"))
        {
            BonhommeController b = other.GetComponent<BonhommeController>();
            b.inZone = true;
            StartCoroutine(b.InZone(timeBeforeDeath));
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonhomme"))
        {
            BonhommeController b = other.GetComponent<BonhommeController>();
            b.inZone = false;
            StopCoroutine(b.InZone(timeBeforeDeath));
        }

    }
}
