using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MoveTest
    {
        [UnityTest]
        public IEnumerator MoveNorth()
        {
            var gameObject = new GameObject();
            var player = gameObject.AddComponent<PlayerController>();
            yield return null;
        }
    }
}
