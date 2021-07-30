using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explorerManager : MonoBehaviour
{
        private static explorerManager instance;
        [SerializeField]
        private List<GameObject> agents;

        void Awake()
        { 
                instance = this;
                //agents = new List<GameObject>();
        }

        public static void addToAgentList(GameObject obj)
        {
                instance.agents.Add(obj);
        }

        public static List<GameObject> getAgentsList()
        {
                return instance.agents;
        }
}
