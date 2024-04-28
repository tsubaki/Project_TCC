using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.TinyCharacterController.Interfaces.Components;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Unity.TinyCharacterController.Test
{
    public class FitToGroundTest : SceneTestBase
    {
        protected override string ScenePath => "Packages/com.unity.tiny.character.controller/Tests/Scenes/FitToGroundTest.unity";


        [UnityTest]
        public IEnumerator 地面は変化しない()
        {
            var player = GameObject.FindWithTag("Player");

            yield return null;


            Assert.That(player.transform.position, Is.EqualTo(new Vector3(0, 0, 0))
                .Using(Vector3EqualityComparer.Instance));
        }

        [UnityTest]
        public IEnumerator 地面が下がる()
        {
            var player = GameObject.FindWithTag("Player");
            var ground = GameObject.Find("Ground");

            ground.transform.position = new Vector3(0, -0.5f, 0);

            yield return null;
            yield return null;
            yield return null;

            Assert.That(player.transform.position, Is.EqualTo(new Vector3(0, -0.5f, 0))
                .Using(Vector3EqualityComparer.Instance));
        }

        [UnityTest]
        public IEnumerator キャラクターを持ち上げる()
        {
            var player = GameObject.FindWithTag("Player");
            player.TryGetComponent(out IWarp warp);
            warp.Warp(new Vector3(0, 0.5f, 5));

            yield return null;
            yield return null;
            yield return null;

            Assert.That(player.transform.position, Is.EqualTo(new Vector3(0, 0, 5))
                .Using(Vector3EqualityComparer.Instance));
        }

        [UnityTest]
        public IEnumerator 地面が消える()
        {
            var player = GameObject.FindWithTag("Player");
            var ground = GameObject.Find("Ground");
            GameObject.Destroy(ground.gameObject);

            yield return null;
            yield return null;

            Assert.That(new Vector3(0, 0, 0), Is.EqualTo(player.transform.position)
                .Using(Vector3EqualityComparer.Instance));
        }
    }
}