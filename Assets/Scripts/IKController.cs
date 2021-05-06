using UnityEngine;

public class IKController : MonoBehaviour {
    public Animator Animator;
    public Transform LeftHandTarget;
    public Transform RightHandTarget;

    private void OnAnimatorIK(int layerIndex) {
        Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        Animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget.position);
        Animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandTarget.rotation);
        Animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.position);
        Animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTarget.rotation);
    }

    private void OnEnable() {
        Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
    }

    private void OnDisable() {
        Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
        Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
        Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
        Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
    }
}
