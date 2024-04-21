using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.TinyCharacterController.Interfaces.Core;
using Unity.TinyCharacterController.Utility;

namespace Unity.TinyCharacterController.Control
{

    /// <summary>
    /// Update the character's orientation to the direction specified by <see cref="Look"/>.
    ///
    /// If <see cref="TurnPriority"/> is high, the character is turned in the direction of the stick movement.
    /// </summary>
    [AddComponentMenu(MenuList.MenuControl + nameof(LookStickDirectionControl))]
    [RequireComponent(typeof(CharacterSettings))]
    [RenamedFrom("TinyCharacterController.StickLookControl")]
    [RenamedFrom("Unity.TinyCharacterController.Control.StickLookControl")]
    [Unity.VisualScripting.RenamedFrom("TinyCharacterController.Control.StickLookControl")]
    public class LookStickDirectionControl : MonoBehaviour, 
        ITurn
    {
        /// <summary>
        /// Rotation priority. When the priority is higher than the priority of other components,
        /// it turns in the direction specified by the stick.
        /// </summary>
        [FormerlySerializedAs("_turnPriority")] 
        [SerializeField]
        public int TurnPriority = 1;

        /// <summary>
        /// Speed to change orientation
        /// </summary>
        [SerializeField, Range(-1, 50)] 
        private int _turnSpeed = -1;

        private bool _hasInput = false;

        /// <summary>
        /// Turns in the direction specified by the stick. The direction is compensated by the camera orientation.
        /// </summary>
        /// <param name="rightStick">X faces left and right on the screen space, Y faces up and down on the screen space</param>.
        public void Look(Vector2 rightStick)
        {
            _hasInput = rightStick != Vector2.zero;

            if (_hasInput == false)
                return;

            var rotation = Quaternion.AngleAxis(_settings.CameraTransform.rotation.eulerAngles.y, Vector3.up);
            var target = Quaternion.LookRotation(new Vector3(rightStick.x, 0, rightStick.y), Vector3.up);
            _yawAngle = (rotation * target).eulerAngles.y;
        }

        private CharacterSettings _settings;

        private float _yawAngle;

        int IPriority<ITurn>.Priority => _hasInput ? TurnPriority : 0;
        int ITurn.TurnSpeed => _turnSpeed;
        float ITurn.YawAngle => _yawAngle;

        private void Awake()
        {
            TryGetComponent(out _settings);            
        }
    }

}