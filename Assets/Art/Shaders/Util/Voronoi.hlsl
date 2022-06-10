// blender/source/blender/gpu/shaders/common/gpu_shader_common_hash.glsl
#define rot(x, k) (((x) << (k)) | ((x) >> (32 - (k))))

#define final(a, b, c) \
  { \
    c ^= b; \
    c -= rot(b, 14); \
    a ^= c; \
    a -= rot(c, 11); \
    b ^= a; \
    b -= rot(a, 25); \
    c ^= b; \
    c -= rot(b, 16); \
    a ^= c; \
    a -= rot(c, 4); \
    b ^= a; \
    b -= rot(a, 14); \
    c ^= b; \
    c -= rot(b, 24); \
  }

inline uint hash_uint2(uint kx, uint ky)
{
  uint a, b, c;
  a = b = c = 0xDEADBEEFu + (2u << 2u) + 13u;

  b += ky;
  a += kx;
  final(a, b, c);

  return c;
}

inline uint hash_uint3(uint kx, uint ky, uint kz)
{
  uint a, b, c;
  a = b = c = 0xDEADBEEFu + (3u << 2u) + 13u;

  c += kz;
  b += ky;
  a += kx;
  final(a, b, c);

  return c;
}

inline float hash_uint2_to_float(uint kx, uint ky)
{
  return float(hash_uint2(kx, ky)) / float(0xFFFFFFFFu);
}

inline float hash_uint3_to_float(uint kx, uint ky, uint kz)
{
  return float(hash_uint3(kx, ky, kz)) / float(0xFFFFFFFFu);
}

inline float hash_vec2_to_float(float2 k)
{
  return hash_uint2_to_float(asuint(k.x), asuint(k.y));
}

inline float hash_vec3_to_float(float3 k)
{
  return hash_uint3_to_float(asuint(k.x), asuint(k.y), asuint(k.z));
}

inline float2 hash_vec2_to_vec2(float2 k)
{
  return float2(hash_vec2_to_float(k), hash_vec3_to_float(float3(k.x, k.y, 1.0)));
}

inline float3 hash_vec2_to_vec3(float2 k)
{
  return float3(
      hash_vec2_to_float(k), hash_vec3_to_float(float3(k.x, k.y, 1.0)), hash_vec3_to_float(float3(k.x, k.y, 2.0)));
}
// =====================================================================================

// blender/source/blender/gpu/shades/common/gpu_shader_common_math_utils.glsl
inline float safe_divide(float a, float b)
{
  return (b != 0.0) ? a / b : 0.0;
}

inline float2 safe_divide(float2 a, float2 b)
{
  return float2(safe_divide(a.x, b.x), safe_divide(a.y, b.y));
}
//======================================================================================

// blender/source/blender/gpu/shaders/material/gpu_shader_material_tex_voronoi.glsl
inline float voronoi_distance(float a, float b)
{
  return distance(a, b);
}

void CustomVoronoi_float(float3 coord, float scale, float randomness, out float outDistance, out float4 outColor, out float2 outPosition) {
  outColor = float4(0, 0, 0, 0);
  randomness = saturate(randomness);

  float2 scaledCoord = coord.xy * scale;
  float2 cellPosition = floor(scaledCoord);
  float2 localPosition = scaledCoord - cellPosition;

  float minDistance = 8.0;
  float2 targetOffset, targetPosition;
  for (int j = -1; j <= 1; j++) {
    for (int i = -1; i <= 1; i++) {
      float2 cellOffset = float2(i, j);
      float2 pointPosition = cellOffset + hash_vec2_to_vec2(cellPosition + cellOffset) * randomness;
      float distanceToPoint = voronoi_distance(pointPosition, localPosition);
      if (distanceToPoint < minDistance) {
        targetOffset = cellOffset;
        minDistance = distanceToPoint;
        targetPosition = pointPosition;
      }
    }
  }
  outDistance = minDistance;
  outColor.xyz = hash_vec2_to_vec3(cellPosition + targetOffset);
  outPosition = float3(safe_divide(targetPosition + cellPosition, scale), 0.0);
}