using UnityEngine;
using System.Collections;

public class CameraPanTrigger : MonoBehaviour
{
    public Transform targetToLook; // ลากเสาธงมาใส่ตรงนี้
    public float lookDuration = 2.0f; // จะให้มองนานกี่วินาที

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(PanSequence());
        }
    }

    IEnumerator PanSequence()
    {
        SimpleCameraFollow cam = FindFirstObjectByType<SimpleCameraFollow>();
        if (cam != null)
        {
            // 1. สั่งกล้องให้มองเสาธง
            cam.StartCinematic(targetToLook, cam.defaultSize); // ขนาดปกติ ไม่ต้องซูม

            // 2. รอเวลาให้คนเล่นดูเสา
            yield return new WaitForSeconds(lookDuration);

            // 3. คืนค่ากล้องกลับมาที่ผู้เล่น
            cam.StopCinematic();
        }
    }
}