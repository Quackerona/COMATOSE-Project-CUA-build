shader_type canvas_item;

float pmap(float x,float a,float b,float c,float d){
  float progress = (x - a) / (b - a);
  return c + (d - c) * progress;
}

float timeMoves(float x, float duration, float part, float g){
  float md = mod(x, duration);
  float start = floor(x/duration) * duration;
  float nm = md/duration;
  float change = clamp(pmap(nm, part, 1., 0., 1.), 0., 1.);
  change = 1. - pow(1. - change, g);
  return start + change * duration;
}

mat2 rot2D(float a){
	float c = cos(a), s = sin(a); return mat2(vec2(c, -s), vec2(s, c));
}

vec3 aces(vec3 x) {
  const float a = 2.51;
  const float b = 0.03;
  const float c = 2.43;
  const float d = 0.59;
  const float e = 0.14;
  return clamp((x * (a * x + b)) / (x * (c * x + d) + e), 0.0, 1.0);
}

float discre(float a) {return floor(mod(mod(a,2.0) + 2.0, 2.0));}

////////////////////////////////////////////////////////////////////

float discre2(float a){
  float modulo = mod(mod(a,2.0) + 2.0, 2.0);
  float type = floor(modulo);
  float ret = type==0. ? 0. : mod(8. * modulo,2.0);
  return ret;
}

float field1(vec2 pos0,float q) {
  vec2 pos = pos0;
  pos *= rot2D(-0.3);
  return q - 2.5*(1.5*abs(pos.y)+1.0*abs(pos.x) + 0.3*sin(1.2*pos.x + 1.3));
}

float field2(vec2 pos,float q) {
  //return q - f*(abs(pos.y)+1.4*abs(pos.x));
  return q - 3.3*length(pos);
  //return q - 4.0*abs(pos.y);
}

float field3(vec2 pos,float q) {
  return q + 7.0*length(pos);
}

float field4(vec2 pos, float q) {
  return q + 13.0*abs(pos.x);
}

float pcol(vec2 uv,float q, float q2) {
    //float col0 = discre(discre(field1(uv,q)) + discre(field2(uv,q)));
    //float col1 = discre(field1(uv,q));
    //float col2 = discre2(field1(uv,q) + field2(uv,q) + 1.3*sin(3.*uv.x + 3.*pulseTime) + 1.5*sin(4.*uv.y + 1.1 + 3.*pulseTime) );
    //float col3 = discre(discre2(field1(uv,q)) + discre2(field2(uv,q)));
    //float col4 = discre(discre(field3(uv,q)) + discre(discre2(field1(uv,q)) + discre2(field2(uv,q))));
    //float col5 = discre(discre(field4(uv,q)) + discre(field3(uv,q)) + discre(discre2(field1(uv,q)) + discre2(field2(uv,q))));
    float wavyOffset = 1.3*sin(3.*uv.x + 3.) + 1.5*sin(4.*uv.y + 1.1 + 3.);
    float col6 = discre(wavyOffset
                        + discre(field4(uv,q))
                        + discre(field3(uv,q))
                        + discre(discre2(field1(uv,q))
                        + discre2(field2(uv,q))));
    return col6;
}

void fragment() {
	vec2 s = UV;
	vec2 uv = (s-0.5)*UV.xx / UV.yx;
	
	float t = TIME;

	float bpm = 900./2.;
	float period =  1.0 / (bpm / 2.);
	float TF = 2.0;
	float phase = 5.05;
	t = TF*TIME;
	uv *= 1.3 + 0.85*sin(0.2*t);
	float pulseTime = TF*timeMoves(TIME - phase, period, 0.2, 1.6);
	pulseTime = mix(t,pulseTime,0.7);

	float pulseTime2 = TF*timeMoves(TIME - phase, 2.*period, 0.2, 1.6);
	pulseTime2 = mix(t,pulseTime2,0.7);

	float dt = 0.035;

	vec3 rgb = vec3(pcol(uv,pulseTime,t),
	                pcol(uv,pulseTime-dt,t-dt),
	                pcol(uv,pulseTime-2.*dt,t-2.*dt));

	float coff = length(uv)*1.0;
	rgb.xy *= rot2D(0.57*t-coff);
	rgb.yz *= rot2D(0.87*t-coff*1.2);
	rgb = abs(rgb);

	float scl = 13.0+4.*sin(0.37*t);
	vec2 ruv = uv;
	ruv *= rot2D(10.05*t);
	rgb = mix(rgb,vec3(2.0)-rgb,discre(ruv.x*scl));
	rgb = mix(rgb,vec3(1.0)-rgb,discre(ruv.y*scl));


	rgb = aces(aces(rgb));

	COLOR = vec4(rgb, 1.0);
}