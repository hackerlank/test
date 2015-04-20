using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AESpriteScriptable : ScriptableObject 
{
    public List<AEFootage> spAEFootage = new List<AEFootage>();


    public static List<AEFootage> CloneData(AESpriteScriptable data)
    {
        List<AEFootage> newData = new List<AEFootage>();
        for (int i = 0; i < data.spAEFootage.Count; i++)
        {
            newData.Add(AEFootage.CloneData(data.spAEFootage[i]));
        }

        return newData;
    }
}
