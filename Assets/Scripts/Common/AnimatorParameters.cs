﻿using UnityEngine;

public class AnimatorParameters
{
    public static readonly int IsGrounded = Animator.StringToHash("is_grounded");
    public static readonly int IsRunning = Animator.StringToHash("is_running");
    public static readonly int InputX = Animator.StringToHash("input_x");
    public static readonly int VelocityY = Animator.StringToHash("velocity_y");
}