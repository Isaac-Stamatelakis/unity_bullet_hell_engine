using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Always sorted in descending order by maxTick
/// </summary>
public class SortedRequestList : IEnumerable<ScheduleRequest> {
    private List<ScheduleRequest> scheduleRequests = new List<ScheduleRequest>();
    public List<ScheduleRequest> ScheduleRequests {get{return scheduleRequests;}}
    public void add(ScheduleRequest newScheduleRequest) {
        int index = 0;
        for (int n = 0 ; n < scheduleRequests.Count; n++) {
            if (newScheduleRequest.MaxTick < scheduleRequests[n].MaxTick) {
                index = n+1;
            }
        }
        scheduleRequests.Insert(index,newScheduleRequest);
    }
    public ScheduleRequest get(int index) {
        return scheduleRequests[index];
    }
    public int Count {get{return scheduleRequests.Count;}}
    public IEnumerator<ScheduleRequest> GetEnumerator() {
        return scheduleRequests.GetEnumerator();
        }

    // Implementing IEnumerable to fulfill the interface
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
