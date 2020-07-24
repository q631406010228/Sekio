using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public float horizontalSpeed = 100;
    public float verticalSpeed = 70;
    public float camerDampValue = 0.05f;
    public Image lockDot;
    public Image executeDot;
    public bool lockState;
    public bool isAI = false;
    public GameObject player;
    public GameObject overFrame;

    private IUserInput pi;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject camera;
    private LockTarget lockTarget;
    private Vector3 camerDampVelocity;

    // Start is called before the first frame update
    void Awake()
    {
    }

    void Start()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        ActorController ac = playerHandle.GetComponent<ActorController>();
        model = ac.model;
        pi = ac.pi;

        if (isAI)
        {
            lockTarget = new LockTarget(player);
        }
        else
        {
            camera = Camera.main.gameObject;
            lockDot.enabled = false;
        }

        lockState = false;
        executeDot.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAI)
            return;
        if (lockTarget != null)
        {
            if(isAI == false)
            {
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(
                    lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
                executeDot.rectTransform.position = lockDot.rectTransform.position;
            }
            if(Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10)
            {
                LockUnlock();
            }
            else if (lockTarget.am != null && lockTarget.am.sm.isDie)
            {
                LockUnlock();
            }
        }
    }

    void FixedUpdate()
    {
        if(lockTarget == null)
        {
            Vector3 tempModeEuler = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up, pi.jright * horizontalSpeed * Time.deltaTime);
            //cameraHandle.transform.Rotate(Vector3.right, -pi.jup * verticalSpeed * Time.deltaTime);
            //tempEulerX = cameraHandle.transform.eulerAngles.x;
            tempEulerX -= pi.jup * verticalSpeed * Time.deltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);

            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0);
            model.transform.eulerAngles = tempModeEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }

        if(isAI == false)
        {
            //Vector3 x = new Vector3(transform.position.x, 2.4f, transform.position.z);
            //camera.transform.eulerAngles = transform.eulerAngles;
            //camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, camerDampValue);
            camera.transform.position = Vector3.SmoothDamp(
                camera.transform.position, transform.position, ref camerDampVelocity, camerDampValue);
            camera.transform.LookAt(cameraHandle.transform);
        }
    }

    public void LockChange()
    {
        Vector3 modelOrighin1 = model.transform.position;
        Vector3 modelOrighin2 = modelOrighin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrighin2 + camera.transform.forward * 5;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(1f, 5f, 5f),
            model.transform.rotation, LayerMask.GetMask(isAI ? "Player" : "Enemy"));
        if(cols.Length == 0)
        {
            LockProcessA(null, false, false, isAI);
        }
        else
        {
            foreach (var col in cols)
            {
                if(lockTarget != null && lockTarget.obj == col.gameObject)
                {
                    LockProcessA(null, false, false, isAI);
                    break;
                }
                lockTarget = new LockTarget(col.gameObject, col.bounds.extents.y);
                lockDot.enabled = true;
                lockState = true;
                break;
            }
        }
    }

    private void LockProcessA(LockTarget lockTarget, bool lockDotEnabled, bool lockState, bool isAI)
    {
        this.lockTarget = lockTarget;
        this.lockState = lockState;
        if(isAI == false)
            lockDot.enabled = lockDotEnabled;
    }

    public void LockUnlock()
    {
        LockProcessA(null, false, false, isAI);
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;
        public ActorManager am;

        public LockTarget(GameObject obj, float halfHeight)
        {
            this.obj = obj;
            this.halfHeight = halfHeight;
            am = obj.GetComponent<ActorManager>();
        }

        public LockTarget(GameObject obj)
        {
            this.obj = obj;
        }
    }
}
