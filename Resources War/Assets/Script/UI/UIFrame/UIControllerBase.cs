using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
    public  class UIControllerBase
    {
         protected UIModuleBase _module;
       
        public virtual void ControllerStart(UIModuleBase module)
        {
            _module = module;
        }
    }
}