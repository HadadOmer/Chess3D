using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleLoader
{ 
    
    public static string copsBundleUri= "https://drive.google.com/uc?export=download&id=1dwCCzg-AUYRBuxY8lohNn9Me7NcgLFu7";
    public static string zombiesBundleUri = "https://drive.google.com/uc?export=download&id=1OHUEiVA_mNSoqAPbwfxm5D2NKTVcjeEz";
    public static string pieceIconsBundleUri = "https://drive.google.com/uc?export=download&id=1mtJdI3sO3cCXAX2bItn6C7acKrTL_LdF";

    public List<AssetBundleUri> uris;
    public AssetBundle loadedAssetBundle;

    public AssetBundleLoader()
    {
        uris= new List<AssetBundleUri>
        {
            new AssetBundleUri("cops","https://drive.google.com/uc?export=download&id=1YYV8-6uo9OU0gcKidEBVNixbxzJriStr","https://drive.google.com/uc?export=download&id=1KDvXo5ffqkTtZgZEOPts8K7-deFY2D_S"),
            new AssetBundleUri("zombies", "https://drive.google.com/uc?export=download&id=1flmj4GjxliSDM4NDLl5T4lOGHZLYxDKg","https://drive.google.com/uc?export=download&id=115jRTFGXHjLIbFtYEzjFtFYDBxaOz2QD"),
            new AssetBundleUri("pieceicons","https://drive.google.com/uc?export=download&id=1YUaMWJH5FwFyHCf0MqnTXoQstxxR3C6Y","https://drive.google.com/uc?export=download&id=1yBtaOYGMOIujCY8qTJRNG3hvhgLKvfCe")
        };
    }
    
    public IEnumerator LoadAssetBundle(string name)
    {
        string uri=AssetBundleUri.GetUri(name, uris);        

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri,1,0);
        yield return request.SendWebRequest();
        if(request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            loadedAssetBundle = DownloadHandlerAssetBundle.GetContent(request);
            Debug.Log("Succes "+name);
        }
    }
   /* public IEnumerator LoadAssetBundle(string name)
    {
     
        string uri = AssetBundleUri.GetUri(name, uris);
        WWW request = WWW.LoadFromCacheOrDownload(uri, 0);
        yield return request;
        if(!String.IsNullOrEmpty(request.error))
        {
            Debug.Log(request.error);
        }
        else
            loadedAssetBundle = request.assetBundle;
    }*/
}
public class AssetBundleUri
{
    public string name;
    public string uri;
    public string manifestUri;
    public AssetBundleUri(string name,string uri,string manifestUri)
    {
        this.name = name;
        this.uri = uri;
        this.manifestUri = manifestUri;
    }
    public static string GetUri(string name,List<AssetBundleUri> list)
    {
        foreach (AssetBundleUri u in list)
            if (u.name == name)
                return u.uri;
        return "";
    }
}
