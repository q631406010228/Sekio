using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{

    [Header("==== Output signals ====")]
    public float dup;
    public float dright;
    public float dmag;
    public Vector3 dvec;
    public float jup;
    public float jright;

    //1. pressing signal
    public bool run;
    public bool defense;
    public bool action;
    //2. trigger once signal
    public bool jump;
    public bool attack;
    public bool roll;
    public bool lockon;
    public bool counterBack;
    //3. double signal

    [Header("==== Others ====")]
    public bool inputEnabled = true;

    protected float targetDup;
    protected float targetDright;
    public float targetDmag;
    protected float velocityDup;
    protected float velocityDright;
    public float velocityDmag;

    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - input.y * input.y / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - input.x * input.x / 2.0f);
        return output;
    }

    protected void UpdateDmagDvec(float dup2, float dright2)
    {
        dmag = Mathf.Sqrt(dup2 * dup2 + dright2 * dright2);
        dvec = dright2 * transform.right + dup2 * transform.forward;
    }

}
