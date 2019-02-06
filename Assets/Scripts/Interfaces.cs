using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPowerUp
{
    IEnumerator ApplyPowerup(Controller number);
}

