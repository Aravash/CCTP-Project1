using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = UnityEngine.Random;

public class explorer : MonoBehaviour
{

    public Grid grid;
    private float movespeed = .1f;
    private float neighbourCheckRadius = 3f;
    private int layermask = 1 << 8; //unwalkable layer
    private int neighbourmask = 1 << 10;
    private float neighbourCommCooldown = 0;
    [SerializeField] private GameObject explorerMap;
    private Vector3[] inspectionPoints;

    private void Start()
    {
        grid = Instantiate(explorerMap, Vector3.zero, Quaternion.identity, null).GetComponent<Grid>();
        inspectionPoints = new[]{Vector3.zero, Vector3.forward, Vector3.back, Vector3.left, Vector3.right, 
            Vector3.right + Vector3.forward, Vector3.left + Vector3.forward, Vector3.right + Vector3.back, Vector3.left + Vector3.back};
    }

    // Update is called once per frame
    void Update()
    {
        if(blockadeInWay()) changeDirection();
        
        travelForward();

        /*
        if (neighbourCommCooldown <= 0) checkForNearbyAgents();
        else neighbourCommCooldown -= Time.deltaTime;
        */

        if(neighbourCommCooldown > 0) neighbourCommCooldown -= Time.deltaTime;

        noteEnvironment();
    }

    void noteEnvironment()
    {
        foreach (Vector3 point in inspectionPoints)
        {
            if (!grid.NodeFromWorldPoint(transform.position + point).known)
                grid.NodeFromWorldPoint(transform.position + point).known = true;
        }
    }

    void checkForNearbyAgents()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, neighbourCheckRadius, Vector3.forward, out hit, 0, neighbourmask))
        {
            Debug.Log("wabalooga");
            Grid neighbourGrid = hit.collider.gameObject.GetComponent<Grid>();
            grid.CompareWithOther(neighbourGrid);
            neighbourCommCooldown = 5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (neighbourCommCooldown > 0) return;
        if (other.CompareTag("explorer"))
        {
            Debug.Log("wabalooga");
            Grid neighbourGrid = other.gameObject.GetComponent<explorer>().grid;
            grid.CompareWithOther(neighbourGrid);
            neighbourCommCooldown = 5f;
        }
    }

    bool blockadeInWay()
    {
        return Physics.Raycast(transform.position, transform.forward, .8f, layermask);
    }
    
    void travelForward()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, movespeed);
    }
    
    void changeDirection()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(0, Random.Range(90, 135), 0);
    }
}
