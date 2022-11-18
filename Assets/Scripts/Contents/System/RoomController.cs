using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Lock,
        Free,
    }

    [SerializeField]
    private State state;

    [SerializeField]
    private List<MonsterController> monsterList;

    [SerializeField]
    private List<Door> doorList;

    [SerializeField]
    private List<Trap> trapList;

    public void ChangeState(State state)
    {
        this.state = state;
    }

    public State GetRoomState() {
        return state;
    }

    public void AddMonster(MonsterController monster)
    {
        monsterList.Add(monster);
    }

    public void RemoveMonster(MonsterController monster)
    {
        monsterList.Remove(monster);
    }

    public void AddDoor(Door door)
    {
        doorList.Add(door);
    }

    public void RemoveDoor(Door door)
    {
        doorList.Remove(door);
    }

    public void AllDoorOpen()
    {
        for (var i = 0; i < doorList.Count; ++i)
        {
            doorList[i].Open();
        }
    }

    public void AllDoorClose()
    {
        for (var i = 0; i < doorList.Count; ++i)
        {
            doorList[i].Close();
        }
    }

    public void AddTrap(Trap trap)
    {
        trapList.Add(trap);
    }

    public void RemoveTrap(Trap trap)
    {
        trapList.Remove(trap);
    }

    public void AllPlayTrap()
    {
        for (var i = 0; i < trapList.Count; ++i)
        {
            trapList[i].Play();
        }
    }

    public void AllFreeTrap()
    {
        for (var i = 0; i < trapList.Count; ++i)
        {
            trapList[i].Free();
        }
    }




}
