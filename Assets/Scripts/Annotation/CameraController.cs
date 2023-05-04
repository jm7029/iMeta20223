using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CameraController : MonoBehaviour
{
    private GameObject[] Persons;
    private Camera cam;
    private List<PersonController> PersonControllers = new List<PersonController>();
    private PersonController PersonController;
    private int Number;
    private List<int> PositionX = new List<int>();
    private List<int> PositionY = new List<int>();
    private List<int> Result = new List<int>();
    private List<List<int>> Results = new List<List<int>>();
    private List<int> states = new List<int>();

    public RenderTexture texture;
    private List<Vector3> Points_ = new List<Vector3>();
    private Vector3 centerpoint = new Vector3(0, 0, 0);
    private Dictionary<int, List<Vector3>> Information = new Dictionary<int, List<Vector3>>();
    public int CamNumber;
    // Start is called before the first frame update
    void Start()
    {
        //CamNumber = GetInstanceID();
        Persons = GameObject.FindGameObjectsWithTag("Person");
        cam = GetComponent<Camera>();

        foreach (GameObject person in Persons)
        {
            PersonController = person.GetComponent<PersonController>();
            PersonControllers.Add(PersonController);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ClearPointsAndNumber()
    {
        Results.Clear();
        Information.Clear();
    }
    private void GetPointsAndNumber()
    {
        foreach (PersonController PersonController in PersonControllers)
        {
            Points_ = PersonController.GetPoints();
            Number = PersonController.GetID();
            states.Add(PersonController.GetState());
            Information.Add(Number, Points_);

        }
    }

    private Vector3 CenterPoint(List<Vector3> Points)  // 8개의 포인트 정보를 이용해서 중심점을 구하는 함수
    {
        centerpoint = new Vector3(0, 0, 0);
        foreach (Vector3 point in Points)
        {
            centerpoint += point;
        }
        centerpoint = centerpoint / 8;
        return centerpoint;
    }
    private bool CheckCamera(Vector3 targetObject) // 중심점을 입력해서 카메라 안에 있는지 없는지 확인하는 함수
    {

        Vector3 screenPoint = cam.WorldToViewportPoint(targetObject);
        bool check = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        
        return check;
    }

    private void CheckInformation()
    {
        //foreach (KeyValuePair<int, List<Vector3>> inform in Information)
        //{
        //    Vector3 cenrterPoint = CenterPoint(inform.Value);
        //    bool check = CheckCamera(cenrterPoint);
        //    if (!check)
        //    {
        //        Information.Remove(inform.Key);
        //    }
        //    index++;
        //}
        int index = 0;
        foreach (var key in Information.Keys.ToList())
        {
            Vector3 cenrterPoint = CenterPoint(Information[key]);
            bool check = CheckCamera(cenrterPoint);
            if (!check)
            {
                Information.Remove(key);
                states.RemoveAt(index);
            }
            else
            {
                index++;
            }

        }
    }

    private void GetBoundingBox(int IDNumber, int state,List<Vector3> Points)
    {
        Result.Clear();
        foreach (Vector3 point in Points)
        {
            Vector3 position = cam.WorldToScreenPoint(point);
            PositionX.Add(Mathf.RoundToInt(position.x));
            PositionY.Add(Mathf.RoundToInt(position.y));
        }

        float MinX = PositionX.Min();
        float MaxX = PositionX.Max();
        float MinY = PositionY.Min();
        float MaxY = PositionY.Max();


        int x = Mathf.RoundToInt((MaxX + MinX) * 0.5f);
        int y = Mathf.RoundToInt(500 - (MaxY + MinY) * 0.5f);
        int width = Mathf.RoundToInt(MaxX - MinX);
        int height = Mathf.RoundToInt(MaxY - MinY);


        //Debug.Log("x:" + x);
        //Debug.Log("y:" + y);
        //Debug.Log("w:" + width);
        //Debug.Log("h:" + height);

        List<int> result = new List<int>();
        result.Add(CamNumber);
        result.Add(IDNumber);
        result.Add(x);
        result.Add(y);
        result.Add(width);
        result.Add(height);
        result.Add(state);
        Results.Add(result);
        PositionX.Clear();
        PositionY.Clear();
    }







    public List<List<int>> ReturnResults()
    {
        ClearPointsAndNumber();
        GetPointsAndNumber();
        CheckInformation();
        //foreach(KeyValuePair<int,List<Vector3>> inform in Information)
        //{
        //    GetBoundingBox(inform.Key, inform.Value);
        //    Debug.Log("key" + inform.Key);
        //    Debug.Log("Value" + inform.Value[0]);

        //}
        int index = 0;
        foreach (var key in Information.Keys.ToList())
        {
            GetBoundingBox(key, states[index], Information[key]);
            index++;
        }

        return Results; // Results의 형태는 [CamID,state,CenterX, CenterY, Width, Height]의 리스트
    }
    public int ReturnCamNumber()
    {
        return CamNumber;
    }

    // Start is called before the first frame update


    // Update is called once per frame

}
