using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;

public class GlobalHelper
{
    public static bool OrderedDictionaryContainsKey(OrderedDictionary orderedDictionary, object key) {
        foreach (object givenKey in orderedDictionary) {
            Debug.Log(givenKey);
            if (givenKey == key) {
                return true;
            }
        }
        return false;
    }
}
