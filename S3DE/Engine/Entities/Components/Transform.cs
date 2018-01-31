using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static S3DE.Engine.Enums;

namespace S3DE.Engine.Entities.Components
{
    public class Transform : EntityComponent
    {
        Transform parent;
        List<Transform> children;
        bool hasChanged;

        public bool HasChanged => hasChanged;

        public Vector3 Position
        {
            get => worldPosition;
            set => SetPosition(value, Space.World);
        }

        public Quaternion Rotation
        {
            get => worldQuatRotation;
            set => SetRotation(value, Space.World);
        }

        public Vector3 Scale
        {
            get => worldScale;
            set => SetScale(value, Space.World);
        }

        public Vector3 LocalScale
        {
            get => localScale;
            set => SetScale(value, Space.Local);
        }

        public Vector3 LocalPosition
        {
            get => localPosition;
            set => SetPosition(value, Space.Local);
        }

        public Quaternion LocalRotation
        {
            get => localQuatRotation;
            set => SetRotation(value, Space.Local);
        }

        public Vector3 Forward => forward;
        public Vector3 Right => right;
        public Vector3 Up => up;
        public Vector3 Backward => -Forward;
        public Vector3 Left => -Right;
        public Vector3 Down => -Up;

        Vector3 worldPosition, localPosition;
        Quaternion worldQuatRotation, localQuatRotation;
        Vector3 worldScale, localScale;

        Vector3 forward, up, right;

        Matrix4x4 worldTranslationMatrix, worldRotMatrix, worldScaleMatrix, worldTransformMatrix;
        
        public Matrix4x4 WorldTranslationMatrix => worldTranslationMatrix;
        public Matrix4x4 WorldRotationMatrix => worldRotMatrix;
        public Matrix4x4 WorldScaleMatrix => worldScaleMatrix;
        public Matrix4x4 WorldTransformMatrix => worldTransformMatrix;

        protected override void PostDraw() => hasChanged = false;

        public void Translate(Vector3 direction, float distance) => Translate(direction, distance, Space.World);

        public void Translate(Vector3 direction, float distance, Space space)
        {
            switch (space)
            {
                case Space.World : {SetPosition(worldPosition + (direction * distance), Space.World); break;}
                case Space.Local : {SetPosition(localPosition + (direction * distance), Space.Local); break;}
            }
        }

        public void SetPosition(Vector3 position, Space space)
        {
            hasChanged = true;
            switch (space)
            {
                case Space.World:
                    {
                        //Take parent scale matrix into account aswell.
                        localPosition = (parent == null) ? position : (position - parent.Position) * parent.Rotation.conjugate;
                        break;
                    }
                case Space.Local:
                    {
                        localPosition = position;
                        break;
                    }
            }

            UpdateWorldPosition();
            UpdateChildren();
        }

        public void SetRotation(Quaternion quat, Space space)
        {
            hasChanged = true;
            switch (space)
            {
                case Space.World:
                    {
                        localQuatRotation = (parent == null) ? quat : parent.worldQuatRotation.conjugate * quat;
                        break;
                    }
                case Space.Local:
                    {
                        localQuatRotation = quat;
                        break;
                    }
            }

            UpdateWorldRotation();
            UpdateChildren();
        }

        public void Rotate(Vector3 axis, float angle) => Rotate(axis, angle, Space.World);

        public void Rotate(Vector3 axis, float angle, Space space)
        {
            SetRotation((space == Space.World ? worldQuatRotation : localQuatRotation) * Quaternion.CreateFromAxisAngle(axis, angle), space);
        }

        public void LookAt(Vector3 target) => SetRotation(Quaternion.CreateLookAt(transform.Position, target, Vector3.Up), Space.World);
        

        public void SetScale(Vector3 scale, Space space)
        {
            localScale = scale;
            hasChanged = true;
            UpdateWorldScale();
            UpdateChildren();
        }

        public void SetParent(Transform nParent)
        {
            //Remember to recalculate the matrices if we change our parent!
            if (nParent != null && nParent != this && nParent != parent)
            {
                //Should probably add a check to make sure that
                //we don't set a transform to have it's grandchild or child as its parent.
                if (parent != null)
                    parent.RemoveChild(this);

                nParent.AddChild(this);
                
            } else if (nParent == null && parent != null)
            {
                throw new NotImplementedException();
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

            //children.add(c);
            //bla bla bla, cache scale,rot and pos and reset them and bla bla bla...
        }

        private void RemoveChild(Transform c)
        {

        }


        private void UpdateWorldPosition()
        {
            Matrix4x4 transMatrix = Matrix4x4.CreateTranslationMatrix(localPosition);
            worldTranslationMatrix = (parent == null) ? transMatrix : transMatrix * parent.worldTranslationMatrix;
            UpdateWorldTransform();
            worldPosition = Vector3.Zero * worldTransformMatrix;
        }

        private void UpdateWorldTransform()
        {
            Matrix4x4 transformMatrix = Matrix4x4.CreateTransformMatrix(localPosition, localScale, localQuatRotation);
            worldTransformMatrix = (parent == null) ? transformMatrix : transformMatrix * parent.worldTransformMatrix;
        }
        private void UpdateWorldRotation()
        {
            worldQuatRotation = (parent == null) ? localQuatRotation : localQuatRotation * parent.worldQuatRotation;
            worldRotMatrix = Matrix4x4.CreateRotationMatrix(worldQuatRotation);

            right = Vector3.Right * worldRotMatrix;
            up = Vector3.Up * worldRotMatrix;
            forward = Vector3.Forward * worldRotMatrix;
            UpdateWorldTransform();
        }

        private void UpdateWorldScale()
        {
            Matrix4x4 m = Matrix4x4.CreateScaleMatrix(localScale);
            worldScaleMatrix = (parent == null) ? m : parent.WorldScaleMatrix * m;
            worldScale = Vector3.One * worldScaleMatrix;

            UpdateWorldTransform();
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

            UpdateChildren();
        }

        protected override void OnCreation()
        {
            children = new List<Transform>();

            worldTranslationMatrix = new Matrix4x4().SetIdentity();
            worldRotMatrix = Quaternion.Identity.ToRotationMatrix();
            worldScaleMatrix = Matrix4x4.CreateScaleMatrix(Vector3.One);

            Scale = Vector3.One;
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;

            forward = Vector3.Forward;
            up = Vector3.Up;
            right = Vector3.Right;
            hasChanged = true;

            RecalculateMatrices();
        }
    }
}
