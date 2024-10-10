using System.Collections.Generic;
using _Root.Scripts.Game.Items.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Root.Scripts.Game.Test
{
    public class NewMonoBehaviorScript : MonoBehaviour
    {
        [ShowInInspector] private Dictionary<string, GameItem> GameItems;

    }
}
