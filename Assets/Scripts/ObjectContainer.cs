using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour, IEnumerable<ObjectContainerChild>
{
    private List<ObjectContainerChild> objectList = new List<ObjectContainerChild>();

    public ObjectContainerChild this[int index] => objectList[index];
    public int Count => objectList.Count;

    private void OnTransformChildrenChanged()
    {
        objectList.Clear();
        foreach (Transform child in transform)
        {
            objectList.Add(child.GetComponent<ObjectContainerChild>());
        }
    }

    public IEnumerator<ObjectContainerChild> GetEnumerator()
    {
        return objectList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void OnClick()
    {
        var enumerator = GetEnumerator();
        while (enumerator.MoveNext())
        {
            Debug.Log("position:" + enumerator.Current.transform.position);

        }
    }
}