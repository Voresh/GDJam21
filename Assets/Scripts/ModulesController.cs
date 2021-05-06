using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ModulesController : JamBase<ModulesController> {
    [FormerlySerializedAs("_Modules")]
    public List<Module> Modules = new List<Module>();
    
}
