using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character 
{
    public static class ICharacterFacadeExtensions
    {
        public static void SetPosition(this ICharacterFacade facade, Vector3 position)
        {
            var transform = facade.To<Component>()?.transform;
            var rotation = transform.IsDefault() ? transform.rotation : Quaternion.identity;

            transform?.SetPositionAndRotation(position, rotation);
        }

        public static void SetRotation(this ICharacterFacade facade, Quaternion rotation)
        {
            var transform = facade.To<Component>()?.transform;
            var position = transform.IsDefault() ? transform.position : Vector3.zero;

            transform?.SetPositionAndRotation(position, rotation);
        }

        public static void SetPositionAndRotation(
            this ICharacterFacade facade,
                 Vector3 position,
                 Quaternion rotation)
        {
            facade.To<Component>()?.transform.SetPositionAndRotation(position, rotation);
        }

        public static void SetPositionAndRotation(
            this ICharacterFacade facade,
                 Transform parent,
                 Vector3 position,
                 Quaternion rotation)
        {
            var transform = facade.To<Component>()?.transform;

            transform?.SetParent(parent);
            transform?.SetPositionAndRotation(position, rotation);
        }

        public static void SetLocalPosition(this ICharacterFacade facade, Vector3 position)
        {
            var transform = facade.To<Component>()?.transform;
            var rotation = transform.IsDefault() ? transform.localRotation : Quaternion.identity;

            transform?.SetLocalPositionAndRotation(position, rotation);
        }

        public static void SetLocalRotation(this ICharacterFacade facade, Quaternion rotation)
        {
            var transform = facade.To<Component>()?.transform;
            var position = transform.IsDefault() ? transform.localPosition : Vector3.zero;

            transform?.SetLocalPositionAndRotation(position, rotation);
        }

        public static void SetLocalPositionAndRotation(
            this ICharacterFacade facade,
                 Vector3 position,
                 Quaternion rotation)
        {
            facade.To<Component>()?.transform.SetLocalPositionAndRotation(position, rotation);
        }

        public static void SetLocalPositionAndRotation(
            this ICharacterFacade facade,
                 Transform parent,
                 Vector3 position,
                 Quaternion rotation)
        {
            var transform = facade.To<Component>()?.transform;

            transform?.SetParent(parent);
            transform?.SetLocalPositionAndRotation(position, rotation);
        }

        public static void SetParent(this ICharacterFacade facade, Transform parent)
        {
            facade.To<Component>()?.transform.SetParent(parent);
        }
    }
}
