using UnityEngine;
using USD.NET;
using USD.NET.Unity;

public class UsdImportExample : MonoBehaviour
{
    public string usdFilePath = "/path/to/your/model.usd"; // Replace with your USD file path
    public string usdStageName = "UsdStage";
    private UsdStage _stage;

    void Start()
    {
        // Import the USD stage.
        _stage = UsdStage.Open(usdFilePath);

        if (_stage == null)
        {
            Debug.LogError("Failed to open USD stage: " + usdFilePath);
            return;
        }

        // Create a game object to serve as the parent for the USD scene.
        GameObject usdRoot = new GameObject(usdStageName);

        // Import the USD stage into the Unity scene.
        UsdPrim rootPrim = _stage.GetRootPrim();
        SceneImportOptions importOptions = new SceneImportOptions();
        GameObject rootObject = UsdGameObject.Create(usdRoot, _stage, rootPrim, importOptions);

       if (rootObject == null)
        {
             Debug.LogError("Failed to import USD scene.");
             return;
        }

        // Example: Change the position of a prim (if it exists).
        UsdPrim prim = _stage.GetPrimAtPath(new UsdPrimPath("/yourPrimPath")); // Replace with the path to the prim you want to modify.
        if (prim != null)
        {
            //Get the transform
            Xform xform = prim.GetXform();

            // Create aSample
            var timeSamples = new TimeSample<Matrix4x4>();
            xform.GetLocalToWorld(ref timeSamples);

            Matrix4x4 currentMatrix = timeSamples.Value;
            // Modify the matrix (e.g., change the position).
            currentMatrix.SetColumn(3, new Vector4(5, 0, 0, 1)); // Move it 5 units along the x axis.

            //Apply the transform
             timeSamples.Value = currentMatrix;
             xform.SetLocalToWorld(timeSamples);
            _stage.Save();

        }
        else
        {
            Debug.LogWarning("Prim not found at path: /yourPrimPath");
        }
    }

    void Update()
    {
        // You can add code here to manipulate the USD scene at runtime if needed.
        // For example, you could change material properties, visibility, etc.
        // Important:  Any changes you want to *persist* in the USD file, you must save.
        // _stage.Save();
    }

    void OnDestroy()
    {
        // Dispose of the USD stage when the script is destroyed.  Important for cleanup.
        if (_stage != null)
        {
            _stage.Dispose();
            _stage = null;
        }
    }
}