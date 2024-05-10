using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Codice.Client.BaseCommands.BranchExplorer;

namespace Loyufei
{
    public interface ISubCondition 
    {
        public bool Condition();
    }
}