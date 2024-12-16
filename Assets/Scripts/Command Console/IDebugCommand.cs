using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDebugCommand
{
    string Id { get; }
    string Description { get; }
    string Format { get; }
    void Execute(string input);
}

