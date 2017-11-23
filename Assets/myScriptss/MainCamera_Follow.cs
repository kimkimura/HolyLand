using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera_Follow : MonoBehaviour
{
    public Animation startText,clearText;
    private Transform playerTransform;
    private Vector3 velocity = Vector3.zero;
    private float vel = 0;
    [SerializeField]
    private float smoothTime = 0.8f;
    public float nearly = 400.0f;
    //Debug
    public Slider zoom;
    Vector2 bgLT, bgRB;
    Vector2 viewPortRectMin, viewPortRectMax;
    // Use this for initialization
    void Start()
    {
        //playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
        PlayerMove[] objs = GameObject.FindObjectsOfType<PlayerMove> ();
        foreach (PlayerMove obj in objs)
        {
            if (obj.gameObject.activeSelf)
                playerTransform = obj.transform;
        }
//        playerTransform = GameObject.FindObjectOfType<PlayerMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!BattleSystem.GetIsPose())
        {
            PlayerMove[] objs = GameObject.FindObjectsOfType<PlayerMove> ();
            foreach (PlayerMove obj in objs)
            {
                if (obj.gameObject.activeSelf)
                    playerTransform = obj.transform;
            }

            SpriteRenderer bg = GameObject.FindGameObjectWithTag("StageBG").GetComponent<SpriteRenderer>();
            bgLT = bg.bounds.min;
            bgRB = bg.bounds.max;

            GameObject target = CheckNearEnemyDistance();

            //Camera.main.orthographicSize = zoom.value;
            float size;
            if(target == null)
                size = 200f;
            else
                size = GameSettings.GetCameraSizeMax() * (Vector2.Distance(target.transform.Find("Ground").transform.position, playerTransform.Find("Ground").transform.position) / nearly);
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, Mathf.Clamp(size, GameSettings.GetCameraSizeMin(), GameSettings.GetCameraSizeMax()), ref vel, smoothTime * 2);
            viewPortRectMin = Camera.main.ViewportToWorldPoint(Vector2.zero);
            viewPortRectMax = Camera.main.ViewportToWorldPoint(Vector2.one);
            
            Vector3 moveMin,
                    moveMax;

            moveMin.x = bgLT.x + (Mathf.Abs(viewPortRectMax.x - viewPortRectMin.x) / 2.0f);
            moveMin.y = bgLT.y + (Mathf.Abs(viewPortRectMax.y - viewPortRectMin.y) / 2.0f);
            moveMax.x = bgRB.x - (Mathf.Abs(viewPortRectMax.x - viewPortRectMin.x) / 2.0f);
            moveMax.y = bgRB.y - (Mathf.Abs(viewPortRectMax.y - viewPortRectMin.y) / 2.0f);
            moveMin.z = Camera.main.transform.position.z;
            moveMax.z = Camera.main.transform.position.z;

            if (bgLT.x > viewPortRectMin.x)
                Camera.main.transform.position = new Vector3(moveMin.x + Mathf.Abs(bgLT.x - viewPortRectMin.x), Camera.main.transform.position.y, Camera.main.transform.position.z);
            if (bgRB.x < viewPortRectMax.x)
                Camera.main.transform.position = new Vector3(moveMax.x - Mathf.Abs(bgRB.x - viewPortRectMax.x), Camera.main.transform.position.y, Camera.main.transform.position.z);
            if (bgLT.y > viewPortRectMin.y)
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, moveMin.y + Mathf.Abs(bgLT.y - viewPortRectMin.y), Camera.main.transform.position.z);
            if (bgRB.y < viewPortRectMax.y)
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, moveMax.y - Mathf.Abs(bgRB.y - viewPortRectMax.y), Camera.main.transform.position.z);

            Vector3 movePos = Camera.main.transform.position;
            if (target == null)
            {
                movePos.x = Mathf.Clamp(playerTransform.position.x, moveMin.x, moveMax.x);
                movePos.y = Mathf.Clamp(playerTransform.position.y, moveMin.y, moveMax.y);
            }
            else
            {
                Vector2 pos = Vector2.Lerp(playerTransform.position, target.transform.position, 0.5f);
                movePos.x = Mathf.Clamp(pos.x, moveMin.x, moveMax.x); ;
                movePos.y = Mathf.Clamp(pos.y, moveMin.y, moveMax.y);
            }
            //Camera.main.transform.position = movePos;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, movePos, ref velocity, smoothTime);




        }

    }
    GameObject CheckNearEnemyDistance()
    {
        GameObject nearEnemy = null;
        float distance = 0;
        float nearDistance = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Debug.DrawLine(obj.transform.Find("Ground").transform.position, playerTransform.Find("Ground").transform.position);
            distance = Vector2.Distance(obj.transform.Find("Ground").transform.position, playerTransform.Find("Ground").transform.position);
            if (nearDistance == 0 || nearDistance > distance)
            {
                nearDistance = distance;
                if (nearDistance < nearly)
                    nearEnemy = obj;
            }
        }
        return nearEnemy;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(bgLT.x, bgLT.y), new Vector2(bgRB.x, bgLT.y));
        Gizmos.DrawLine(new Vector2(bgLT.x, bgRB.y), new Vector2(bgRB.x, bgRB.y));
        Gizmos.DrawLine(new Vector2(bgLT.x, bgLT.y), new Vector2(bgLT.x, bgRB.y));
        Gizmos.DrawLine(new Vector2(bgRB.x, bgLT.y), new Vector2(bgRB.x, bgRB.y));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(viewPortRectMin.x,viewPortRectMin.y), new Vector2(viewPortRectMax.x,viewPortRectMin.y));
        Gizmos.DrawLine(new Vector2(viewPortRectMin.x,viewPortRectMax.y), new Vector2(viewPortRectMax.x,viewPortRectMax.y));
        Gizmos.DrawLine(new Vector2(viewPortRectMin.x,viewPortRectMin.y), new Vector2(viewPortRectMin.x,viewPortRectMax.y));
        Gizmos.DrawLine(new Vector2(viewPortRectMax.x,viewPortRectMin.y), new Vector2(viewPortRectMax.x,viewPortRectMax.y));
    }
    public void PlayStartText()
    {
        startText.Play();
        BattleSystem.SetResume();
    }
    public void PlayClearText()
    {
        clearText.Play();
    }
}
