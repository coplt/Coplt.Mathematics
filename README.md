# Coplt.Mathematics

[![Nuget](https://img.shields.io/nuget/v/Coplt.Mathematics)](https://www.nuget.org/packages/Coplt.Mathematics/)

hlsl-style linear algebra library

- bool (b16 b32 b64)、half [[1]](#note-1)、float、double、short、ushort、int、uint、long、ulong、decimal [[2]](#note-2)
- Vector2~4 (float3 double4 int2 ...)
- Matrix2x2~4x4 (float3x3 double4x2 int2x3 ...)
- Hlsl-like cartesian product swizzle (.yzx .abgr ...)
- Full simd support
- Simd accelerated [[3]](#note-3) log, exp, pow, fmod, trigonometry

> [!NOTE]
> <a name="note-1"></a>
> 1. Very slow on CPU
> <a name="note-2"></a>
> 2. Cpu only
> <a name="note-3"></a>
> 3. Lower precision than C# system library

### Function ULPs (vs C# system library)

| function | float | double |
|----------|-------|--------|
| log2     | 1     | 1      |
| asin     | 2     | 2      |
| acos     | 2     | 2      |
| atan     | 2     | 2      |
| atan2    | 2     | 2      |

- The function that needs to be rewritten due to insufficient precision is not in the table

## Todo

- [ ] tests
- [ ] rewrite the function with too low precision

---

## Related Projects

- https://github.com/coplt/Coplt.Shader
