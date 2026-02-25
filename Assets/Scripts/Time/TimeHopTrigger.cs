using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHopTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ต้องเช็ค Tag ว่าเป็น Player เท่านั้นถึงจะเปลี่ยนเวลา
        if (other.CompareTag("Player"))
        {
            TimeManager.Instance.AdvanceTime();
            Debug.Log("Time Hopped!");
        }
    }
}