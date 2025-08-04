using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VNTextData : TextData
{
    public string text;
    [SerializeReference]
    public List<Command> commands;

    public VNTextData()
    {
        text = "";
        commands = new List<Command>();
    }
}
