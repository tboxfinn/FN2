using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class _tbx_ClientData 
{
   public ulong clientId;
   public int characterId = -1;

   public _tbx_ClientData(ulong clientId)
   {
         this.clientId = clientId;
   }
}
