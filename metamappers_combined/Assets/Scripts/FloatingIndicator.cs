using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIndicator : MonoBehaviour
{
    public float amp;
    public float freq;
    Vector3 init_pos;
    // Start is called before the first frame update
    void Start()
    {
        init_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(init_pos.x, Mathf.Sin(Time.time * freq) * amp + init_pos.y, init_pos.z);
    }
}
