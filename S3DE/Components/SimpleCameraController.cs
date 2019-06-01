using S3DE;
using S3DE.Components;
using S3DE.Input;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (Keyboard.GetKeyState(KeyCode.W) == KeyState.Pressed)
                mVec.z += 1f;
            if (Keyboard.GetKeyState(KeyCode.S) == KeyState.Pressed)
                mVec.z -= 1f;
            if (Keyboard.GetKeyState(KeyCode.D) == KeyState.Pressed)
                mVec.x += 1;
            if (Keyboard.GetKeyState(KeyCode.A) == KeyState.Pressed)
                mVec.x -= 1;
            if (Keyboard.GetKeyState(KeyCode.SPACE) == KeyState.Pressed)
                mVec.y += 1;
            if (Keyboard.GetKeyState(KeyCode.LEFT_SHIFT) == KeyState.Pressed)
                mVec.y -= 1;

            fVec = (transform.Forward * mVec.z) + (transform.Right * mVec.x);
            fVec.y += mVec.y;
            
            if (fVec != Vector3.Zero)
                transform.Translate(fVec, 5f * Time.DeltaTime, Space.Local);
            
            if (Mouse.HasMoved)
            {
                xRot += (float)(Mouse.RawDeltaX * -45f);
                yRot += (float)(Mouse.RawDeltaY * -45f);


                xRot = EngineMath.Normalize(-180, 180, xRot);
                yRot = EngineMath.Clamp(-90, 90, yRot);


                Quaternion q1 = Quaternion.CreateFromAxisAngle(Vector3.Up, xRot);
                Quaternion q2 = Quaternion.CreateFromAxisAngle(Vector3.Right, yRot);

                transform.SetRotation(q1 * q2, Space.Local);
            }
        }
    }
}
