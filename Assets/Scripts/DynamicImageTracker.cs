using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class DynamicImageTracker : MonoBehaviour
{

    public GameObject world1;
    public GameObject world2;
    [SerializeField]
    private GameObject textObject;
    [SerializeField]
    private GameObject textScannedObject;

    ARTrackedImageManager m_image_tracker;
    Dictionary<int, GameObject> m_spawned_worlds = new Dictionary<int, GameObject>();

    private void Awake()
    {
        m_image_tracker = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        m_image_tracker.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnDisable()
    {
        m_image_tracker.trackedImagesChanged -= OnTrackedImageChanged;
    }
  
    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs args_)
    {
        textObject.SetActive(false);
        StartCoroutine(ShowAndHide(textScannedObject, 2.0f)); // 1 second
        foreach (ARTrackedImage i in args_.added)
        {
            if (i.referenceImage.name == "one")
            {
                m_spawned_worlds.Add(i.GetInstanceID(), Instantiate(world1, i.transform.position, i.transform.rotation));
            }
            else if (i.referenceImage.name == "two")
            {
                m_spawned_worlds.Add(i.GetInstanceID(), Instantiate(world2, i.transform.position, i.transform.rotation));
            }
        }

        foreach (ARTrackedImage i in args_.updated)
        {
            int id = i.GetInstanceID();
            if (m_spawned_worlds.ContainsKey(id))
            {
                m_spawned_worlds[id].transform.position = i.transform.position;
                m_spawned_worlds[id].transform.rotation = i.transform.rotation;
            }
        }

        foreach (ARTrackedImage i in args_.removed)
        {
            int id = i.GetInstanceID();
            if (m_spawned_worlds.ContainsKey(id))
            {
                Object.Destroy(m_spawned_worlds[id]);
                m_spawned_worlds.Remove(id);
            }
        }

    }
    IEnumerator ShowAndHide(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        go.SetActive(false);
    }

}