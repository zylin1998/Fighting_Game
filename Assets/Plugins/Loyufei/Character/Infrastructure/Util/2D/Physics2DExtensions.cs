using Loyufei.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.Character
{
    public static class ICharacterControlExtensions
    {
        public static void SetFixedHorizontalSpeed2D(this Rigidbody2D self, float direction, float speed, ForceMode2D forceMode = ForceMode2D.Force) 
        {
            var hori = direction * speed;

            self.AddForce(new Vector2(hori, 0), forceMode);
        }

        public static void SetFixedVerticalSpeed2D(this Rigidbody2D self, float direction, float speed, ForceMode2D forceMode = ForceMode2D.Force)
        {
            var hori = direction * speed;

            self.AddForce(new Vector2(hori, 0), forceMode);
        }

        public static void SetFixedHorizontalVelocity2D(this Rigidbody2D self, float direction, float speed) 
        {
            var velocityX = direction * speed;

            self.velocity = new Vector2(velocityX, self.velocity.y); ;
        }

        public static void SetFixedVerticalVelocity2D(this Rigidbody2D self, float direction, float speed)
        {
            var velocityY = direction * speed;

            self.velocity = new Vector2(self.velocity.x, velocityY); ;
        }

        public static void GroundCheck(this Rigidbody2D self, float radius, LayerMask groundMask, IReposit isGround)
        {
            var position = self.transform.position;
            var collider = Physics2D.OverlapCircle(position, radius, groundMask);

            isGround.Preserve(collider && self.velocity.y <= 0);
        }

        public static void GroundCheck(this Rigidbody2D self, float radius, LayerMask groundMask, out bool isGround)
        {
            var position = self.transform.position;
            var collider = Physics2D.OverlapCircle(position, radius, groundMask);

            isGround = collider && self.velocity.y <= 0;
        }

        public static void FixedFlip(this Rigidbody2D self, float direction, float smooth = 1, float scaleMax = 1, bool invert = false)
        {
            if (direction == 0) { return; }

            var scale      = self.transform.localScale;
            var scalex     = scale.x;
            var side       = direction > 0 ? 1 : -1;
            var smoothSide = side / smooth * Time.fixedDeltaTime * 2;
            var result     = (invert ? smoothSide * -1 * scaleMax : smoothSide) + scalex;
            var newScale   = new Vector3(Mathf.Clamp(result, -scaleMax, scaleMax), scale.y, scale.z);

            self.transform.localScale = newScale;
        }
    }
}