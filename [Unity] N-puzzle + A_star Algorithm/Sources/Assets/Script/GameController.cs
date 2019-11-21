using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameController : MonoBehaviour {
    public int level;//lưu level
    public int row, col, countStep;
    public int rowBlank, colBlank;
    public int sizeRow, sizeCol;
    int countPoint = 0;
    int countImageKey = 0;//kiem tra ket qua
    int countComplete = 0;
   
    public bool startControl = false;
    public bool checkComplete;
    public bool gameIsComplete=false;   

    GameObject temp;

    public List<GameObject> imageKeyList;
    public List<GameObject> imageOfPictureList;
    public List<GameObject> checkPointList;

    GameObject[,] imageKeyMatrix;
    GameObject[,] imageOfPictureMatrix;
    GameObject[,] checkPointMatrix;

    //AI
    public int[,] CurrentMatrix;
   
	// Use this for initialization
	void Start () {
        imageKeyMatrix = new GameObject[sizeRow, sizeCol];
        imageOfPictureMatrix = new GameObject[sizeRow, sizeCol];
        checkPointMatrix = new GameObject[sizeRow, sizeCol];
        if (level == 1)
        {
            ImageOfEasyLevel();
        }
        if(level==2)
        {
            ImageOfNormalLevel();
        }
        if(level==3)
        {
            ImageOfHardLevel();
        }
        
        //call function
        CheckPointManager();
        ImageKeyManager();

        for (int r = 0; r < sizeRow; r++)
        {
            for (int c = 0; c < sizeCol; c++)
            {
               if( imageOfPictureMatrix[r, c].name.CompareTo("0") == 0)
                {
                    rowBlank = r;
                    colBlank = c;
                    break;
                }

            }
        }

        CurrentMatrix = new int[sizeRow, sizeCol];
        for (int r = 0; r < sizeRow; r++)
        {
            for (int c = 0; c < sizeCol; c++)
            {
                CurrentMatrix[r, c] = Convert.ToInt32(imageOfPictureMatrix[r, c].gameObject.name);
            }
        }
    }
    void CheckPointManager()
    {
        for (int r = 0; r < sizeRow; r++)
        {//run row
            for (int c = 0; c < sizeCol; c++)
            {//run col
                checkPointMatrix[r, c] = checkPointList[countPoint];
                countPoint++;
            }
        }
    }
	void ImageKeyManager(){
        for(int r=0;r<sizeRow;r++)
        {
            for(int c=0;c<sizeCol;c++)
            {
                imageKeyMatrix[r, c] = imageKeyList[countImageKey];
                countImageKey++;
            }
        }
    }
    // Update is called once per frame
    void Update () {
            //bat dau di chuyen
            if (startControl)
            {//move for image of picture

                startControl = false;
                if (countStep == 1)
                {
                    if (imageOfPictureMatrix[row, col] != null && imageOfPictureMatrix[row, col].name.CompareTo("0") != 0)//check if position is image or not image blank
                    {
                        if (rowBlank != row && colBlank == col)
                        {
                            if (Mathf.Abs(row - rowBlank) == 1)
                            {
                                SortImage();
                                countStep = 0;
                            }
                            else
                                countStep = 0;
                        }
                        else if (rowBlank == row && colBlank != col)
                        {
                            if (Mathf.Abs(col - colBlank) == 1)
                            {
                                SortImage();
                                countStep = 0;
                            }
                            else
                                countStep = 0;
                        }
                        else if ((rowBlank == row && colBlank == col) || (rowBlank != row && colBlank != col))
                        {
                            countStep = 0;
                        }
                    }
                    else
                        countStep = 0;
                }
                else
                    countStep = 0;
            }

        
    }
    public void ProcessSoft()
    {
        
        if (countStep == 1)
        {
            if (imageOfPictureMatrix[row, col] != null && imageOfPictureMatrix[row, col].name.CompareTo("0") != 0)//check if position is image or not image blank
            {
                if (rowBlank != row && colBlank == col)
                {
                    if (Mathf.Abs(row - rowBlank) == 1)
                    {
                        SortImage();
                        countStep = 0;
                    }
                    else
                        countStep = 0;
                }
                else if (rowBlank == row && colBlank != col)
                {
                    if (Mathf.Abs(col - colBlank) == 1)
                    {
                        SortImage();
                        countStep = 0;
                    }
                    else
                        countStep = 0;
                }
                else if ((rowBlank == row && colBlank == col) || (rowBlank != row && colBlank != col))
                {
                    countStep = 0;
                }
            }
            else
                countStep = 0;
        }
        else
            countStep = 0;
    }
    
    void FixedUpdate()
    {
        if (checkComplete)
        {
            checkComplete = false;
            for(int r=0;r<sizeRow;r++)
            {
                for(int c=0;c<sizeCol;c++)
                {
                    if (imageKeyMatrix[r, c].gameObject.name.CompareTo(imageOfPictureMatrix[r, c].gameObject.name) == 0)
                    {
                        countComplete++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (countComplete == checkPointList.Count)//if 9 imageOfPicture==9 imagekey (in 2 array) (checkPointList.count=9)
            {
                gameIsComplete = true;
                Application.LoadLevel("WinScreen");
            }
            else
            {
                countComplete = 0;
            }   
        }
    }
    void SortImage()
    {
        
            //sort
            temp = imageOfPictureMatrix[rowBlank, colBlank];//select image blank and save it at temp variable
            imageOfPictureMatrix[rowBlank, colBlank] = null;

            imageOfPictureMatrix[rowBlank, colBlank] = imageOfPictureMatrix[row, col];//select image is not image blank and save it at new position
            imageOfPictureMatrix[row, col] = null;

            imageOfPictureMatrix[row, col] = temp;
            //set move for image;
            imageOfPictureMatrix[rowBlank, colBlank].GetComponent<ImageController>().target = checkPointMatrix[rowBlank, colBlank];//set new point for image blank
            imageOfPictureMatrix[row, col].GetComponent<ImageController>().target = checkPointMatrix[row, col];

            imageOfPictureMatrix[rowBlank, colBlank].GetComponent<ImageController>().startMove = true;
            imageOfPictureMatrix[row, col].GetComponent<ImageController>().startMove = true;

            rowBlank = row;//position touch
            colBlank = col;
            //For AI
            for (int r = 0; r < sizeRow; r++)
            {
                for (int c = 0; c < sizeCol; c++)
                {
                    CurrentMatrix[r, c] = Convert.ToInt32(imageOfPictureMatrix[r, c].gameObject.name);
                }
            }
    }
    void ImageOfEasyLevel()
    {
        imageOfPictureMatrix[0,0]= imageOfPictureList[0];
        imageOfPictureMatrix[0,1]= imageOfPictureList[2];
        imageOfPictureMatrix[0,2]= imageOfPictureList[5];
        imageOfPictureMatrix[1,0]= imageOfPictureList[4];
        imageOfPictureMatrix[1,1]= imageOfPictureList[1];
        imageOfPictureMatrix[1,2]= imageOfPictureList[7];
        imageOfPictureMatrix[2,0]= imageOfPictureList[3];
        imageOfPictureMatrix[2,1]= imageOfPictureList[6];
        imageOfPictureMatrix[2,2]= imageOfPictureList[8];
    }
    void ImageOfNormalLevel()
    {
        imageOfPictureMatrix[0, 0] = imageOfPictureList[4];
        imageOfPictureMatrix[0, 1] = imageOfPictureList[0];
        imageOfPictureMatrix[0, 2] = imageOfPictureList[1];
        imageOfPictureMatrix[0, 3] = imageOfPictureList[2];
        imageOfPictureMatrix[1, 0] = imageOfPictureList[8];
        imageOfPictureMatrix[1, 1] = imageOfPictureList[6];
        imageOfPictureMatrix[1, 2] = imageOfPictureList[7];
        imageOfPictureMatrix[1, 3] = imageOfPictureList[11];
        imageOfPictureMatrix[2, 0] = imageOfPictureList[12];
        imageOfPictureMatrix[2, 1] = imageOfPictureList[5];
        imageOfPictureMatrix[2, 2] = imageOfPictureList[14];
        imageOfPictureMatrix[2, 3] = imageOfPictureList[10];
        imageOfPictureMatrix[3, 0] = imageOfPictureList[13];
        imageOfPictureMatrix[3, 1] = imageOfPictureList[9];
        imageOfPictureMatrix[3, 2] = imageOfPictureList[15];
        imageOfPictureMatrix[3, 3] = imageOfPictureList[3];
    }
    void ImageOfHardLevel()
    {
        imageOfPictureMatrix[0,0] = imageOfPictureList[5];
        imageOfPictureMatrix[0,1] = imageOfPictureList[2];
        imageOfPictureMatrix[0,2] = imageOfPictureList[3];
        imageOfPictureMatrix[0,3] = imageOfPictureList[4];
        imageOfPictureMatrix[0,4] = imageOfPictureList[9];
        imageOfPictureMatrix[1,0] = imageOfPictureList[10];
        imageOfPictureMatrix[1,1] = imageOfPictureList[1];
        imageOfPictureMatrix[1,2] = imageOfPictureList[12];
        imageOfPictureMatrix[1,3] = imageOfPictureList[7];
        imageOfPictureMatrix[1,4] = imageOfPictureList[8];
        imageOfPictureMatrix[2,0] = imageOfPictureList[15];
        imageOfPictureMatrix[2,1] = imageOfPictureList[6];
        imageOfPictureMatrix[2,2] = imageOfPictureList[13];
        imageOfPictureMatrix[2,3] = imageOfPictureList[14];
        imageOfPictureMatrix[2,4] = imageOfPictureList[19];
        imageOfPictureMatrix[3,0] = imageOfPictureList[20];
        imageOfPictureMatrix[3,1] = imageOfPictureList[11];
        imageOfPictureMatrix[3,2] = imageOfPictureList[22];
        imageOfPictureMatrix[3,3] = imageOfPictureList[17];
        imageOfPictureMatrix[3,4] = imageOfPictureList[18];
        imageOfPictureMatrix[4,0] = imageOfPictureList[21];
        imageOfPictureMatrix[4,1] = imageOfPictureList[16];
        imageOfPictureMatrix[4,2] = imageOfPictureList[23];
        imageOfPictureMatrix[4,3] = imageOfPictureList[24];
        imageOfPictureMatrix[4,4] = imageOfPictureList[0];

    }

}
