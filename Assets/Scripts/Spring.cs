using UnityEngine;

public static class Spring
{
    /*
    x     - value             (input/output)
    v     - velocity          (input/output)
    xt    - target value      (input)
    zeta  - damping ratio     (input)
    omega - angular frequency (input)
    dt    - time step         (input)
    */
    public static float SpringLerp(float x, ref float v, float xt, float zeta, float omega, float dt)
    {
        float f = 1.0f + 2.0f * dt * zeta * omega;
        float oo = omega * omega;
        float hoo = dt * oo;
        float hhoo = dt * hoo;
        float detInv = 1.0f / (f + hhoo);
        float detX = f * x + dt * v + hhoo * xt;
        float detV = v + hoo * (xt - x);
        v = detV * detInv;
        return detX * detInv;
    }

    public static Vector3 Vector3SpringLerp(Vector3 from, ref Vector3 velocity, Vector3 to, float zeta, float omega, float dt)
    {
        var x = SpringLerp(from.x, ref velocity.x, to.x, zeta, omega, dt);
        var y = SpringLerp(from.y, ref velocity.y, to.y, zeta, omega, dt);
        var z = SpringLerp(from.z, ref velocity.z, to.z, zeta, omega, dt);
        return new Vector3(x, y, z);
    }
}