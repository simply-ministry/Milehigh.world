// SPDX-License-Identifier: (Boost-1.0 OR MIT OR Apache-2.0)
using UnityEngine;
using USD.NET;
using USD.NET.Unity;

/// <summary>
/// An example script demonstrating how to import and manipulate a USD stage at runtime.
/// This script opens a specified USD file, imports it into the Unity scene,
/// and provides an example of how to modify a prim's transform.
/// </summary>
public class UsdImportExample : MonoBehaviour
{
    /// <summary>
    /// The absolute file path to the USD file to be imported (e.g., "C:/Users/YourUser/Desktop/model.usd").
    /// </summary>
    [Tooltip("The absolute file path to the USD file to be imported.")]
    public string usdFilePath = "/path/to/your/model.usd";

    /// <summary>
    /// The name to give the root GameObject that will contain the imported USD scene.
    /// </summary>
    [Tooltip("The name for the root GameObject of the imported USD scene.")]
    public string usdStageName = "UsdStage";

    /// <summary>
    /// A reference to the active USD stage, which holds the scene data in memory.
    /// </summary>
    private UsdStage _stage;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// It handles the opening of the USD stage, imports it into the scene, and demonstrates a prim modification.
    /// </summary>
    void Start()
    {
        // Import the USD stage from the specified file path.
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
        // Replace "/yourPrimPath" with the actual path to a prim in your USD file.
        UsdPrim prim = _stage.GetPrimAtPath(new UsdPrimPath("/yourPrimPath"));
        if (prim != null)
        {
            //Get the transform
            Xform xform = prim.GetXform();
            // Create a TimeSample
            var timeSamples = new TimeSample<Matrix4x4>();
            xform.GetLocalToWorld(ref timeSamples);
            Matrix4x4 currentMatrix = timeSamples.Value;
            // Modify the matrix (e.g., change the position).
            currentMatrix.SetColumn(3, new Vector4(5, 0, 0, 1)); // Move it 5 units along the x axis.
            //Apply the transform
            timeSamples.Value = currentMatrix;
            xform.SetLocalToWorld(timeSamples);
            _stage.Save();
            Debug.Log("Prim at path '/yourPrimPath' has been moved and the stage was saved.");
        }
        else
        {
            Debug.LogWarning("Prim not found at path: /yourPrimPath. No modification was made.");
        }
    }

    /// <summary>
    /// Called every frame. This method can be used to manipulate the USD scene at runtime.
    /// </summary>
    void Update()
    {
        // You can add code here to manipulate the USD scene at runtime if needed.
        // For example, you could change material properties, visibility, etc.
        // Important: Any changes you want to *persist* in the USD file must be saved.
        // _stage.Save();
    }

    /// <summary>
    /// Called when the MonoBehaviour will be destroyed.
    /// Ensures that the USD stage is properly disposed of to free up resources.
    /// </summary>
    void OnDestroy()
    {
        // Dispose of the USD stage when the script is destroyed. This is crucial for cleanup.
        if (_stage != null)
        {
            _stage.Dispose();
            _stage = null;
        }
    }
}