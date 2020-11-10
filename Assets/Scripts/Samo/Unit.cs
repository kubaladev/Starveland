﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : CellObject
{
    public float movementSpeed=2f;

    public UnitCommand CurrentCommand { get; set; }
    public Resource CarriedResource = new Resource();
    private ActivityState CurrentActivity;

    public Dictionary<SkillType, Skill> Skills = new Dictionary<SkillType, Skill> {
        { SkillType.woodcutting, new SkillWoodcutting() }
    };


    public void SetActivity(ActivityState Activity)
    {
        this.CurrentActivity = Activity;
        Activity.InitializeCommand(this);
        if (Activity is ActivityStateIdle)
        {
            UnitManager.Instance.AddUnitToIdleList(this);
        }
    }
    protected override void Awake()
    {
        base.Awake();
        this.SetActivity(new ActivityStateIdle());
    }
    protected override void Start()
    {
        StartCoroutine("ControlUnit");
        objectName = NameGenerator.GetRandomName();
    }
    public IEnumerator ControlUnit()
    {
        while(true)
        {
            yield return StartCoroutine(this.CurrentActivity.PerformAction(this));
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator MoveUnitToNextPosition(MapCell TargetCell)
    {

        // TODO - Will have to be more sophisticated
        //set cell to be used by unit, free the old cell
        MapCell PreviousCell = this.CurrentCell;
        this.CurrentCell.SetCellObject(null);
        TargetCell.SetCellObject(this.gameObject);

        //calculate distance and movement vector
        float distance;
        distance = Vector3.Distance(transform.position, TargetCell.position);
        Vector3 movementVector = TargetCell.position - transform.position;
        movementVector = movementVector.normalized;

        //turn on animations
        ScalingAnim(true);
        RotationAnim(true);

        //Flip unit towards position
        if (TargetCell.position.x > transform.position.x)
        {
            Flip("right");
        }
        else if (TargetCell.position.x < transform.position.x)
        {
            Flip("left");
        }

        //initiate ingame movement process
        while (distance > 0.5f)
        {
            transform.position += movementVector * movementSpeed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
            distance = Vector3.Distance(transform.position, TargetCell.position);
        }
        //turn off animations
        ScalingAnim(false);
        RotationAnim(false);

        //center units position to cells position
        transform.position = TargetCell.position;
    }
    public IEnumerator GatherResource(ResourceSource target, float GatheringTime)
    {
       /* if (itemInHand == null)
        {*/
            Debug.Log("Preparing the axe");
            yield return new WaitForSeconds(GatheringTime);
            Debug.Log("Gathering object");
            //itemInHand = target.Gather();
            target.Flash();
            yield return new WaitForSeconds(0.2f);
       /* }
        else
        {
            Debug.Log("my hand is full");
        }*/
    }
    
    public IEnumerator StoreResource(BuildingStorage target)
    {
        /*if (itemInHand != null)
        {
            Debug.Log("Storing resource with name:" + itemInHand.name);
            Resource storedResource=itemInHand;
            itemInHand = null;
            return storedResource;
        }*/
        Debug.Log("About to drop");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Dropping resources");
        //itemInHand = target.Gather();
        target.Flash();
        yield return new WaitForSeconds(0.2f);
    }
    public IEnumerator BeIdle()
    {
        /*if (itemInHand != null)
        {
            Debug.Log("Storing resource with name:" + itemInHand.name);
            Resource storedResource=itemInHand;
            itemInHand = null;
            return storedResource;
        }*/
        Debug.Log("About to do idle fun");
        yield return new WaitForSeconds(1.0f);
        Debug.Log("I'm idling");
        //itemInHand = target.Gather();
        yield return new WaitForSeconds(0.2f);
    }
}
