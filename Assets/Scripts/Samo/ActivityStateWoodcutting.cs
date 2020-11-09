﻿using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class ActivityStateWoodcutting : ActivityState
{
    private UnitCommandMove CommandMove2Resource;
    private UnitCommandGather CommandGatherFromResource;
    private UnitCommandMove CommandMove2Storage;
    private UnitCommandDrop CommandDrop2Storage;

    public ActivityStateWoodcutting(MapCell Target, Unit Unit) : base()
    {
        List<MapCell> Path2Resource = PathFinding.Instance.FindPath(Unit.CurrentCell, Target, PathFinding.EXCLUDE_LAST);

        this.CommandMove2Resource = new UnitCommandMove(Target, Path2Resource);

        this.CommandGatherFromResource = new UnitCommandGather(Target, Unit.SkillWoodcutting);
        this.CommandMove2Storage = null;
        this.CommandDrop2Storage = null;
    }
    public override IEnumerator PerformAction(Unit Unit)
    {
        if (Unit.CurrentCommand.IsDone(Unit))
        {
            // If Unit arrived next to resource, let's command it to gather
            if (Unit.CurrentCommand == this.CommandMove2Resource)
            {
                Unit.CurrentCommand = this.CommandGatherFromResource;
            }
            // If Unit is finished gathering (full inventory), let's command it to move to storage
            else if (Unit.CurrentCommand == this.CommandGatherFromResource)
            {
                (List<MapCell>, MapCell) Temp = PathFinding.Instance.FindPath(Unit.CurrentCell, MapControl.Instance.StorageList, PathFinding.EXCLUDE_LAST);
                List<MapCell> Path = Temp.Item1;
                MapCell ClosestStorage = Temp.Item2;

                // Oh no, it's not possible to get to any Storage?
                if (Path == null)
                {
                    Unit.SetActivity(new ActivityStateIdle());
                }
                // We found a path to Storage
                else
                {
                    this.CommandMove2Storage = new UnitCommandMove(ClosestStorage, Path);
                    Unit.CurrentCommand = this.CommandMove2Storage;
                }
            }
            // If unit has walked next to the storage, let's command it to drop resources to it
            else if (Unit.CurrentCommand == this.CommandMove2Storage)
            {
                this.CommandDrop2Storage = new UnitCommandDrop(this.CommandMove2Storage.Target);
                Unit.CurrentCommand = this.CommandDrop2Storage;
            }
            // If unit has dropped resources to the storage, let's command it to move to the resource source again
            else if (Unit.CurrentCommand == this.CommandDrop2Storage)
            {
                this.CommandMove2Resource = new UnitCommandMove(this.CommandMove2Resource.Target, PathFinding.Instance.FindPath(Unit.CurrentCell, this.CommandMove2Resource.Target, PathFinding.EXCLUDE_LAST));
                Unit.CurrentCommand = this.CommandMove2Resource;
            }
            else
            {
                //TODO
                throw new Exception("Unit's current command is something unexpected");
            }
            // If Unit is done doing something, we set new command to queue.
            // However, we were expected to do something because PerformAction was called, so we need to retry
            this.PerformAction(Unit);
        }
        else if (!Unit.CurrentCommand.CanBePerformed(Unit))
        {
            // If moving to resource is not possible
            if (Unit.CurrentCommand == this.CommandMove2Resource)
            {
                //TODO
            }
            // If gathering from resource is not possible
            else if (Unit.CurrentCommand == this.CommandGatherFromResource)
            {
                //TODO
            }
            // If moving to storage is not possible
            else if (Unit.CurrentCommand == this.CommandMove2Storage)
            {
                //TODO
            }
            // If dropping resources at storage is not possible
            else if (Unit.CurrentCommand == this.CommandDrop2Storage)
            {
                //TODO
            }
            else
            {
                //TODO
                throw new Exception("Unit's current command is something unexpected");
            }
        }
        else
        {
            yield return Unit.StartCoroutine(Unit.CurrentCommand.PerformAction(Unit));
        }
    }
    public override void InitializeCommand(Unit Unit)
    {
        // TODO if carrying capacity is full, first move to storage

        Unit.CurrentCommand = this.CommandMove2Resource;
    }
}