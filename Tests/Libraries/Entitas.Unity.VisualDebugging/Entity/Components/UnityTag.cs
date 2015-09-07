using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entitas;
using UnityEngine;

public class UnityTag : MonoBehaviour {

    /// <summary>
    /// A unique identifier for a Unity GameObject.
    /// </summary>
    public string value;

    /// <summary>
    /// Finds all GameObjects tagged.
    /// </summary>
    public static List<UnityTag> FindAll(string tag) {
        var tagged = Resources.FindObjectsOfTypeAll<UnityTag>();
        List<UnityTag> tags = new List<UnityTag>();
        foreach(UnityTag t in tagged)
        {
            if(tag.Equals(t.value))
                tags.Add(t);
        }

        return tags;
    }

    /// <summary>
    /// Finds the first GameObjects tagged.
    /// </summary>
    public static UnityTag FindFirst(string tag){
        var tagged = Resources.FindObjectsOfTypeAll<UnityTag>();
        for (int i = 0; i < tagged.Count(); i++) {
            if (tag.Equals(tagged[i].value))
                return tagged[i];
        }

        Debug.LogError("Could not find tag: "+tag);
        return null;
    }
    
}
