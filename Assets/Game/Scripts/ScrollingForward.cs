using UnityEngine;
using System.Collections;

public class ScrollingForward : MonoBehaviour
{
    [SerializeField] private Transform backFloor;
    [SerializeField] private Transform currFloor;
    [SerializeField] private Transform frontFloor;

    [SerializeField] private float scrollSpeed;
    [SerializeField] private float roomSize; 

    private float movement = 0.0f;

    void Update()
    {
        float posDelta = scrollSpeed * Time.deltaTime;
        movement += posDelta;

        this.MoveFloor(this.backFloor,  posDelta);
        this.MoveFloor(this.currFloor,  posDelta);
        this.MoveFloor(this.frontFloor, posDelta);

        if(movement >= roomSize)
        {
            movement = 0.0f;
            Vector3 toFront = this.backFloor.transform.localPosition;
            toFront.z = roomSize * 2;
            this.backFloor.transform.localPosition = toFront;

            Transform f = this.backFloor;
            this.backFloor  = this.currFloor;
            this.currFloor  = this.frontFloor;
            this.frontFloor = f;
        }
    }

    private void MoveFloor(Transform floor, float posDelta)
    {
        Vector3 pos = floor.transform.localPosition;
        pos.z -= posDelta;
        floor.transform.localPosition = pos;
    }
}
