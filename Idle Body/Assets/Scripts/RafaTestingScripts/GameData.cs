using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData data;
    public float points;
    public bool organ0unlocked;
    public int organ0smallRedCells;
    public int organ0medRedCells;
    public int organ0bigRedCells;
    public float organ0smallRedCell0Health;
    public float organ0smallRedCell1Health;
    public float organ0smallRedCell2Health;
    public float organ0smallRedCell3Health;
    public float organ0smallRedCell4Health;
    public float organ0smallRedCell5Health;
    public float organ0smallRedCell6Health;
    public float organ0smallRedCell7Health;
    public float organ0smallRedCell8Health;
    public float organ0medRedCell0Health;
    public float organ0medRedCell1Health;
    public float organ0medRedCell2Health;

    // Start is called before the first frame update
    private void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
    }
   
}
