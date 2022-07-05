using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITappable
{
    void onTapStart();
    void onTapStay();
    void onTapEnd(bool wasFirst);
}
