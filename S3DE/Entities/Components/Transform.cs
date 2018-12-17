using S3DE.Maths;
using System.Collections.Generic;

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

        public Vector3 Position
        {
            get { CheckUpdate(); return worldPosition; }
            set => SetPosition(value, Space.World);
        }

        public Quaternion Rotation
        {
            get { CheckUpdate(); return worldQuatRotation; }
            set => SetRotation(value, Space.World);
        }

        public Vector3 Scale
        {
            get { CheckUpdate(); return worldScale; }
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
        public Vector3 QuatUp => quatUp;
        public Vector3 QuatForward => quatForward;
        public Vector3 Backward => -Forward;
        public Vector3 Left => -Right;
        public Vector3 Down => -Up;

        Vector3 worldPosition, localPosition;
        Quaternion worldQuatRotation, localQuatRotation;
        Vector3 worldScale, localScale;

        Vector3 forward, up, right,quatUp,quatForward;

        Matrix4x4 worldTranslationMatrix, worldRotMatrix, worldScaleMatrix, worldTransformMatrix;

        public Matrix4x4 WorldTranslationMatrix { get { CheckUpdate(); return worldTranslationMatrix; } }
        public Matrix4x4 WorldRotationMatrix { get { CheckUpdate(); return worldRotMatrix; } }
        public Matrix4x4 WorldScaleMatrix { get { CheckUpdate(); return worldScaleMatrix; } }
        public Matrix4x4 WorldTransformMatrix { get { CheckUpdate(); return worldTransformMatrix; } }

        protected override void PostRender() => hasChanged = false;

        bool ShouldUpdate()
        {
            return !(rotUpdated & posUpdated & scaleUpdated);
        }

        public void ForceUpdate()
        {
            CheckUpdate();
        }

        void CheckUpdate()
        {
            if (ShouldUpdate())
            {
                if (!rotUpdated)
                    UpdateWorldRotation();

                if (!scaleUpdated)
                    UpdateWorldScale();

                if (!posUpdated)
                    UpdateWorldPosition();

                if ((!scaleUpdated || !rotUpdated) && posUpdated)
                    UpdateWorldTransform();

                rotUpdated = true;
                scaleUpdated = true;
                posUpdated = true;
                UpdateChildren();
            }
        }

        public void Translate(Vector3 direction, float distance) => Translate(direction, distance, Space.World);

        public void Translate(Vector3 direction, float distance, Space space)
        {
            switch (space)
            {
                case Space.World : {SetPosition(this.Position + (direction * distance), Space.World); break;}
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
                        localPosition = (parent == null) ? position : 
                            (position - parent.Position).Transform(parent.Rotation.Conjugate()) 
                            / parent.Scale;
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

        public void SetRotation(Quaternion quat, Space space)
        {
            hasChanged = true;
            switch (space)
            {
                case Space.World:
                    {
                        localQuatRotation = (parent == null) ? quat : parent.Rotation.Conjugate() * quat;
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

        public void Rotate(Quaternion q) => Rotate(q, Space.World);
        public void Rotate(Vector3 axis, float angle) => Rotate(axis, angle, Space.World);

        public void Rotate(Vector3 axis, float angle, Space space) => Rotate(Quaternion.CreateFromAxisAngle(axis, angle), space);

        public void Rotate(Quaternion q, Space s) => SetRotation(q * (s == Space.World ? Rotation : localQuatRotation), s);

        public void SetScale(Vector3 scale, Space space)
        {
            switch (space)
            {
                case Space.World: { localScale = parent == null ? scale : scale / parent.worldScale; break; }
                case Space.Local: { localScale = scale; break; }
            }

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
            Vector3 pos = c.Position;
            Quaternion rot = c.Rotation;
            Vector3 scale = c.Scale;
            c.parent = this;
            c.SetPosition(pos, Space.World);
            c.SetRotation(rot, Space.World);
            c.SetScale(scale, Space.World);
            c.RecalculateMatrices();
        }

        private void RemoveChild(Transform c)
        {
            children.Remove(c);

            Vector3 pos = c.Position;
            Quaternion rot = c.Rotation;
            Vector3 scale = c.Scale;

            c.parent = null;

            c.localPosition = pos;
            c.localQuatRotation = rot;
            c.localScale = scale;
            c.RecalculateMatrices();
        }


        private void UpdateWorldPosition()
        {
            Matrix4x4 transMatrix = Matrix4x4.CreateTranslationMatrix(localPosition);
            worldTranslationMatrix = (parent == null) ? transMatrix : transMatrix * parent.WorldTranslationMatrix;
            UpdateWorldTransform();
            worldPosition = Vector3.Zero.Transform(worldTransformMatrix);
        }

        private void UpdateWorldTransform()
        {
            Matrix4x4 transformMatrix = Matrix4x4.CreateTransformMatrix(localPosition, localQuatRotation, localScale);
            worldTransformMatrix = (parent == null) ? transformMatrix : transformMatrix * parent.WorldTransformMatrix;
        }
        private void UpdateWorldRotation()
        {
            worldQuatRotation = (parent == null) ? localQuatRotation : parent.Rotation * localQuatRotation;
            worldRotMatrix = Matrix4x4.CreateRotationMatrix(worldQuatRotation);

            right = Vector3.Right.Transform(worldRotMatrix);
            up = Vector3.Up.Transform(worldRotMatrix);
            quatUp = Vector3.Up.Transform(worldQuatRotation);
            forward = Vector3.Forward.Transform(worldRotMatrix);
            quatForward = Vector3.Forward.Transform(worldQuatRotation);
        }

        private void UpdateWorldScale()
        {
            Matrix4x4 m = Matrix4x4.CreateScaleMatrix(localScale);
            worldScaleMatrix = (parent == null) ? m : parent.WorldScaleMatrix * m;
            worldScale = Vector3.One.Transform(worldScaleMatrix);
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
            worldRotMatrix = Quaternion.Identity.ToRotationMatrix();
            worldScaleMatrix = Matrix4x4.CreateScaleMatrix(Vector3.One);

            Scale = Vector3.One;
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;

            forward = Vector3.Forward;
            up = Vector3.Up;
            quatUp = Vector3.Up;
            right = Vector3.Right;
            hasChanged = true;

            RecalculateMatrices();
        }
    }
}
