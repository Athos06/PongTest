﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterInput
{
    List<Command> GetInput();
}
