using UnityEngine;
using System.IO;
using System;

public class GameUtils
{
    public static string LoadAppFile(string fileName) 
    {
        string filePath="";

        #if UNITY_WEBPLAYER
        //if playing on webplayer use loaded file cache
        if(PPLSDKConfig.inst.DebugAssetServer || !Application.isEditor)
        {
            if(fileCache.ContainsKey(fileName))
            {
                return fileCache[fileName].ToString();
            }
            else
            {
                if(fileCache.Count == 0)
                {
                    Debug.LogError("File cache is empty. PreloadAssetFiles() may have not been called yet.");
                }
                
                return "";
            }
        }
        #endif

        if (Application.platform== RuntimePlatform.IPhonePlayer) 
        {
            filePath = Application.dataPath + "/Raw/" + fileName;
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
        else  if(Application.platform == RuntimePlatform.Android)
        {
            filePath = "jar:file://" + Application.dataPath + "!/assets/" + fileName;
            WWW loadFile = new WWW(filePath);

            while(!loadFile.isDone) 
            {

            }

            if( !string.IsNullOrEmpty(loadFile.text) )
            {
                var resultData = loadFile.text;
                GC.Collect();
                return resultData;
            }
        }
        #if UNITY_5
        else  if(Application.isEditor || Application.platform == RuntimePlatform.WP8Player || Application.platform == RuntimePlatform.WSAPlayerARM)
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