using UnityEngine;

public class BotonPuente : MonoBehaviour
{

    private Animation animation;
    [SerializeField] private Animator puenteMover;


    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        animation.enabled = true;
        puenteMover.enabled = true;
    }

}
