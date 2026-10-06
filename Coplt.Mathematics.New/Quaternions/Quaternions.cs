namespace Coplt.Mathematics;

public partial struct quaternion;
public partial struct quaternion_d;
public partial struct quaternion_h;

public partial struct quaternion
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 ToEulerZXY()
    {
        var q = value;
        // (-y*z, z*x, x*y, w*w)
        var cross = math.chg_sign(q.yzxw * q.zxyw, new(-1, 1, 1, 1));
        // 2 * (w * q + cross) -> (sinP, Ny, Nz, 4w^2)
        var num = 2 * math.fma(q.wwww, q, cross);
        var sin_p = num.xxxx;
        if (math.abs(sin_p.x) >= 0.99999f)
        {
            // (x*y, z*w, x^2, x^2)
            var prod = q.xzxx * q.ywxx;
            // 2 * (x*y - sign * z*w) on y
            var num_lock = math.chg_sign(2 * (prod.xxxx - prod), sin_p);
            // (x^2, y^2, z^2, w^2)
            var q2 = q * q;
            // 1 - 2 * (y^2 + z^2)
            var den_lock = math.fsm(1, 2, q2.yyyy + q2.zzzz);
            // (_, yaw, _, _)
            var r = math.atan2(num_lock, den_lock);
            // (pitch, yaw, _, _)
            r.x = math.chg_sign(math.F_Half_PI, sin_p).x;
            return (r & new int4(-1, -1, 0, 0).asf).as3;
        }
        else
        {
            // (x^2, y^2, z^2, w^2)
            var q2 = q * q;
            // (x^2 + y^2, x^2 + z^2, x^2 + z^2, x^2 + z^2)
            var sq = q2.xxxx + q2.yzzz;
            // den = 1 - 2 * sq4
            // den.x = 1 - 2*(x^2 + y^2) -> Dy (Yaw)
            // den.y = 1 - 2*(x^2 + z^2) -> Dz (Roll)
            var den = math.fsm(1, 2, sq);
            // (_, yaw, roll, _)
            var r = math.atan2(num, den.xxyx);
            // (pitch, yaw, roll, _)
            r.x = math.asin(math.clamp(sin_p.x, -1, 1));
            return r.xyz;
        }
    }
}
