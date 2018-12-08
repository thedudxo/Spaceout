using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class CameraCollision : MonoBehaviour {

    public void Collide(Vector3 collideTarget, float reactionSpeed, AnimationCurve collisionCurve)
    {
        Tween.Position(transform, collideTarget, reactionSpeed, 0f, collisionCurve);
    }
}
