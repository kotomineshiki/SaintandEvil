using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour ,ISceneController{
    //public  Camera viewport;
    // Use this for initialization

    
    public string gameState="";
    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        LoadResources();
    }
	void Start () {


    }
    void OnGUI()
    {
        if (gameState == "Win")
        {
            GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 30), "WIN");
            if(GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.5f, 100, 30),"REstart"))
            {
                restart();
            }
        }
        if (gameState == "Lose")
        {
            GUI.Label(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 100, 30), "saint KILLED");
            if (GUI.Button(new Rect(Screen.width * 0.7f, Screen.height * 0.5f, 100, 30),"REstart"))
            {
                restart();
            }
        }
        
    }
    public void LoadResources()
    {
        this.GetComponent<GenGameObject>().generate();
    }
    public void restart()
    {

    }
    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire1"))
        {


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                //Debug.Log("!");
                var todo = hit.collider;
                switch (todo.transform.tag)
                {
                    case "saint":
                        todo.GetComponent<CharacterMoving>().move();
                        break;
                    case "devil":
                        todo.GetComponent<CharacterMoving>().move();
                        break;
                    case "boat":
                        todo.GetComponent<Boat>().move();
                        break;
                }

            }
        }
        switch (checkWin())
        {
            case 1:
                Debug.Log("Wins");
                gameState = "Win";
                break;
            case 2:
                Debug.Log("Saints defeated");
                gameState = "Lose";
                break;

        }

    }
    int checkWin()//检查游戏结果
    {
        int AsideSaintsCount=0;
        int BsideSaintsCount=0;
        int AsideDevilsCount = 0;
        int BsideDevilsCount = 0;

        int BoatSaintsCount = 0;
        int BoatDevilsCount = 0;
        foreach(var i in this.GetComponent<GenGameObject>().saints)
        {
            if (i.GetComponent<CharacterMoving>().status == "Bside"|| i.GetComponent<CharacterMoving>().status == "BoatToBside"|| i.GetComponent<CharacterMoving>().status == "BsideToBoat") BsideSaintsCount++;
            if (i.GetComponent<CharacterMoving>().status == "Aside"|| i.GetComponent<CharacterMoving>().status == "BoatToAside" || i.GetComponent<CharacterMoving>().status == "AsideToBoat") AsideSaintsCount++;

            if (i.GetComponent<CharacterMoving>().status == "Boat") BoatSaintsCount++;
        }

        foreach (var i in this.GetComponent<GenGameObject>().devils)
        {
            if (i.GetComponent<CharacterMoving>().status == "Bside"|| i.GetComponent<CharacterMoving>().status == "BoatToBside" || i.GetComponent<CharacterMoving>().status == "BsideToBoat") BsideDevilsCount++;
            if (i.GetComponent<CharacterMoving>().status == "Aside"|| i.GetComponent<CharacterMoving>().status == "BoatToAside" || i.GetComponent<CharacterMoving>().status == "AsideToBoat") AsideDevilsCount++;

            if (i.GetComponent<CharacterMoving>().status == "Boat") BoatDevilsCount++;
        }
        string bstatus = this.GetComponent<GenGameObject>().boat.GetComponent<Boat>().status;
        if (BsideSaintsCount + BsideDevilsCount == 6) return 1;//全员到达B
        if (bstatus == "Aside")
        {
            
            AsideSaintsCount += BoatSaintsCount;
            AsideDevilsCount += BoatDevilsCount;
       //     Debug.Log("A"+BoatSaintsCount);
        }
        if (bstatus == "Bside")
        {
            BsideSaintsCount += BoatSaintsCount;
            BsideDevilsCount += BoatDevilsCount;
        }

        if (BsideSaintsCount != 0 && BsideSaintsCount < BsideDevilsCount) {
            Debug.Log(BsideSaintsCount);
            return 2;
        }//牧师人数比魔鬼少
        //牧师被杀了
        if (AsideSaintsCount != 0 && AsideSaintsCount < AsideDevilsCount)
        {
            Debug.Log(AsideSaintsCount);
            return 2;
        }
        return 0;//无事发生
    }

}
