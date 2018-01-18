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

        public Vector3 Position
        {
            get => worldPosition;
            set => SetPosition(value, Space.World);
        }

        public Quaternion Rotation
        {
            get => worldQuatRotation;
        }

        public Vector3 Scale
        {
            get => worldScale;
        }

        public Vector3 LocalScale
        {
            get => localScale;
        }

        public Vector3 LocalPosition
        {
            get => localPosition;
            set => SetPosition(value, Space.Local);
        }

        public Quaternion LocalRotation
        {
            get => localQuatRotation;
        }

        Vector3 worldPosition, localPosition;
        Quaternion worldQuatRotation, localQuatRotation;
        Vector3 worldScale, localScale;

        Matrix4x4 worldTranslationMatrix, worldRotMatrix, worldScaleMatrix, worldTransformMatrix;
        
        public Matrix4x4 WorldTranslationMatrix => worldTranslationMatrix;
        public Matrix4x4 WorldRotationMatrix => worldRotMatrix;
        public Matrix4x4 WorldScaleMatrix => worldScaleMatrix;
        public Matrix4x4 WorldTransformMatrix => worldTransformMatrix;

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
        }

        public void SetParent(Transform nParent)
        {
            if (nParent != this && nParent != parent)
            {
                //Should probably add a check to make sure that
                //we don't set a transform to have it's grandchild or child as its parent.
                if (parent != null)
                    parent.RemoveChild(this);

                if (nParent != null)
                    nParent.AddChild(this);
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

        protected override void OnCreation()
        {
            worldTranslationMatrix = new Matrix4x4().SetIdentity();
            worldRotMatrix = Quaternion.Identity.ToRotationMatrix();
            worldScaleMatrix = Matrix4x4.CreateScaleMatrix(new Vector3(1, 1, 1));

            worldScale = Vector3.One;
            localScale = Vector3.One;

            worldPosition = Vector3.Zero;
            localPosition = Vector3.Zero;

            worldQuatRotation = Quaternion.Identity;
            localQuatRotation = Quaternion.Identity;
        }
    }
}
