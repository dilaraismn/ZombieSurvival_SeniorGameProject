using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peter : MonoBehaviour
{
    public static Peter instance;
    private Animator _animator;

    private void Awake()
    {
        instance = this;
        _animator = GetComponent<Animator>();
    }

    public void PlayThankfulAnim()
    {
        _animator.Play("Thankful");
    }
    public void PlayNoAnim()
    {
        _animator.Play("No");
    }
}
