//
// (C) 2022 Takap.
//

using System.Runtime.CompilerServices;
using UnityEngine;

namespace Takap.Utility
{
    /// <summary>
    /// <see cref="Transform.position"/> の操作のショートカットを定義します。
    /// </summary>
    public static class TrasnformPositionExtensions
    {
        // 
        // Get
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetPos(this Transform target)
        {
            return target.position;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetPosXY(this Transform target)
        {
            var p = target.position;
            return new Vector2(p.x, p.y);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetPosXZ(this Transform target)
        {
            var p = target.position;
            return new Vector2(p.x, p.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetPosYZ(this Transform target)
        {
            var p = target.position;
            return new Vector2(p.y, p.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPosX(this Transform target) => target.position.x;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPosY(this Transform target) => target.position.y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPosZ(this Transform target) => target.position.z;

        // 
        // Set
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPos(this Transform target, in Vector3 v) => target.position = v;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPos(this Transform target, float x, float y, float z)
        {
            var p = target.position;
            p.x = x;
            p.y = y;
            p.z = z;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosXY(this Transform target, float x, float y)
        {
            var p = target.position;
            p.x = x;
            p.y = y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosXY(this Transform target, in Vector2 xy)
        {
            var p = target.position;
            p.x = xy.x;
            p.y = xy.y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosXZ(this Transform target, float x, float z)
        {
            var p = target.position;
            p.x = x;
            p.z = z;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosXZ(this Transform target, in Vector2 xz)
        {
            var p = target.position;
            p.x = xz.x;
            p.z = xz.y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosYZ(this Transform target, float y, float z)
        {
            var p = target.position;
            p.y = y;
            p.z = z;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosYZ(this Transform target, in Vector2 yz)
        {
            var p = target.position;
            p.y = yz.x;
            p.z = yz.y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosX(this Transform target, float x)
        {
            var p = target.position;
            p.x = x;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosY(this Transform target, float y)
        {
            var p = target.position;
            p.y = y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosZ(this Transform target, float z)
        {
            var p = target.position;
            p.z = z;
            target.position = p;
        }

        // 
        // Add
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPos(this Transform target, in Vector3 v)
        {
            var p = target.position;
            p.x += v.x;
            p.y += v.y;
            p.z += v.z;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPos(this Transform target, float x, float y, float z)
        {
            var p = target.position;
            p.x += x;
            p.y += y;
            p.z += z;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosXY(this Transform target, float x, float y)
        {
            var p = target.position;
            p.x += x;
            p.y += y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosXY(this Transform target, in Vector2 xy)
        {
            var p = target.position;
            p.x += xy.x;
            p.y += xy.y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosXZ(this Transform target, float x, float z)
        {
            var p = target.position;
            p.x += x;
            p.z += z;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosXZ(this Transform target, in Vector2 xz)
        {
            var p = target.position;
            p.x += xz.x;
            p.z += xz.y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosYZ(this Transform target, float y, float z)
        {
            var p = target.position;
            p.y += y;
            p.z += z;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosYZ(this Transform target, in Vector2 yz)
        {
            var p = target.position;
            p.y += yz.x;
            p.z += yz.y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosX(this Transform target, float x)
        {
            var p = target.position;
            p.x += x;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosY(this Transform target, float y)
        {
            var p = target.position;
            p.y += y;
            target.position = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPosZ(this Transform target, float z)
        {
            var p = target.position;
            p.z += z;
            target.position = p;
        }

        // 
        // TryAdd
        // The specified value is not reflected. Calculation only.
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddPos(this Transform target, in Vector3 v)
        {
            var p = target.position;
            p.x += v.x;
            p.y += v.y;
            p.z += v.z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddPos(this Transform target, float x, float y, float z)
        {
            var p = target.position;
            p.x += x;
            p.y += y;
            p.z += z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddPosXY(this Transform target, float x, float y)
        {
            var p = target.position;
            p.x += x;
            p.y += y;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddPosXY(this Transform target, in Vector2 xy)
        {
            var p = target.position;
            p.x += xy.x;
            p.y += xy.y;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddPosXZ(this Transform target, float x, float z)
        {
            var p = target.position;
            p.x += x;
            p.z += z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddPosXZ(this Transform target, in Vector2 xz)
        {
            var p = target.position;
            p.x += xz.x;
            p.z += xz.y;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddPosYZ(this Transform target, float y, float z)
        {
            var p = target.position;
            p.y += y;
            p.z += z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddPosYZ(this Transform target, in Vector2 yz)
        {
            var p = target.position;
            p.y += yz.x;
            p.z += yz.y;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddPosX(this Transform target, float x)
        {
            var p = target.position;
            p.x += x;
            return p.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddPosY(this Transform target, float y)
        {
            var p = target.position;
            p.y += y;
            return p.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddPosZ(this Transform target, float z)
        {
            var p = target.position;
            p.z += z;
            return p.z;
        }
    }

    /// <summary>
    /// <see cref="Transform.localPosition"/> の操作のショートカットを定義します。
    /// </summary>
    public static class TrasnformLocalPositionExtensions
    {
        // 
        // Get
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetLocalPos(this Transform target)
        {
            return target.localPosition;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetLocalPosXY(this Transform target)
        {
            var p = target.localPosition;
            return new Vector2(p.x, p.y);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float x, float y) GetLocalPosXZ(this Transform target)
        {
            var p = target.localPosition;
            return (p.x, p.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float y, float z) GetLocalPosYZ(this Transform target)
        {
            var p = target.localPosition;
            return (p.y, p.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalPosX(this Transform target) => target.localPosition.x;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalPosY(this Transform target) => target.localPosition.y;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalPosZ(this Transform target) => target.localPosition.z;

        // 
        // Set
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPos(this Transform target, in Vector3 v) => target.localPosition = v;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPos(this Transform target, float x, float y, float z)
        {
            var p = target.localPosition;
            p.x = x;
            p.y = y;
            p.z = z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosXY(this Transform target, float x, float y)
        {
            var p = target.localPosition;
            p.x = x;
            p.y = y;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosXY(this Transform target, in Vector2 xy)
        {
            var p = target.localPosition;
            p.x = xy.x;
            p.y = xy.y;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosXZ(this Transform target, float x, float z)
        {
            var p = target.localPosition;
            p.x = x;
            p.z = z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosXZ(this Transform target, in (float x, float z) xz)
        {
            var p = target.localPosition;
            p.x = xz.x;
            p.z = xz.z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosYZ(this Transform target, float y, float z)
        {
            var p = target.localPosition;
            p.y = y;
            p.z = z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosYZ(this Transform target, in (float y, float z) yz)
        {
            var p = target.localPosition;
            p.y = yz.y;
            p.z = yz.z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosX(this Transform target, float x)
        {
            var p = target.localPosition;
            p.x = x;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosY(this Transform target, float y)
        {
            var p = target.localPosition;
            p.y = y;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalPosZ(this Transform target, float z)
        {
            var p = target.localPosition;
            p.z = z;
            target.localPosition = p;
        }

        // 
        // Add
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPos(this Transform target, in Vector3 v)
        {
            var p = target.localPosition;
            p.x += v.x;
            p.y += v.y;
            p.z += v.z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPos(this Transform target, float x, float y, float z)
        {
            var p = target.localPosition;
            p.x += x;
            p.y += y;
            p.z += z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosXY(this Transform target, float x, float y)
        {
            var p = target.localPosition;
            p.x += x;
            p.y += y;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosXY(this Transform target, in Vector2 xy)
        {
            var p = target.localPosition;
            p.x += xy.x;
            p.y += xy.y;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosXZ(this Transform target, float x, float z)
        {
            var p = target.localPosition;
            p.x += x;
            p.z += z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosXZ(this Transform target, in (float x, float z) xz)
        {
            var p = target.localPosition;
            p.x += xz.x;
            p.z += xz.z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosYZ(this Transform target, float y, float z)
        {
            var p = target.localPosition;
            p.y += y;
            p.z += z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosYZ(this Transform target, in (float y, float z) yz)
        {
            var p = target.localPosition;
            p.y += yz.y;
            p.z += yz.z;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosX(this Transform target, float x)
        {
            var p = target.localPosition;
            p.x += x;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosY(this Transform target, float y)
        {
            var p = target.localPosition;
            p.y += y;
            target.localPosition = p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalPosZ(this Transform target, float z)
        {
            var p = target.localPosition;
            p.z += z;
            target.localPosition = p;
        }

        // 
        // TryAdd
        // The specified value is not reflected. Calculation only.
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalPos(this Transform target, in Vector3 v)
        {
            var p = target.localPosition;
            p.x += v.x;
            p.y += v.y;
            p.z += v.z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalPos(this Transform target, float x, float y, float z)
        {
            var p = target.localPosition;
            p.x += x;
            p.y += y;
            p.z += z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalPosXY(this Transform target, float x, float y)
        {
            var p = target.localPosition;
            p.x += x;
            p.y += y;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalPosXY(this Transform target, in Vector2 xy)
        {
            var p = target.localPosition;
            p.x += xy.x;
            p.y += xy.y;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalPosXZ(this Transform target, float x, float z)
        {
            var p = target.localPosition;
            p.x += x;
            p.z += z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalPosXZ(this Transform target, in (float x, float z) xz)
        {
            var p = target.localPosition;
            p.x += xz.x;
            p.z += xz.z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalPosYZ(this Transform target, float y, float z)
        {
            var p = target.localPosition;
            p.y += y;
            p.z += z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalPosYZ(this Transform target, in (float y, float z) yz)
        {
            var p = target.localPosition;
            p.y += yz.y;
            p.z += yz.z;
            return p;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalPosX(this Transform target, float x)
        {
            var p = target.localPosition;
            p.x += x;
            return p.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalPosY(this Transform target, float y)
        {
            var p = target.localPosition;
            p.y += y;
            return p.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalPosZ(this Transform target, float z)
        {
            var p = target.localPosition;
            p.z += z;
            return p.z;
        }
    }

    /// <summary>
    /// <see cref="Transform.localEulerAngles"/> の操作のショートカットを定義します。
    /// </summary>
    public static class TransformLocalEulerAnglesExtensions
    {
        // 
        // Get
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetLocalEuler(this Transform self)
        {
            return self.localEulerAngles;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalEulerX(this Transform self)
        {
            return self.localEulerAngles.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalEulerY(this Transform self)
        {
            return self.localEulerAngles.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalEulerZ(this Transform self)
        {
            return self.localEulerAngles.z;
        }

        // 
        // Set
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEuler(this Transform self, float localEulerX, float localEulerY, float localEulerZ)
        {
            self.localEulerAngles = new Vector3(localEulerX, localEulerY, localEulerZ);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEuler(this Transform self, in Vector3 localEuler)
        {
            self.localEulerAngles = localEuler;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerXY(this Transform self, float localEulerX, float localEulerY)
        {
            self.localEulerAngles = new Vector3(localEulerX, localEulerY, self.localEulerAngles.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerXY(this Transform self, in Vector2 localEulerXY)
        {
            self.localEulerAngles = new Vector3(localEulerXY.x, localEulerXY.y, self.localEulerAngles.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerXZ(this Transform self, float localEulerX, float localEulerZ)
        {
            self.localEulerAngles = new Vector3(localEulerX, self.localEulerAngles.y, localEulerZ);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerXZ(this Transform self, in (float x, float z) localEuler)
        {
            self.localEulerAngles = new Vector3(localEuler.x, self.localEulerAngles.y, localEuler.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerYZ(this Transform self, float localEulerY, float localEulerZ)
        {
            self.localEulerAngles = new Vector3(self.localEulerAngles.x, localEulerY, localEulerZ);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerYZ(this Transform self, in (float y, float z) localEuler)
        {
            self.localEulerAngles = new Vector3(self.localEulerAngles.x, localEuler.y, localEuler.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerX(this Transform self, float localEulerX)
        {
            Vector3 a = self.localEulerAngles;
            self.localEulerAngles = new Vector3(localEulerX, a.y, a.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerY(this Transform self, float localEulerY)
        {
            Vector3 a = self.localEulerAngles;
            self.localEulerAngles = new Vector3(a.x, localEulerY, a.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalEulerZ(this Transform self, float localEulerZ)
        {
            Vector3 a = self.localEulerAngles;
            self.localEulerAngles = new Vector3(a.x, a.y, localEulerZ);
        }

        // 
        // Add
        // -- Not necessarily a good idea.
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateXY(this Transform self, float localEulerX, float localEulerY)
        {
            self.Rotate(localEulerX, localEulerY, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateXY(this Transform self, in Vector2 localEulerXY)
        {
            self.Rotate(localEulerXY.x, localEulerXY.y, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateXZ(this Transform self, float localEulerX, float localEulerZ)
        {
            self.Rotate(localEulerX, 0, localEulerZ);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateXZ(this Transform self, in (float x, float z) localEuler)
        {
            self.Rotate(localEuler.x, 0, localEuler.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateYZ(this Transform self, float localEulerY, float localEulerZ)
        {
            self.Rotate(0, localEulerY, localEulerZ);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateYZ(this Transform self, in (float y, float z) localEuler)
        {
            self.Rotate(0, localEuler.y, localEuler.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateX(this Transform self, float localEulerX)
        {
            self.Rotate(localEulerX, 0, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateY(this Transform self, float localEulerY)
        {
            self.Rotate(0, localEulerY, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RotateZ(this Transform self, float localEulerZ)
        {
            self.Rotate(0, 0, localEulerZ);
        }

        // 
        // TryAdd
        // The specified value is not reflected. Calculation only.
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalEuler(this Transform self, float localEulerX, float localEulerY, float localEulerZ)
        {
            var a = self.localEulerAngles;
            a.x += localEulerX;
            a.y += localEulerY;
            a.z += localEulerZ;
            return a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalEuler(this Transform self, in Vector3 localEuler)
        {
            var a = self.localEulerAngles;
            a.x += localEuler.x;
            a.y += localEuler.y;
            a.z += localEuler.z;
            return a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalEulerXY(this Transform self, float localEulerX, float localEulerY)
        {
            var a = self.localEulerAngles;
            a.x += localEulerX;
            a.y += localEulerY;
            return a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalEulerXY(this Transform self, in Vector2 localEulerXY)
        {
            var a = self.localEulerAngles;
            a.x += localEulerXY.x;
            a.y += localEulerXY.y;
            return a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalEulerXZ(this Transform self, float localEulerX, float localEulerZ)
        {
            var a = self.localEulerAngles;
            a.x += localEulerX;
            a.z += localEulerZ;
            return a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalEulerXZ(this Transform self, in (float x, float z) localEuler)
        {
            var a = self.localEulerAngles;
            a.x += localEuler.x;
            a.z += localEuler.z;
            return a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalEulerYZ(this Transform self, float localEulerY, float localEulerZ)
        {
            var a = self.localEulerAngles;
            a.y += localEulerY;
            a.z += localEulerZ;
            return a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalEulerYZ(this Transform self, in (float y, float z) localEuler)
        {
            var a = self.localEulerAngles;
            a.y += localEuler.y;
            a.z += localEuler.z;
            return a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalEulerX(this Transform self, float localEulerX)
        {
            return self.localEulerAngles.x + localEulerX;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalEulerY(this Transform self, float localEulerY)
        {
            return self.localEulerAngles.y + localEulerY;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalEulerZ(this Transform self, float localEulerZ)
        {
            return self.localEulerAngles.z + localEulerZ;
        }
    }

    /// <summary>
    /// <see cref="Transform.localScale"/> の操作のショートカットを定義します。
    /// </summary>
    public static class TransformLocalLocalScaleExtensions
    {
        // 
        // Get
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetLocalScale(this Transform self)
        {
            return self.localScale;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalScaleX(this Transform self)
        {
            return self.localScale.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalScaleY(this Transform self)
        {
            return self.localScale.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLocalScaleZ(this Transform self)
        {
            return self.localScale.z;
        }

        // 
        // Set
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScale(this Transform self, float unifromScale)
        {
            Vector3 v = self.localScale;
            v.x = unifromScale;
            v.y = unifromScale;
            v.z = unifromScale;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScale(this Transform self, float scaleX, float scaleY, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.x = scaleX;
            v.y = scaleY;
            v.z = scaleZ;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScale(this Transform self, in Vector3 value)
        {
            self.localScale = value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleXY(this Transform self, float scaleXY)
        {
            Vector3 v = self.localScale;
            v.x = scaleXY;
            v.y = scaleXY;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleXY(this Transform self, float scaleX, float scaleY)
        {
            Vector3 v = self.localScale;
            v.x = scaleX;
            v.y = scaleY;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleXY(this Transform self, in Vector2 scale)
        {
            Vector3 v = self.localScale;
            v.x = scale.x;
            v.y = scale.y;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleXZ(this Transform self, float scale)
        {
            Vector3 v = self.localScale;
            v.x = scale;
            v.z = scale;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleXZ(this Transform self, float scaleX, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.x = scaleX;
            v.z = scaleZ;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleXZ(this Transform self, in (float x, float z) scale)
        {
            Vector3 v = self.localScale;
            v.x = scale.x;
            v.z = scale.z;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleYZ(this Transform self, float scale)
        {
            Vector3 v = self.localScale;
            v.y = scale;
            v.z = scale;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleYZ(this Transform self, float scaleY, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.y = scaleY;
            v.z = scaleZ;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleYZ(this Transform self, in (float y, float z) scale)
        {
            Vector3 v = self.localScale;
            v.y = scale.y;
            v.z = scale.z;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleX(this Transform self, float scaleX)
        {
            Vector3 v = self.localScale;
            v.x = scaleX;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleY(this Transform self, float scaleY)
        {
            Vector3 v = self.localScale;
            v.y = scaleY;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLocalScaleZ(this Transform self, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.z = scaleZ;
            self.localScale = v;
        }

        // 
        // Add
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScale(this Transform self, float unifromScale)
        {
            Vector3 v = self.localScale;
            v.x = unifromScale;
            v.y = unifromScale;
            v.z = unifromScale;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScale(this Transform self, float scaleX, float scaleY, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.x += scaleX;
            v.y += scaleY;
            v.z += scaleZ;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScale(this Transform self, in Vector3 value)
        {
            self.localScale += value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleXY(this Transform self, float scaleXY)
        {
            Vector3 v = self.localScale;
            v.x += scaleXY;
            v.y += scaleXY;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleXY(this Transform self, float scaleX, float scaleY)
        {
            Vector3 v = self.localScale;
            v.x += scaleX;
            v.y += scaleY;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleXY(this Transform self, in Vector2 scale)
        {
            Vector3 v = self.localScale;
            v.x += scale.x;
            v.y += scale.y;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleXZ(this Transform self, float scale)
        {
            Vector3 v = self.localScale;
            v.x += scale;
            v.z += scale;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleXZ(this Transform self, float scaleX, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.x += scaleX;
            v.z += scaleZ;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleXZ(this Transform self, in (float x, float z) scale)
        {
            Vector3 v = self.localScale;
            v.x += scale.x;
            v.z += scale.z;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleYZ(this Transform self, float scale)
        {
            Vector3 v = self.localScale;
            v.y += scale;
            v.z += scale;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleYZ(this Transform self, float scaleY, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.y += scaleY;
            v.z += scaleZ;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleYZ(this Transform self, in (float y, float z) scale)
        {
            Vector3 v = self.localScale;
            v.y += scale.y;
            v.z += scale.z;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleX(this Transform self, float scaleX)
        {
            Vector3 v = self.localScale;
            v.x += scaleX;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleY(this Transform self, float scaleY)
        {
            Vector3 v = self.localScale;
            v.y += scaleY;
            self.localScale = v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLocalScaleZ(this Transform self, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.z += scaleZ;
            self.localScale = v;
        }

        // 
        // TryAdd
        // 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScale(this Transform self, float unifromScale)
        {
            Vector3 v = self.localScale;
            v.x = unifromScale;
            v.y = unifromScale;
            v.z = unifromScale;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScale(this Transform self, float scaleX, float scaleY, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.x += scaleX;
            v.y += scaleY;
            v.z += scaleZ;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScale(this Transform self, in Vector3 value)
        {
            Vector3 v = self.localScale;
            return v + value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleXY(this Transform self, float scaleXY)
        {
            Vector3 v = self.localScale;
            v.x += scaleXY;
            v.y += scaleXY;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleXY(this Transform self, float scaleX, float scaleY)
        {
            Vector3 v = self.localScale;
            v.x += scaleX;
            v.y += scaleY;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleXY(this Transform self, in Vector2 scale)
        {
            Vector3 v = self.localScale;
            v.x += scale.x;
            v.y += scale.y;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleXZ(this Transform self, float scale)
        {
            Vector3 v = self.localScale;
            v.x += scale;
            v.z += scale;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleXZ(this Transform self, float scaleX, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.x += scaleX;
            v.z += scaleZ;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleXZ(this Transform self, in (float x, float z) scale)
        {
            Vector3 v = self.localScale;
            v.x += scale.x;
            v.z += scale.z;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleYZ(this Transform self, float scale)
        {
            Vector3 v = self.localScale;
            v.y += scale;
            v.z += scale;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleYZ(this Transform self, float scaleY, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.y += scaleY;
            v.z += scaleZ;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TryAddLocalScaleYZ(this Transform self, in (float y, float z) scale)
        {
            Vector3 v = self.localScale;
            v.y += scale.y;
            v.z += scale.z;
            return v;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalScaleX(this Transform self, float scaleX)
        {
            Vector3 v = self.localScale;
            v.x += scaleX;
            return v.x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalScaleY(this Transform self, float scaleY)
        {
            Vector3 v = self.localScale;
            v.y += scaleY;
            return v.y;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float TryAddLocalScaleZ(this Transform self, float scaleZ)
        {
            Vector3 v = self.localScale;
            v.z += scaleZ;
            return v.z;
        }
    }

    public static class Vector2Extensions
    {
        /// <summary>
        /// <see cref="Vector2"/> を Tupleに変換します。
        /// </summary>
        public static void Deconstruct(this Vector2 vector, out float x, out float y)
        {
            x = vector.x;
            y = vector.y;
        }

        /// <summary>
        /// 2つの値の組み合わせを <see cref="Vector2"/> に変換します。
        /// </summary>
        public static Vector2 ToVector2(this (float a, float b) p)
        {
            return new Vector2(p.a, p.b);
        }
    }
}