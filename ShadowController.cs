using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour {

    public bool FixedShadow = false;
    public float ShadowStartingAngle = 45f;
    public float ShadowEndingAngle = -45f;
    public float MaxShadowLength = 1.5f;
    public float MinShadowLength = 1f;


    [SerializeField] private Transform model;

    public SpriteRenderer Sprite;

    private void Awake() {
        Sprite = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        
        if (model.localScale.x == -1 ) {
            FlipShadow();
        }
        else {
            ResetShadow();
        }

        Vector3 currentRotationEuler = transform.rotation.eulerAngles;
        Debug.Log(currentRotationEuler.z);
    }

    private void FlipShadow() {

        Sprite.flipX = true;

    }

    private void ResetShadow() {

        Sprite.flipX = false;

    }
}