using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstatiationScheduleUnitTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //sortedRequestTest();

        /*
        Test one expected results are 
        2|5
        5|10
        10|30
        35|50
        50|100
        */
        Debug.Log("test1");
        runInstatiationTest(new List<SpawnAction> {
            new SpawnAction() {
                executeTick=10,
                spawnAmount=6,
                prefab = Resources.Load<GameObject>("Prefabs/testobject1")
            },
            new SpawnAction() {
                executeTick=20,
                spawnAmount=10,
                prefab = Resources.Load<GameObject>("Prefabs/testobject1")
            },
            new SpawnAction() {
                executeTick=50,
                spawnAmount=30,
                prefab = Resources.Load<GameObject>("Prefabs/testobject1")
            },
            new SpawnAction() {
                executeTick=100,
                spawnAmount=100,
                prefab = Resources.Load<GameObject>("Prefabs/testobject1")
            },
            new SpawnAction() {
                executeTick=30,
                spawnAmount=40,
                prefab = Resources.Load<GameObject>("Prefabs/testobject1")
            }
        });
        /*
        Test two expected results are 
        4|5
        49|50
        50|100
        */
        Debug.Log("test2");
        runInstatiationTest(new List<SpawnAction> {
            new SpawnAction() {
                executeTick=100,
                spawnAmount=100,
                prefab = Resources.Load<GameObject>("Prefabs/testobject1")
            },
            new SpawnAction() {
                executeTick=50,
                spawnAmount=1,
                prefab = Resources.Load<GameObject>("Prefabs/testobject1")
            },
            new SpawnAction() {
                executeTick=5,
                spawnAmount=2,
                prefab = Resources.Load<GameObject>("Prefabs/testobject1")
            }
        });

    }

    protected void runInstatiationTest(List<SpawnAction> spawnActions) {
        LinkedList<SimultaneousActions> actions = new LinkedList<SimultaneousActions>();
        foreach (SpawnAction spawnAction in spawnActions) {
            SimultaneousActions simultaneousActions = new SimultaneousActions(new List<ReadableAction>{spawnAction},(int)spawnAction.executeTick);
            actions.AddLast(simultaneousActions);
        }
        LinkedList<InstatiationRequest> schedule = InstantiationScheduleGenerator.generateSchedule(actions,2);
        foreach (InstatiationRequest instatiationRequest in schedule) {
            Debug.Log(instatiationRequest.StartTime+ "|" + instatiationRequest.FinishTime);
        }
    }
    
    protected void sortedRequestTest() {
        SortedRequestList scheduleRequests = new SortedRequestList();
        scheduleRequests.add(new ScheduleRequest(10,6,Resources.Load<GameObject>("Prefabs/testobject1")));
        scheduleRequests.add(new ScheduleRequest(20,10,Resources.Load<GameObject>("Prefabs/testobject1")));
        scheduleRequests.add(new ScheduleRequest(50,30,Resources.Load<GameObject>("Prefabs/testobject1")));
        scheduleRequests.add(new ScheduleRequest(100,100,Resources.Load<GameObject>("Prefabs/testobject1")));
        scheduleRequests.add(new ScheduleRequest(30,40,Resources.Load<GameObject>("Prefabs/testobject1")));
        /**
        Test one expected results are 
        100|100
        50|30
        30|40
        20|10
        10|6
        **/
        foreach (ScheduleRequest scheduleRequest in scheduleRequests) {
            Debug.Log(scheduleRequest.MaxTick + "|" + scheduleRequest.MaxAmount);
        }
        

    }
}
