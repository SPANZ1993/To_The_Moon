using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Shake : MonoBehaviour
{

    public Vector3 Amount = new Vector3(1f, 1f, 0);


    public float Duration;
    public float Speed = 10;

    public AnimationCurve Curve;// = AnimationCurve.EaseInOut(0, 1, 1, 0);

    public bool DeltaMovement = true;

    protected Camera Camera;
    protected float time = 0;
    protected Vector3 lastPos;
    protected Vector3 nextPos;
    protected float lastFoV;
    protected float nextFoV;
    protected bool destroyAfterPlay;

    public bool currentlyShaking {get; private set;}

    private void Awake()
    {
        Camera = GetComponent<Camera>();
    }

    public void Update(){
        if (time > 0 && !currentlyShaking){
            currentlyShaking = true;
        }
        else if (time <= 0 && currentlyShaking){
            currentlyShaking = false;
        }
    }

    public void shake(float duration)
    {
        ResetCam();
        time = duration;
        Duration = duration;
    }

    private void LateUpdate()
    {
        if (time > 0)
        {
            //do something
            time -= Time.deltaTime;
            if (time > 0)
            {
                //next position based on perlin noise
                nextPos = (Mathf.PerlinNoise(time * Speed, time * Speed * 2) - 0.5f) * Amount.x * transform.right * Curve.Evaluate(1f - time / Duration) +
                            (Mathf.PerlinNoise(time * Speed * 2, time * Speed) - 0.5f) * Amount.y * transform.up * Curve.Evaluate(1f - time / Duration);
                nextFoV = (Mathf.PerlinNoise(time * Speed * 2, time * Speed * 2) - 0.5f) * Amount.z * Curve.Evaluate(1f - time / Duration);

                Camera.fieldOfView += (nextFoV - lastFoV);
                Camera.transform.Translate(DeltaMovement ? (nextPos - lastPos) : nextPos);

                lastPos = nextPos;
                lastFoV = nextFoV;
            }
            else
            {
                //last frame
                ResetCam();
                if (destroyAfterPlay)
                    Destroy(this);
            }
        }
    }

    private void ResetCam()
    {
        //reset the last delta
        Camera.transform.Translate(DeltaMovement ? -lastPos : Vector3.zero);
        Camera.fieldOfView -= lastFoV;

        //clear values
        lastPos = nextPos = Vector3.zero;
        lastFoV = nextFoV = 0f;
    }
}

