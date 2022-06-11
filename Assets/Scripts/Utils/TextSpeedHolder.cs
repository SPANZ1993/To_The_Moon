using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpeedHolder : MonoBehaviour
{
    public TextSpeed textSpeed { get { return _textSpeed; } private set { _textSpeed = value; } }
    [SerializeField]
    private TextSpeed _textSpeed;
}
