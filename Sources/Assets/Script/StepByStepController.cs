using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepByStepController : MonoBehaviour {
    //hàng và cột hiện tại.
    public int row, col;
    GameController GameMN;  
    
    // Use this for initialization
    void Start () {
        //xác định game object và lấy các thành phần từ game object
        
        GameObject GameManager = GameObject.Find("GameController");
        GameMN = GameManager.GetComponent<GameController>();
    }
	
    void OnMouseDown()
    {
            GameMN.countStep += 1;
            GameMN.row = row;
            GameMN.col = col;
            GameMN.startControl = true;
    }
}
