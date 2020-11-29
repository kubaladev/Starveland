﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : CellObject
{
    [HideInInspector]
    public List<Resource> Resources=new List<Resource>();
    override protected  void Awake()
    {
        base.Awake();
        this.IsBlocking = true;
        this.IsSelectable = true;
    }

    public Resource GatherResource(int amount)
    {
        Resource Result = this.Resources[0].Subtract(amount);
        //Debug.LogWarning(Result.itemInfo.name);
        if (this.Resources[0].Amount <= 0)
        {
            Debug.Log("Destroying Resource Source");
            UnitManager.Instance.RemoveFromQueue(this);
            //this.CurrentCell.SetCellObject(null);
            this.CurrentCell.EraseCellObject(this);
            Destroy(this.gameObject);
        }
        if (Result == null)
        {
            Debug.LogWarning("Result=null v Gather resource");
        }
        return Result;
    }

    private void OnMouseOver()
    {
        if (GlobalGameState.Instance.InGameInputAllowed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                UnitManager.Instance.AddActionToQueue(this);
            }
        }
    }

    public List<ResourcePack> ResourcePacks;

    public void GenerateResources()
    {
        List<Resource> generatedResources = new List<Resource>();
        foreach (ResourcePack rp in ResourcePacks)
        {
            Resource toAddResource = rp.UnpackPack();
            if (toAddResource != null)
            {
                generatedResources.Add(rp.resource);
            }
                
        }
        this.Resources = generatedResources;
    }

    public void AddResource(Resource toAddResource)
    {
        bool added = false;
        for (int i = 0; i < Resources.Count; i++)
        {
            if (Resources[i].itemInfo.name.Equals(toAddResource.itemInfo.name))
            {
                Resources[i].AddDestructive(toAddResource);
                added = true;
                break;
            }
        }
        if (!added)
        {
            Resources.Add(toAddResource);
        }
    }
}
