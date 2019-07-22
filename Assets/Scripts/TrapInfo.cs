using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrapInfo : MonoBehaviour
{
    public TrapManager trapScript;
    public TextMeshProUGUI textbox;
    Camera mainCamera;
    Vector3 targetPoint;

    public float objectScale = 1.0f;
    private Vector3 initialScale;
    private float distance;

    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        initialScale = transform.localScale;
    }
    private void Update()
    {
        distance = Vector3.Distance(transform.position, mainCamera.transform.position);

        if(distance >= 30f)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = initialScale;
        }
        // rotating to face player
        targetPoint = mainCamera.transform.position;
        targetPoint.y = transform.position.y;
        transform.LookAt(targetPoint, Vector3.up);
        //transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);
        //transform.rotation = mainCamera.transform.rotation;
    }

    private void OnGUI()
    {
        if (trapScript.type == TrapManager.TrapType.Cooldown) {
            if (trapScript.CurTime != trapScript.trapCooldown) {
                textbox.text = trapScript.CurTime.ToString();
            }
            else {
                textbox.text = "";
            }
        }

        if(trapScript.type == TrapManager.TrapType.Fueled)
            textbox.text = trapScript.CurFuel.ToString() + "/" + trapScript.maxFuel.ToString();
    }
}
