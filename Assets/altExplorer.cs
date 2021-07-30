using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altExplorer : MonoBehaviour
{

    public Grid grid;
    [SerializeField]private float turn_delay_time = .5f;
    private float timer;
    private bool communicated_last_turn = false;

    [SerializeField] private GameObject explorerMap;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = turn_delay_time;
        grid = Instantiate(explorerMap, Vector3.zero, Quaternion.identity, null).GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            TakeTurn();
            timer = turn_delay_time;
        }
        else timer -= Time.deltaTime;
    }
    private void TakeTurn()
    {
        List<Node> optimalMovements = new List<Node>();
        List<Node> secondaryMovements = new List<Node>();
        foreach (var node in grid.GetCardinalNeighbours(grid.NodeFromWorldPoint(transform.position)))
        {
            //checked in order: grid 1 EAST, grid 2 NORTH, grid 3 SOUTH, grid 4 WEST
            if (!communicated_last_turn)
            {
                foreach (var agent in explorerManager.getAgentsList())
                {
                    if (agent.transform.position == node.worldPos)
                    {
                        Grid neighbourGrid = agent.GetComponent<altExplorer>().grid;
                        grid.CompareWithOther(neighbourGrid);
                        communicated_last_turn = true;
                        Debug.Log("shared info with another agent!");
                        return;
                    }
                }
            }
            
            if (node.state != Node.State.UNWALKABLE)
            {
                if(node.state != Node.State.KNOWN && node.state != Node.State.INFORMED) optimalMovements.Add(node);
                else secondaryMovements.Add(node);
            }
        }
        
        if (optimalMovements.Count > 0)
        {
            Node chosenNode = optimalMovements[Random.Range(0, optimalMovements.Count)];
            transform.position = chosenNode.worldPos;
            chosenNode.known = true;
            chosenNode.state = Node.State.KNOWN;
        }
        else
        {
            Node chosenNode = secondaryMovements[Random.Range(0, secondaryMovements.Count)];
            transform.position = chosenNode.worldPos;
        }

        communicated_last_turn = false;
    }
}
