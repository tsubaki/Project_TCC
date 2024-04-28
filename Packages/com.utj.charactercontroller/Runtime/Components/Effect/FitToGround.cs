using System.Collections;
using System.Collections.Generic;
using Unity.TinyCharacterController.Attributes;
using Unity.TinyCharacterController.Interfaces.Components;
using Unity.TinyCharacterController.Interfaces.Core;
using Unity.TinyCharacterController.Utility;
using Unity.VisualScripting;
using UnityEngine;

namespace Unity.TinyCharacterController.Effect
{
    [DisallowMultipleComponent]
    [AddComponentMenu(MenuList.MenuEffect + nameof(FitToGround))]
    [RequireComponent(typeof(CharacterSettings))]
    [RenamedFrom("TinyCharacterController.FitToGround")]

    public class FitToGround : MonoBehaviour, IEffect, IEarlyUpdateComponent
    {
        private CharacterSettings _settings;
        private ITransform _transform;
        private bool _hasGround = false;

        private void Awake()
        {
            TryGetComponent(out _settings);
            TryGetComponent(out _transform);
        }

        private Vector3 _velocity = Vector3.zero;
        private static readonly RaycastHit[] hits = new RaycastHit[15];

        Vector3 IEffect.Velocity => _hasGround ? _velocity : Vector3.zero;

        int IEarlyUpdateComponent.Order => -1;

        void IEffect.ResetVelocity() 
        {
            _velocity = Vector3.zero;
        }

        void IEarlyUpdateComponent.OnUpdate(float deltaTime)
        {
            var roofHitCount = Physics.RaycastNonAlloc(_transform.Position, Vector3.up, hits);
            var hasRoof = _settings.ClosestHit(hits, roofHitCount, float.MaxValue, out var roofHit);

            var offset = hasRoof ? new Vector3(0, roofHit.distance, 0) : new Vector3(0, _settings.Height, 0);
            var upperPosition = _transform.Position + offset;

            var groundHitCount = Physics.RaycastNonAlloc(upperPosition, Vector3.down, hits);
            _hasGround = _settings.ClosestHit(hits, groundHitCount, float.MaxValue, out var groundHit);

            if(_hasGround)
            {
                _velocity = new Vector3(0, offset.y - groundHit.distance , 0) / deltaTime;
            }
            else
            {
                _velocity = Vector3.zero;
            }
        }
    }
}