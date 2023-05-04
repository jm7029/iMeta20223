using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour
{
    private pointController[] pointControllers = new pointController[8];
    private List<Transform> PointsTransform = new List<Transform>();
    private List<Vector3> Points = new List<Vector3>();
    private List<Vector3> InitPoints = new List<Vector3>();
    public GameObject[] points = new GameObject[8];
    public int state; // 0: 서있기, 1:걷기, 2:뛰기, 4:앉기
    private Animator animator;
   
    private Vector3 point;
    private int CarNumber;
    private Vector3 boxCenter;
    private Vector3 boxSize;
    private Vector3 boxInitSize;

    public bool grey;
    public BoxCollider col;
    // Start is called before the first frame update
    void Awake()
    {
        //  모터 토크 = 타겟속도 - 내속도 
        foreach (GameObject point in points)
        {
            PointsTransform.Add(point.GetComponent<Transform>());
        }
        if (!grey)
        {
            col = GetComponent<BoxCollider>();
        }
        boxCenter = col.bounds.center; //global좌표다
        boxSize = col.bounds.size;
        boxInitSize = col.size;
        Vector3 pt1 = new Vector3(boxCenter.x - boxSize.x * 0.5f, boxCenter.y - boxSize.y * 0.5f, boxCenter.z + boxSize.z * 0.5f);
        Vector3 pt2 = new Vector3(boxCenter.x + boxSize.x * 0.5f, boxCenter.y - boxSize.y * 0.5f, boxCenter.z + boxSize.z * 0.5f);
        Vector3 pt3 = new Vector3(boxCenter.x + boxSize.x * 0.5f, boxCenter.y - boxSize.y * 0.5f, boxCenter.z - boxSize.z * 0.5f);
        Vector3 pt4 = new Vector3(boxCenter.x - boxSize.x * 0.5f, boxCenter.y - boxSize.y * 0.5f, boxCenter.z - boxSize.z * 0.5f);
        Vector3 pt5 = new Vector3(boxCenter.x - boxSize.x * 0.5f, boxCenter.y + boxSize.y * 0.5f, boxCenter.z + boxSize.z * 0.5f);
        Vector3 pt6 = new Vector3(boxCenter.x + boxSize.x * 0.5f, boxCenter.y + boxSize.y * 0.5f, boxCenter.z + boxSize.z * 0.5f);
        Vector3 pt7 = new Vector3(boxCenter.x + boxSize.x * 0.5f, boxCenter.y + boxSize.y * 0.5f, boxCenter.z - boxSize.z * 0.5f);
        Vector3 pt8 = new Vector3(boxCenter.x - boxSize.x * 0.5f, boxCenter.y + boxSize.y * 0.5f, boxCenter.z - boxSize.z * 0.5f);

        InitPoints.Add(pt1);
        InitPoints.Add(pt2);
        InitPoints.Add(pt3);
        InitPoints.Add(pt4);
        InitPoints.Add(pt5);
        InitPoints.Add(pt6);
        InitPoints.Add(pt7);
        InitPoints.Add(pt8);
        int i = 0;
        foreach (Transform pointTransform in PointsTransform)
        {

            pointTransform.position = InitPoints[i];
            i++;
        }

        CarNumber = GetInstanceID();
        //pointControllers = GetComponentsInChildren<PointController>();
        for (int j = 0; j < 8; j++)
        {
            pointControllers[j] = points[j].GetComponent<pointController>();
        }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();

     }

    private void setClass(int number)
    {
        state = number;
    }
    private void ClearPoints() // 이전 점 좌표를 지우는 함수
    {
        Points.Clear();
    }
    private void ReturnPoints() // 8개 점의 좌표를 리스트에 추가
    {
        try
        {
            foreach (pointController pointController in pointControllers)
            {
                point = pointController.ReturnPosition();
                Points.Add(point);
            }
        }
        catch
        {
            Debug.Log(gameObject.name);
            Debug.Log(transform.position);
        }
    }

    public int GetID()
    {
        return gameObject.GetInstanceID();
    }
    public List<Vector3> GetPoints()
    {
        ClearPoints(); // 이전 정보를 지우고
        ReturnPoints(); // 현재 정보로 최신화
        return Points; // 좌표들을 반환
    }
    public int GetState()
    {
        return state;
    }
    
    public void setState()
    {
        //state = Random.Range(0, 3);
        //switch (state)
        //{
        //    case 0:
        //        animator.SetBool("Walk", false);
        //        animator.SetBool("Run", false);
        //        animator.SetBool("Stand", true);
        //        break;
        //    case 1:
        //        animator.SetBool("Run", false);
        //        animator.SetBool("Stand", false);
        //        animator.SetBool("Walk", true);
        //        break;
        //    case 2:
        //        animator.SetBool("Walk", false);
        //        animator.SetBool("Stand", false);
        //        animator.SetBool("Run", true);
        //        break;
        //}

    }

    public void setPosition()
    {

    }
    
}
