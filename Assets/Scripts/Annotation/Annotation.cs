using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Annotation : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] Cameras;
    private List<CameraController> cameraControllers = new List<CameraController>();
    private int[] Result = new int[6];
    public Canvas canvas;

    public static bool ThreeD = true;

    public static float ScreenHeight =500;
    public static float ScreenWidth = 500;
    private RenderTexture texture;
    private string path;
    private List<List<int>> Results = new List<List<int>>();
    private string ImagePath;
    private string AnnotationPath;
    private string AnnotationPath2;

    private int frameNumber = 1;
    private string frameID;
    private float[] pt1 = new float[2];
    private float[] pt2 = new float[2];
    int CameraID;
    int state;
    int Class = 0;
    float x;
    float y;
    float width;
    float height;
    float Timer = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        ScreenCapture.CaptureScreenshot("test.png");
        Application.targetFrameRate = 30;
        ImagePath = @"C:\Users\zz525\iMeta2023\images\";
        AnnotationPath = @"C:\Users\zz525\iMeta2023\labels\";
        AnnotationPath2 = @"C:\Users\zz525\iMeta2023\labels2\";
        Cameras = GameObject.FindGameObjectsWithTag("Cam");
        foreach (GameObject cam in Cameras)
        {
            cameraControllers.Add(cam.GetComponent<CameraController>());
        }
        InvokeRepeating("Annot", 1f, 0.1f);
    }
    // Update is called once per frame
    void LateUpdate()
    {


     }

    void Annot()
    {
        frameID = frameNumber.ToString().PadLeft(8, '0');
        CameraID = 0;

        foreach (CameraController cam in cameraControllers)
        {
            Results = cam.ReturnResults();
            texture = cam.texture;
            string path = @"C:\Users\zz525\iMeta2023\images\" + CameraID.ToString().PadLeft(8, '0') + frameID + ".jpg";
            string Annots2 = "";

            foreach (List<int> Result in Results)
            {
                string Annots = "";
                try
                {
                    x = Result[2]; // 중심 x좌표
                    y = Result[3]; // 중심 y좌표
                    width = Result[4]; // 수평 길이
                    height = Result[5]; // 수직 길이
                    state = Result[6];
                    pt1[0] = x - width * 0.5f;
                    if (pt1[0] < 0)
                    {
                        pt1[0] = 0;
                    }
                    pt1[1] = y - height * 0.5f;
                    if (pt1[1] < 0)
                    {
                        pt1[1] = 0;
                    }
                    pt2[0] = x + width * 0.5f;
                    if (pt2[0] > ScreenWidth)
                    {
                        pt2[0] = ScreenWidth;
                    }
                    pt2[1] = y + height * 0.5f;
                    if (pt2[1] > ScreenHeight)
                    {
                        pt2[1] = ScreenHeight;
                    }
                    x = (pt1[0] + pt2[0]) / 32 * 0.5f;
                    y = (pt1[1] + pt2[1]) / 512 * 0.5f;
                    width = (pt2[0] - pt1[0]) / 32;
                    height = (pt2[1] - pt1[1]) / 512;
                    string Annot = Class.ToString() + " " + x.ToString() + " " + y.ToString() + " " + width.ToString() + " " + height.ToString() + "\n";
                    string Annot2 = state.ToString() + " " + x.ToString() + " " + y.ToString() + " " + width.ToString() + " " + height.ToString() + "\n";
                    Annots = Annots + Annot;
                    Annots2 = Annots2 + Annot2;

                }
                catch
                {

                }
            }
            //DirectoryInfo imgDi = new DirectoryInfo(ImagePath + CameraID.ToString().PadLeft(8, '0')); // 이미지 폴더 저장 경로
            //DirectoryInfo anoDi = new DirectoryInfo(AnnotationPath + CameraID.ToString().PadLeft(8, '0')); //어노테이션 폴더 저장 경로
            DirectoryInfo imgDi = new DirectoryInfo(ImagePath); // 이미지 폴더 저장 경로
            DirectoryInfo anoDi = new DirectoryInfo(AnnotationPath); //Human Detection 폴더 저장 경로
            DirectoryInfo anoDi2 = new DirectoryInfo(AnnotationPath2); //Action Detection 폴더 저장 경로
            if (imgDi.Exists == false)
            {
                imgDi.Create();
            }
            if (anoDi.Exists == false)
            {
                anoDi.Create();
            }
            if (anoDi2.Exists == false)
            {
                anoDi2.Create();
            }
            ImageSave(texture, path);
            //File.WriteAllText(@"C:\Users\zz525\Annotation\" + CameraID.ToString().PadLeft(8, '0') + @"\" + frameID + ".txt", Annots);
            //File.WriteAllText(@"C:\Users\zz525\MADPC\labels\" + CameraID.ToString().PadLeft(8, '0') + frameID + ".txt", Annots);
            File.WriteAllText(@"C:\Users\zz525\iMeta2023\labels2\" + CameraID.ToString().PadLeft(8, '0') + frameID + ".txt", Annots2);
            Resources.UnloadUnusedAssets();
            CameraID++;
            
        }
        frameNumber++;

    }

        void ImageSave(RenderTexture texture, string path)
        {
            RenderTexture.active = texture;
            var texture2D = new Texture2D(texture.width, texture.height);
            texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture2D.Apply();
            var data = texture2D.EncodeToJPG();
            File.WriteAllBytes(path, data);
        }
    }

