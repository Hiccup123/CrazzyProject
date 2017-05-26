using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModelBase{

	public ModelBase ()
    {

    }

    public delegate void SubEventHandler();

    public event SubEventHandler SubEvent;

    protected void Nodify()
    {
        if(this.SubEvent != null)
        {
            this.SubEvent();
        }
    }
}

public class Cat : ModelBase
{
    public Cat()
    {

    }

    public void Cry()
    {
        Debug.Log("Cat Cry...");
        this.Nodify();
    }
}


