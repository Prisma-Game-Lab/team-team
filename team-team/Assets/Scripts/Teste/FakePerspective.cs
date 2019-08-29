using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePerspective : MonoBehaviour
{
    //o valor inicial para a escala em Y, que será manipulada para criar a impressão de profundidade
    private Vector3 baseScale;


    public Vector3 screenTop;
    public Vector3 screenBottom;

    public float scaleUpperMultiplier = 2.0f;
    public float scaleLowerMultiplier = 0.5f;


    //public float upperScaleLimit = 2.0f;
    //public float lowerScaleLimit = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, screenBottom.y, screenTop.y), transform.position.z);
        
        //calcula a diferença, no eixo y, da posição atual do objeto para o centro do mundo  
        float deltaY = (screenTop.y - transform.position.y);
        float maxDelta = (screenTop.y - screenBottom.y);
        
        Debug.Assert(maxDelta > 0.0f);
        
        float norm = deltaY / maxDelta;

        transform.localScale = baseScale * Mathf.Lerp(scaleLowerMultiplier, scaleUpperMultiplier, norm);

    }
}
