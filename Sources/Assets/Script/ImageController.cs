using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour {
    public GameObject target;//lưu GameObject có vị trí muốn chuyển tới
    public bool startMove = false;
    GameController GameMN;
	// Use this for initialization
	void Start () {
        GameObject Gamemanager = GameObject.Find("GameController");
        GameMN = Gamemanager.GetComponent<GameController>();
	}

    // Update is called once per frame
    void Update()
    {
        if (startMove)
        {
            startMove = false;//mở khóa di chuyển
            this.transform.position = target.transform.position;//chuyển tới nơi mới.
            GameMN.checkComplete = true;
        }
    }
}
