using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootStrategy
{
    void Shoot(int damage, Transform transform, bool right);
}
