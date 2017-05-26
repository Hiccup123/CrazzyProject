using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer
{
	public Observer (ModelBase childModel)
    {
        childModel.SubEvent += new ModelBase.SubEventHandler(Response);
    }

    public abstract void Response();
}

public abstract class Observer2
{
    public Observer2(ModelBase childModel)
    {
        childModel.SubEvent += new ModelBase.SubEventHandler(Response);
        childModel.SubEvent += new ModelBase.SubEventHandler(Response2);
    }

    public abstract void Response();
    public abstract void Response2();
}

public class Mouse : Observer
{
    private string name;

    public Mouse(string name, ModelBase childModel) : base(childModel)
    {
        this.name = name;
    }

    public override void Response()
    {
        Debug.Log(this.name + "开始逃跑");
    }
}

public class Master : Observer
{
    public Master(ModelBase childModel) : base (childModel)
    {

    }

    public override void Response()
    {
        Debug.Log("主人醒来");
    }
}

public class Master2 : Observer2
{
    public Master2 (ModelBase childModel) : base(childModel)
    {

    }

    public override void Response()
    {
        Debug.Log("Baby 醒来...");
    }

    public override void Response2()
    {
        Debug.Log("开始哭闹...");
    }
}
