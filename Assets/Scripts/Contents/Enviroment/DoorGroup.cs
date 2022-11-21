using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGroup : Door
{
    public List<Door> doors;

    private int workDoorCount;

    public override void Open()
    {
        state = State.OpenWork;
        beginOpenEvent?.Invoke(this);

        workDoorCount =  doors.Count;

        for (var i = 0; i < doors.Count; ++i)
        {
            doors[i].endOpenEvent.AddListener(EndOpen);
            doors[i].Open();
        }
    }

    private void EndOpen(Door door)
    {
        --workDoorCount;
        door.endOpenEvent.RemoveListener(EndOpen);

        if(workDoorCount == 0) {
            endOpenEvent?.Invoke(this);
            state = State.Open;
        }
    }

    public override void Close()
    {
        state = State.CloseWork;
        beginCloseEvent?.Invoke(this);

        workDoorCount = doors.Count;

        for (var i = 0; i < doors.Count; ++i)
        {
            doors[i].endCloseEvent.AddListener(EndClose);
            doors[i].Close();
        }
    }

    private void EndClose(Door door)
    {
        --workDoorCount;
        door.endCloseEvent.RemoveListener(EndOpen);

        if (workDoorCount == 0)
        {
            endCloseEvent?.Invoke(this);
            state = State.Close;
        }
    }

}
