using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMain : MonoBehaviour {
    
	void Start ()
    {
        Cat myCat = new Cat();
        Mouse myMouse1 = new Mouse("mouse1", myCat);
        Mouse myMouse2 = new Mouse("mouse2", myCat);
        Master myMaster = new Master(myCat);
        Master2 myLittleMaster = new Master2(myCat);

        myCat.Cry();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
