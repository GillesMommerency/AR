using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Http;
using System.Threading.Tasks;

public class sliderTest : MonoBehaviour
{
    // Start is called before the first frame update
    public void onValueChange(float value)
    {
        Debug.Log(value);
    }
}
