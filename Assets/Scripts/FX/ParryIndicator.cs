using UnityEngine;

public class ParryIndicator : MonoBehaviour
{
    public Animator anim { get; private set; }
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartParry() => anim.SetTrigger("start");
    public void StopParry() => anim.SetTrigger("stop");
    public void DestroyMe() => Destroy(gameObject);
}
