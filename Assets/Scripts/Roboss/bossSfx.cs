using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossSfx : MonoBehaviour
{
    [SerializeField] GameObject boss;
    AudioSource walk;
    // Start is called before the first frame update
    void Start()
    {
        walk = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        walk.enabled = (boss.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Boss Walking"))?true:false;
    }
}
