using S3DECore.Input;
using S3DECore.Math;
using System;

namespace S3DE.Components
{
    public class SimpleCameraController : EntityComponent
    {
        Vector3 mVec,fVec;
        float xRot = 0, yRot = 0;
        
        protected override void Update()
        {
            mVec.x = 0;
            mVec.y = 0;
            mVec.z = 0;

            if (Keyboard.GetKey(KeyCode.W).CheckState(KeyState.Down))
                mVec.z += 1f;
            if (Keyboard.GetKey(KeyCode.S).CheckState(KeyState.Down))
                mVec.z -= 1f;
            if (Keyboard.GetKey(KeyCode.D).CheckState(KeyState.Down))
                mVec.x += 1;
            if (Keyboard.GetKey(KeyCode.A).CheckState(KeyState.Down))
                mVec.x -= 1;
            if (Keyboard.GetKey(KeyCode.SPACE).CheckState(KeyState.Down))
                mVec.y += 1;
            if (Keyboard.GetKey(KeyCode.LEFT_SHIFT).CheckState(KeyState.Down))
                mVec.y -= 1;

            fVec = (transform.Forward * mVec.z) + (transform.Right * mVec.x);
            fVec.y += mVec.y;
            
            if (fVec != Vector3.Zero)
                transform.Translate(fVec, 5f * (float)DeltaTime, Space.Local);

            

            if (Mouse.CheckState(MouseState.IsMoving))
            {
                Vector2 mouseDelta = Mouse.Delta;
                xRot += (float)(mouseDelta.x * -45f);
                yRot += (float)(mouseDelta.y * -45f);
                
                xRot = MathFun.Normalize(-180, 180, xRot);
                yRot = MathFun.Clamp(-90, 90, yRot);


                Quaternion q1 = Quaternion.CreateFromAxisAngle(Vector3.Up, xRot);
                Quaternion q2 = Quaternion.CreateFromAxisAngle(Vector3.Right, yRot);

                transform.SetRotation(q1 * q2, Space.Local);
            }
        }
    }
}
