using S3DE.Collections;
using S3DE.Maths;
using S3DE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Components
{
    public class Transform : EntityComponent
    {
        Transform parent;
        List<Transform> children;
        
        public bool HasChanged => hasChanged;
        bool hasChanged;
        bool rotUpdated, scaleUpdated, posUpdated;

        public Transform Parent => parent;

        public System.Numerics.Vector3 Position
        {
            get { CheckUpdate(); return worldPosition; }
            set => SetPosition(value, Space.World);
        }

        public System.Numerics.Quaternion Rotation
        {
            get { CheckUpdate(); return worldQuatRotation; }
            set => SetRotation(value, Space.World);
        }

        public System.Numerics.Vector3 Scale
        {
            get { CheckUpdate(); return worldScale; }
            set => SetScale(value, Space.World);
        }

        public System.Numerics.Vector3 LocalScale
        {
            get => localScale;
            set => SetScale(value, Space.Local);
        }

        public System.Numerics.Vector3 LocalPosition
        {
            get => localPosition;
            set => SetPosition(value, Space.Local);
        }

        public System.Numerics.Quaternion LocalRotation
        {
            get => localQuatRotation;
            set => SetRotation(value, Space.Local);
        }

        public System.Numerics.Vector3 Forward => forward;
        public System.Numerics.Vector3 Right => right;
        public System.Numerics.Vector3 Up => up;
        public System.Numerics.Vector3 Backward => -Forward;
        public System.Numerics.Vector3 Left => -Right;
        public System.Numerics.Vector3 Down => -Up;

        System.Numerics.Vector3 worldPosition, localPosition;
        System.Numerics.Quaternion worldQuatRotation, localQuatRotation;
        System.Numerics.Vector3 worldScale, localScale;

        System.Numerics.Vector3 forward, up, right;

        Matrix4x4 worldTranslationMatrix, worldRotMatrix, worldScaleMatrix, worldTransformMatrix;

        public Matrix4x4 WorldTranslationMatrix { get { CheckUpdate(); return worldTranslationMatrix; } }
        public Matrix4x4 WorldRotationMatrix { get { CheckUpdate(); return worldRotMatrix; } }
        public Matrix4x4 WorldScaleMatrix { get { CheckUpdate(); return worldScaleMatrix; } }
        public Matrix4x4 WorldTransformMatrix { get { CheckUpdate(); return worldTransformMatrix; } }

        protected override void PostRender() => hasChanged = false;


        void CheckUpdate()
        {
            if (hasChanged && !(rotUpdated & posUpdated & scaleUpdated))
            {
                if (!rotUpdated)
                    UpdateWorldRotation();

                if (!scaleUpdated)
                    UpdateWorldScale();

                if (!posUpdated)
                    UpdateWorldPosition();
                else
                    UpdateWorldTransform();

                UpdateChildren();
                rotUpdated = true;
                scaleUpdated = true;
                posUpdated = true;
            }
        }

        public void Translate(System.Numerics.Vector3 direction, float distance) => Translate(direction, distance, Space.World);

        public void Translate(System.Numerics.Vector3 direction, float distance, Space space)
        {
            switch (space)
            {
                case Space.World : {SetPosition(this.Position + (direction * distance), Space.World); break;}
                case Space.Local : {SetPosition(localPosition + (direction * distance), Space.Local); break;}
            }
        }

        public void SetPosition(System.Numerics.Vector3 position, Space space)
        {
            hasChanged = true;
            switch (space)
            {
                case Space.World:
                    {
                        //Take parent scale matrix into account aswell.
                        localPosition = (parent == null) ? position : System.Numerics.Vector3.Transform((position - parent.Position),parent.Rotation.conjugate());
                        break;
                    }
                case Space.Local:
                    {
                        localPosition = position;
                        break;
                    }
            }

            posUpdated = false;
        }

        public void SetRotation(System.Numerics.Quaternion quat, Space space)
        {
            hasChanged = true;
            switch (space)
            {
                case Space.World:
                    {
                        localQuatRotation = (parent == null) ? quat : parent.Rotation.conjugate() * quat;
                        break;
                    }
                case Space.Local:
                    {
                        localQuatRotation = quat;
                        break;
                    }
            }

            rotUpdated = false;
        }

        public void Rotate(System.Numerics.Quaternion q) => Rotate(q, Space.World);
        public void Rotate(System.Numerics.Vector3 axis, float angle) => Rotate(axis, angle, Space.World);

        public void Rotate(System.Numerics.Vector3 axis, float angle, Space space) => Rotate(VectorExtensions.Quat_CreateFromAxisAngle(axis, angle), space);

        public void Rotate(System.Numerics.Quaternion q, Space s) => SetRotation(q * (s == Space.World ? Rotation : localQuatRotation), s);

        public void LookAt(System.Numerics.Vector3 target) => SetRotation(VectorExtensions.Quat_CreateLookAt(transform.Position, target, VectorExtensions.Vec3_Up), Space.World);
        

        public void SetScale(System.Numerics.Vector3 scale, Space space)
        {
            localScale = scale;
            hasChanged = true;
            scaleUpdated = false;
        }

        public void SetParent(Transform nParent)
        {
            //Remember to recalculate the matrices if we change our parent!
            if (nParent != null && nParent != this && nParent != parent)
            {
                if (parent != null)
                    parent.RemoveChild(this);

                nParent.AddChild(this);
            }
            else if (nParent == null && parent != null)
            {
                //Set worldpos/rot/scale as local pos/rot/scale.
                parent.RemoveChild(this);
            }
        }

        private void AddChild(Transform c)
        {
            Transform p = this;

            while (p != null)
            {
                if (p.parent == c)
                {
                    p.SetParent(null);
                    break;
                } else
                {
                    p = p.parent;
                }
            }
            children.Add(c);
            c.parent = this;
            c.RecalculateMatrices();
        }

        private void RemoveChild(Transform c)
        {
            children.Remove(c);

            System.Numerics.Vector3 pos = c.Position;
            System.Numerics.Quaternion rot = c.Rotation;
            System.Numerics.Vector3 scale = c.Scale;

            c.parent = null;

            c.localPosition = pos;
            c.localQuatRotation = rot;
            c.localScale = scale;
            c.RecalculateMatrices();
        }


        private void UpdateWorldPosition()
        {
            Matrix4x4 transMatrix = Matrix4x4.CreateTranslationMatrix(localPosition);
            worldTranslationMatrix = (parent == null) ? transMatrix : transMatrix * parent.worldTranslationMatrix;
            UpdateWorldTransform();
            worldPosition = VectorExtensions.Vec3_Zero.Transform(worldTransformMatrix);
        }

        private void UpdateWorldTransform()
        {
            Matrix4x4 transformMatrix = Matrix4x4.CreateTransformMatrix(localPosition, localScale, localQuatRotation);
            worldTransformMatrix = (parent == null) ? transformMatrix : transformMatrix * parent.WorldTransformMatrix;
        }
        private void UpdateWorldRotation()
        {
            worldQuatRotation = (parent == null) ? localQuatRotation : parent.Rotation * localQuatRotation;
            worldRotMatrix = Matrix4x4.CreateRotationMatrix(worldQuatRotation);

            right = VectorExtensions.Vec3_Right.Transform(worldRotMatrix);
            up = VectorExtensions.Vec3_Up.Transform(worldRotMatrix);
            forward = VectorExtensions.Vec3_Forward.Transform(worldRotMatrix);
        }

        private void UpdateWorldScale()
        {
            Matrix4x4 m = Matrix4x4.CreateScaleMatrix(localScale);
            worldScaleMatrix = (parent == null) ? m : parent.WorldScaleMatrix * m;
            worldScale = VectorExtensions.Vec3_One.Transform(worldScaleMatrix);
        }

        private void UpdateChildren()
        {
            foreach (Transform child in children)
                child.RecalculateMatrices();
        }

        private void RecalculateMatrices()
        {
            hasChanged = true;
            
            UpdateWorldScale();
            UpdateWorldRotation();
            UpdateWorldPosition();
            rotUpdated = true;
            posUpdated = true;
            scaleUpdated = true;
            UpdateChildren();
        }

        protected override void OnCreation()
        {
            children = new List<Transform>();

            worldTranslationMatrix = new Matrix4x4().SetIdentity();
            worldRotMatrix = System.Numerics.Quaternion.Identity.ToRotationMatrix();
            worldScaleMatrix = Matrix4x4.CreateScaleMatrix(VectorExtensions.Vec3_One);

            Scale = VectorExtensions.Vec3_One;
            Position = VectorExtensions.Vec3_Zero;
            Rotation = System.Numerics.Quaternion.Identity;

            forward = VectorExtensions.Vec3_Forward;
            up = VectorExtensions.Vec3_Up;
            right = VectorExtensions.Vec3_Right;
            hasChanged = true;

            RecalculateMatrices();
        }
    }
}
