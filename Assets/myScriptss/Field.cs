using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private static float fieldXMin = 0,
                         fieldXMax = 0,
                         fieldYMin = 0,
                         fieldYMax = 0,
                         fieldZMin = 0,
                         fieldZMax = 0,
                         lineBottomMax = 0,
                         lineCenterMax = 0,
                         lineYSize = 0;
    private const float fieldXSize = 2200.0f,
                        fieldYSize = 390.0f,
                        fieldZSize = 10.0f;
    private static int prevLine = 0,
                currentLine = 0;
    public GameObject line, bottomCircle;
    private Animator lineAnim;
    private Transform playerGround;
    void OnDrawGizmos ()
    {
        //Debug
        Gizmos.color = Color.red;
        Gizmos.DrawLine (new Vector2 (fieldXMin, fieldYMin), new Vector2 (fieldXMax, fieldYMin));
        Gizmos.DrawLine (new Vector2 (fieldXMin, fieldYMin), new Vector2 (fieldXMin, fieldYMax));
        Gizmos.DrawLine (new Vector2 (fieldXMin, fieldYMax), new Vector2 (fieldXMax, fieldYMax));
        Gizmos.DrawLine (new Vector2 (fieldXMax, fieldYMin), new Vector2 (fieldXMax, fieldYMax));
        Gizmos.DrawLine (new Vector2 (fieldXMin, fieldYMax - (lineYSize / 2)), new Vector2 (fieldXMax, fieldYMax - (lineYSize / 2)));
        Gizmos.DrawLine (new Vector2 (fieldXMin, fieldYMax - (lineYSize / 2)*3), new Vector2 (fieldXMax, fieldYMax - (lineYSize / 2)*3));
        Gizmos.DrawLine (new Vector2 (fieldXMin, fieldYMax - (lineYSize / 2)*5), new Vector2 (fieldXMax, fieldYMax - (lineYSize / 2)*5));
    }
    void Start ()
    {
        InitField ();

        lineAnim = line.GetComponent<Animator> ();
        playerGround = GameObject.FindObjectOfType<PlayerMove> ().transform.Find ("Ground").transform;
    }
    void Update ()
    {
        playerGround = GameObject.FindObjectOfType<PlayerMove> ().transform.Find ("Ground").transform;
        prevLine = currentLine;
        currentLine = CheckLine (playerGround.position);
        if (currentLine != prevLine)
        {
            line.transform.position = new Vector3 (playerGround.transform.position.x, fieldYMax - ((float)currentLine * lineYSize - (lineYSize / 2.0f)), line.transform.localPosition.z);
            lineAnim.Play ("ShowLine");
        }
        SetBottomCircle ();

    }
    protected void InitField ()
    {
        fieldXMin = this.transform.position.x - fieldXSize / 2.0f;
        fieldXMax = this.transform.position.x + fieldXSize / 2.0f;
        fieldYMin = this.transform.position.y - fieldYSize / 2.0f;
        fieldYMax = this.transform.position.y + fieldYSize / 2.0f;
        fieldZMin = this.transform.position.z - fieldZSize / 2.0f;
        fieldZMax = this.transform.position.z + fieldZSize / 2.0f;
        lineYSize = fieldYSize / 3.0f;

        lineBottomMax = fieldYMin + lineYSize;
        lineCenterMax = fieldYMin + lineYSize * 2.0f;
    }
    public static Vector3 ClampFieldPos (Vector3 unitPos)
    {
        unitPos.x = Mathf.Clamp (unitPos.x, fieldXMin, fieldXMax);
        unitPos.y = Mathf.Clamp (unitPos.y, fieldYMin, fieldYMax);
        unitPos.z = Mathf.Clamp (unitPos.z, fieldZMin, fieldZMax);
        return unitPos;
    }
    public static bool CheckInField (Vector3 unitPos)
    {
        if (unitPos.x >= fieldXMin &&
           unitPos.x <= fieldXMax &&
           unitPos.y >= fieldYMin &&
           unitPos.y <= fieldYMax)
            return true;
        else
            return false;
    }
    public static float GetScaleInField (Vector3 unitPos)
    {
        float rate;

        unitPos.y = Mathf.Clamp (unitPos.y, fieldYMin, fieldYMax);
        rate = unitPos.y / fieldYMax;

        return rate;
    }
    public static int CheckLine (Vector3 unitPos)
    {
        if (unitPos.y < fieldYMax && unitPos.y >= lineCenterMax)
            return 1;
        else if (unitPos.y <= lineCenterMax && unitPos.y >=lineBottomMax)
            return 2;
        else if(unitPos.y < lineBottomMax && unitPos.y >= fieldYMin)
            return 3;
        else return 99;
    }
    public static Vector3 SetLine (int lineNumber, Vector3 currentPos)
    {

        switch (lineNumber)
        {
            case 1:
                {
                    currentPos.y = fieldYMax - (lineYSize / 2);
                    break;
                }
            case 2:
                {
                    currentPos.y = fieldYMax - ((lineYSize / 2) * 3);
                    break;
                }
            case 3:
                {
                    currentPos.y = fieldYMax - ((lineYSize / 2) * 5);
                    break;
                }
            default: { break; }
        }
        return currentPos;
    }
    private void SetBottomCircle ()
    {
        bottomCircle.transform.position = playerGround.position;
    }
    public static Rect GetField ()
    {
        Rect rect = new Rect ();
        rect.xMin = fieldXMin;
        rect.xMax = fieldXMax;
        rect.yMin = fieldYMin;
        rect.yMax = fieldYMax;
        return rect;
    }
}
