using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class SceneLoadItem : Editor
{
    [MenuItem("Open SceneSetup/Prototype02")]
    static void Prototype02()
    {
        OpenScene("Prototype02");
    }

    [MenuItem("Open SceneSetup/Prototype01")]
    static void Prototype01()
    {
        OpenScene("Prototype01");
    }

    [MenuItem("Open SceneSetup/MainMenu")]
    static void MainMenu()
    {
        OpenScene("MainMenuInstance");
    }

    static void OpenScene(string name)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            GameInstance gameInstance = Resources.Load("Datas/GameInstances/" + name) as GameInstance;

            EditorSceneManager.OpenScene("Assets/Scenes/DevScenes/MasterScene.unity");

            DirectoryInfo path = new DirectoryInfo(Application.dataPath);

            FileInfo[] fileInfo = path.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (FileInfo file in fileInfo)
            {
                for (int i = 0; i < gameInstance.sceneList.Count; i++)
                {
                    if (file.Name == gameInstance.sceneList[i]+".unity")
                    {
                        var directoryPath = Application.dataPath;
                        //Debug.LogError("dir " + directoryPath);
                        string filePath = file.FullName;
                        filePath = filePath.Replace(@"\","/");
                        var relativeFilePath = filePath.Replace(directoryPath, string.Empty);
                        //Debug.LogError(filePath + "   " + relativeFilePath);

                        EditorSceneManager.OpenScene("Assets" + relativeFilePath, OpenSceneMode.Additive);
                    }
                }
            }
        }
    }
}
