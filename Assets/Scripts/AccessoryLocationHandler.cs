using UnityEngine;

public class AccessoryLocationHandler : MonoBehaviour
{
    public GameObject Sword;
    public Animator animator;
    public HumanBodyBones handBone = HumanBodyBones.RightHand;
    private Vector3 swordOffset = new Vector3(0.0011f, 0.006f, -0.006f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform hand = animator.GetBoneTransform(handBone);
        Sword.transform.SetParent(hand);
        Sword.transform.localPosition = swordOffset;
        Sword.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
