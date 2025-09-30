using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityNodesRuntimeBank : MonoBehaviour
{
   public static UtilityNodesRuntimeBank instance { get; private set; }

   public UtilityNodesCollection nodesCollection;

   void Awake()
   {
      instance = this;
      nodesCollection = Instantiate(nodesCollection);
   }
}
