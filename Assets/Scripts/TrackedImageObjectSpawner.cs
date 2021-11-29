using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Events;

public class TrackedImageObjectSpawner : MonoBehaviour
{

    [SerializeField] ARTrackedImageManager _trackedImageManager;
    
    // Uncomment to use manual spawning 
   /* [SerializeField] GameObject _prefabToSpawn;

    private GameObject _spawnedPrefab;
    */

    [SerializeField] private UnityEvent OnImageFound;

    // Start is called before the first frame update
    void Awake()
    {
        if(!_trackedImageManager)
            _trackedImageManager = GetComponent<ARTrackedImageManager>();
        if(!_trackedImageManager)
        {
            Debug.LogError("Need a ARTrackedImageManager component");
            this.enabled = false;
        }
    
    }

    private void OnEnable() {
        
        _trackedImageManager.trackedImagesChanged += ImageTracked;
    }
    private void OnDisable() {
        _trackedImageManager.trackedImagesChanged -= ImageTracked;
        
    }

    private void ImageTracked(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Uncomment to use manual spawning
      /*  foreach(ARTrackedImage image in eventArgs.added)
        {
            _spawnedPrefab = Instantiate(_prefabToSpawn,image.transform.position, Quaternion.identity);
            _spawnedPrefab.transform.LookAt(Camera.main.transform,transform.up);
            OnImageFound.Invoke();

        }
        foreach(ARTrackedImage image in eventArgs.updated)
        {
            _spawnedPrefab.transform.position = image.transform.position;
        }*/
    }
}
