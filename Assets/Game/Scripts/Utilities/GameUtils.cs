using UnityEngine;
using System.IO;

public class GameUtils
{
    public static string LoadAppFile(string fileName) 
    {
        string filePath = "";

        #if UNITY_5
        if(Application.isEditor || Application.platform == RuntimePlatform.WP8Player || Application.platform == RuntimePlatform.WSAPlayerARM)
        {
            filePath = Application.streamingAssetsPath + "/" + fileName;
            if (File.Exists(filePath)) 
            {
                StreamReader r = File.OpenText(filePath);

                if (r != null) 
                {
                    string data = r.ReadToEnd();
                    #if NETFX_CORE
                        r.Dispose();
                    #else
                        r.Close();
                    #endif
                    return data;
                }
            }

        }
        #endif

        return "";
    }
}