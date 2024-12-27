namespace Coplt.Mathematics;

public static partial class math
{
    [MethodImpl(512)]
    public static float3 ToEulerAngles(this quaternion q, RotationOrder order = RotationOrder.Default)
    {
        const float epsilon = 1e-6f;

        //prepare the data
        var qv = q.value;
        var d1 = qv * qv.wwww * new float4(2.0f); //xw, yw, zw, ww
        var d2 = qv * qv.yzxw * new float4(2.0f); //xy, yz, zx, ww
        var d3 = qv * qv;
        var euler = new float3(0.0f);

        const float CUTOFF = (1.0f - 2.0f * epsilon) * (1.0f - 2.0f * epsilon);

        switch (order)
        {
            case RotationOrder.ZYX:
            {
                var y1 = d2.z + d1.y;
                if (y1 * y1 < CUTOFF)
                {
                    var x1 = -d2.x + d1.z;
                    var x2 = d3.x + d3.w - d3.y - d3.z;
                    var z1 = -d2.y + d1.x;
                    var z2 = d3.z + d3.w - d3.y - d3.x;
                    euler = new float3(atan2(x1, x2), asin(y1), atan2(z1, z2));
                }
                else //zxz
                {
                    y1 = math.clamp(y1, -1.0f, 1.0f);
                    var abcd = new float4(d2.z, d1.y, d2.y, d1.x);
                    var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                    var x2 = csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                    euler = new float3(atan2(x1, x2), asin(y1), 0.0f);
                }

                break;
            }

            case RotationOrder.ZXY:
            {
                var y1 = d2.y - d1.x;
                if (y1 * y1 < CUTOFF)
                {
                    var x1 = d2.x + d1.z;
                    var x2 = d3.y + d3.w - d3.x - d3.z;
                    var z1 = d2.z + d1.y;
                    var z2 = d3.z + d3.w - d3.x - d3.y;
                    euler = new float3(atan2(x1, x2), -asin(y1), atan2(z1, z2));
                }
                else //zxz
                {
                    y1 = clamp(y1, -1.0f, 1.0f);
                    var abcd = new float4(d2.z, d1.y, d2.y, d1.x);
                    var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                    var x2 = csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                    euler = new float3(atan2(x1, x2), -asin(y1), 0.0f);
                }

                break;
            }

            case RotationOrder.YXZ:
            {
                var y1 = d2.y + d1.x;
                if (y1 * y1 < CUTOFF)
                {
                    var x1 = -d2.z + d1.y;
                    var x2 = d3.z + d3.w - d3.x - d3.y;
                    var z1 = -d2.x + d1.z;
                    var z2 = d3.y + d3.w - d3.z - d3.x;
                    euler = new float3(atan2(x1, x2), asin(y1), atan2(z1, z2));
                }
                else //yzy
                {
                    y1 = clamp(y1, -1.0f, 1.0f);
                    var abcd = new float4(d2.x, d1.z, d2.y, d1.x);
                    var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                    var x2 = csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                    euler = new float3(atan2(x1, x2), asin(y1), 0.0f);
                }

                break;
            }

            case RotationOrder.YZX:
            {
                var y1 = d2.x - d1.z;
                if (y1 * y1 < CUTOFF)
                {
                    var x1 = d2.z + d1.y;
                    var x2 = d3.x + d3.w - d3.z - d3.y;
                    var z1 = d2.y + d1.x;
                    var z2 = d3.y + d3.w - d3.x - d3.z;
                    euler = new float3(atan2(x1, x2), -asin(y1), atan2(z1, z2));
                }
                else //yxy
                {
                    y1 = clamp(y1, -1.0f, 1.0f);
                    var abcd = new float4(d2.x, d1.z, d2.y, d1.x);
                    var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                    var x2 = csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                    euler = new float3(atan2(x1, x2), -asin(y1), 0.0f);
                }

                break;
            }

            case RotationOrder.XZY:
            {
                var y1 = d2.x + d1.z;
                if (y1 * y1 < CUTOFF)
                {
                    var x1 = -d2.y + d1.x;
                    var x2 = d3.y + d3.w - d3.z - d3.x;
                    var z1 = -d2.z + d1.y;
                    var z2 = d3.x + d3.w - d3.y - d3.z;
                    euler = new float3(atan2(x1, x2), asin(y1), atan2(z1, z2));
                }
                else //xyx
                {
                    y1 = clamp(y1, -1.0f, 1.0f);
                    var abcd = new float4(d2.x, d1.z, d2.z, d1.y);
                    var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                    var x2 = csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                    euler = new float3(atan2(x1, x2), asin(y1), 0.0f);
                }

                break;
            }

            case RotationOrder.XYZ:
            {
                var y1 = d2.z - d1.y;
                if (y1 * y1 < CUTOFF)
                {
                    var x1 = d2.y + d1.x;
                    var x2 = d3.z + d3.w - d3.y - d3.x;
                    var z1 = d2.x + d1.z;
                    var z2 = d3.x + d3.w - d3.y - d3.z;
                    euler = new float3(atan2(x1, x2), -asin(y1), atan2(z1, z2));
                }
                else //xzx
                {
                    y1 = clamp(y1, -1.0f, 1.0f);
                    var abcd = new float4(d2.z, d1.y, d2.x, d1.z);
                    var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                    var x2 = csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                    euler = new float3(atan2(x1, x2), -asin(y1), 0.0f);
                }

                break;
            }
        }

        return EulerReorderBack(euler, order);
    }

    [MethodImpl(256 | 512)]
    internal static float3 EulerReorderBack(float3 euler, RotationOrder order)
    {
        switch (order)
        {
            case RotationOrder.XZY:
                return euler.xzy;
            case RotationOrder.YZX:
                return euler.zxy;
            case RotationOrder.YXZ:
                return euler.yxz;
            case RotationOrder.ZXY:
                return euler.yzx;
            case RotationOrder.ZYX:
                return euler.zyx;
            case RotationOrder.XYZ:
            default:
                return euler;
        }
    }
}
