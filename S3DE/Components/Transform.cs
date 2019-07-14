using S3DECore.Math;
using System.Collections.Generic;
using S3DE.Collections;

namespace S3DE.Components
{
    public class Transform : EntityComponent
    {
        Transform parent;
        List<Transform> children;
        
        
        public bool HasChanged => hasChanged;
        public bool NeedsUpdate { get; private set; }

        public Transform Parent => parent;
        bool recalcScale, recalcRot, recalcPos;
        bool hasChanged;
        

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
        public Vector3 Backward => Forward.Inverse();
        public Vector3 Left => Right.Inverse();
        public Vector3 Down => Up.Inverse();

        Vector3 worldPosition, localPosition;
        Quaternion worldQuatRotation, localQuatRotation;
        Vector3 worldScale, localScale;

        Vector3 forward, up, right;
        Matrix4x4 worldTransformMatrix,localTransformMatrix;

        public Matrix4x4 WorldTransformMatrix { get { CheckUpdate(); return worldTransformMatrix; } }
        public Matrix4x4 LocalTransformMatrix { get { CheckUpdate(); return localTransformMatrix; } }

        protected override void PostRender() => hasChanged = false;
        void CheckUpdate()
        {
            if (this.NeedsUpdate)
            {
                //Find the root of the update.
                Transform t = this;
                while (t.parent != null && t.parent.NeedsUpdate)
                    t = t.parent;

                t.PerformUpdate();
            }
        }

        void PerformUpdate()
        {
            SimpleQueue<(byte,Transform)> transforms = new SimpleQueue<(byte,Transform)>();
            transforms.Enqueue((0,this));
            (byte UpdateMask,Transform Transform) curr;
            while (transforms.Count > 0)
            {
                curr = transforms.Dequeue();
                byte upByte = curr.Transform.Recalculate(curr.UpdateMask);
                for (int i = 0; i < curr.Transform.children.Count; i++)
                    transforms.Enqueue((upByte,curr.Transform.children[i]));
            }
        }

        byte Recalculate(byte UpdateMask)
        {
            UpdateMask |= (byte)(recalcScale ? 0x3 : 0x0);
            UpdateMask |= (byte)(recalcRot ? 0x5 : 0x0);
            UpdateMask |= (byte)(recalcPos ? 0x1 : 0x0);

            switch (UpdateMask)
            {
                case 0x1:
                    {
                        UpdateWorldTransform();
                        UpdateWorldPosition();
                        break;
                    }
                case 0x3:
                    {
                        UpdateWorldScale();
                        UpdateWorldTransform();
                        UpdateWorldPosition();
                        break;
                    }
                case 0x5:
                    {
                        UpdateWorldRotation();
                        UpdateWorldTransform();
                        UpdateWorldPosition();
                        break;
                    }
                case 0x7:
                    {
                        UpdateWorldScale();
                        UpdateWorldRotation();
                        UpdateWorldTransform();
                        UpdateWorldPosition();
                        break;
                    }
            }
            
            NeedsUpdate = false;
            recalcScale = false;
            recalcPos = false;
            recalcRot = false;

            return UpdateMask;
        }

        void FlagUpdateRequired()
        {
            SimpleQueue<Transform> transforms = new SimpleQueue<Transform>();
            transforms.Enqueue(this);
            Transform t;
            while (transforms.Count > 0)
            {
                t = transforms.Dequeue();
                if (t.NeedsUpdate)
                    continue;
                else
                {
                    t.NeedsUpdate = true;
                    for (int i = 0; i < t.children.Count; i++)
                        transforms.Enqueue(t.children[i]);
                }
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
                        if (parent == null)
                            localPosition = position;
                        else
                        {
                            //If we have a parent, calculate the distance between the parents position and our target pos.
                            localPosition = (position - parent.Position).Transform(parent.Rotation.Conjugate()) / parent.Scale;
                        }
                        
                        break;
                    }
                case Space.Local:
                    {
                        localPosition = position;
                        break;
                    }
            }
            recalcPos = true;
            FlagUpdateRequired();
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
            recalcRot = true;
            FlagUpdateRequired();
        }

        public void Rotate(Quaternion q) => Rotate(q, Space.World);
        public void Rotate(Vector3 axis, float angle) => Rotate(axis, angle, Space.World);

        public void Rotate(Vector3 axis, float angle, Space space) => Rotate(Quaternion.CreateFromAxisAngle(axis, angle), space);

        public void Rotate(Quaternion q, Space s) => SetRotation((s == Space.World ? worldQuatRotation : localQuatRotation) * q, s);

        public void SetScale(Vector3 scale, Space space)
        {
            hasChanged = true;
            switch (space)
            {
                
                case Space.World: { localScale = parent == null ? scale : scale / parent.Scale; break; }
                case Space.Local: { localScale = scale; break; }
            }
            recalcScale = true;
            NeedsUpdate = true;
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
            c.FlagUpdateRequired();
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
            c.FlagUpdateRequired();
        }


        private void UpdateWorldPosition()
        {
            worldPosition = localPosition;
            if (parent != null)
                worldPosition = worldPosition.Transform(parent.WorldTransformMatrix);
        }

        private void UpdateWorldTransform()
        {
            localTransformMatrix = Matrix4x4.CreateWorldMatrix(
                localPosition, localScale, localQuatRotation);

            worldTransformMatrix = (parent == null) ? localTransformMatrix : localTransformMatrix * parent.worldTransformMatrix;

        }

        private void UpdateWorldRotation()
        {
            worldQuatRotation = (parent == null) ? localQuatRotation : parent.worldQuatRotation * localQuatRotation;

            right = Vector3.Right.Transform(worldQuatRotation);
            up = Vector3.Up.Transform(worldQuatRotation);
            forward = Vector3.Forward.Transform(worldQuatRotation);
        }

        private void UpdateWorldScale()
        {
            worldScale = (parent == null) ? localScale : localScale * parent.worldScale;
        }

        protected override void OnCreation()
        {
            children = new List<Transform>();
            
            worldTransformMatrix = Matrix4x4.Identity;
            Scale = Vector3.One;
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;

            forward = Vector3.Forward;
            up = Vector3.Up;
            right = Vector3.Right;
            hasChanged = true;
            NeedsUpdate = true;
        }
    }
}
