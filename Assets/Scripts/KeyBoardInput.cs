using Assets;
using UnityEngine;

public class KeyBoardInput : IUserInput
{
    [Header("==== Key settings ====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyCounterBack = "left shift";
    public string keyRun = "q";
    public string keyJump = "f";
    public string keyRoll = "space";
    public string keyAttack = "mouse 0";
    public string keyDefense = "mouse 1";
    public string keyLockon = "mouse 2";
    public string keyAction = "r";

    public string keyJUp = "up";
    public string keyJDown = "down";
    public string keyJLeft = "left";
    public string keyJRight = "right";

    public MyButton buttonUp = new MyButton();
    public MyButton buttonDown = new MyButton();
    public MyButton buttonLeft = new MyButton();
    public MyButton buttonRight = new MyButton();
    public MyButton buttonRun = new MyButton();
    public MyButton buttonJump = new MyButton();
    public MyButton buttonAttack = new MyButton();
    public MyButton buttonDefense = new MyButton();
    public MyButton buttonJUp = new MyButton();
    public MyButton buttonJDown = new MyButton();
    public MyButton buttonJLeft = new MyButton();
    public MyButton buttonJRight = new MyButton();
    public MyButton buttonLockon = new MyButton();
    public MyButton buttonRoll = new MyButton();
    public MyButton buttonCounterBack = new MyButton();
    public MyButton buttonAction = new MyButton();

    [Header("==== Mouse Setting ====")]
    public bool mouseEnable = false;
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;
    public float a;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buttonUp.Tick(Input.GetKey(keyUp));
        buttonDown.Tick(Input.GetKey(keyDown));
        buttonLeft.Tick(Input.GetKey(keyLeft));
        buttonRight.Tick(Input.GetKey(keyRight));
        buttonRun.Tick(Input.GetKey(keyRun));
        buttonJump.Tick(Input.GetKey(keyJump));
        buttonRoll.Tick(Input.GetKey(keyRoll));
        buttonAttack.Tick(Input.GetKey(keyAttack));
        buttonDefense.Tick(Input.GetKey(keyDefense), a);
        buttonJUp.Tick(Input.GetKey(keyJUp));
        buttonJDown.Tick(Input.GetKey(keyJDown));
        buttonJLeft.Tick(Input.GetKey(keyJLeft));
        buttonJRight.Tick(Input.GetKey(keyJRight));
        buttonLockon.Tick(Input.GetKey(keyLockon));
        buttonCounterBack.Tick(Input.GetKey(keyCounterBack));
        buttonAction.Tick(Input.GetKey(keyAction));

        if (mouseEnable)
        {
            jup = Input.GetAxis("Mouse Y") * mouseSensitivityY;
            jright = Input.GetAxis("Mouse X") * mouseSensitivityX;
        }
        else
        {
            jup = (Input.GetKey(keyJUp) ? 1f : 0) - (Input.GetKey(keyJDown) ? 1f : 0);
            jright = (Input.GetKey(keyJRight) ? 1f : 0) - (Input.GetKey(keyJLeft) ? 1f : 0);
        }


        targetDup = (Input.GetKey(keyUp) ? 1f : 0) - (Input.GetKey(keyDown) ? 1f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1f : 0) - (Input.GetKey(keyLeft) ? 1f : 0);

        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        dup = Mathf.SmoothDamp(dup, targetDup, ref velocityDup, 0.1f);
        dright = Mathf.SmoothDamp(dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempAxis = SquareToCircle(new Vector2(dright, dup));
        float dright2 = tempAxis.x;
        float dup2 = tempAxis.y;

        UpdateDmagDvec(dup2, dright2);

        //run = (buttonRun.isPressing && !buttonRun.isDelaying) || buttonRun.isExtending;
        defense = buttonDefense.onPressed ;
        //counterBack = buttonDefense.isExtending && defense && !counterBack;
        jump = buttonJump.onPressed ;
        attack = buttonAttack.onPressed;
        roll = buttonRoll.onPressed ;
        lockon = buttonLockon.onPressed;
        action = buttonAction.onPressed;
    }
}
