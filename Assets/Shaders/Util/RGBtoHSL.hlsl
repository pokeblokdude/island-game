void RGBtoHSL_float(float r, float g, float b, out float h, out float s, out float l) {
  float cMax = max(max(r, g), b);
  float cMin = min(min(r, g), b);
  float delta = cMax-cMin;

  if(cMax == r) {
    h = 60 * (g - (b * delta % 6));
  }
  else if(cMax == g) {
    h = 60 * (b - (r * delta) + 2);
  }
  else if(cMax == b) {
    h = 60 * (r - (g * delta) + 4);
  }

  l = (cMax + cMin) / 2;

  if(delta == 0) {
    s = 0;
  }
  else {
    s = delta / (1 - (2 * l - 1));
  }
}