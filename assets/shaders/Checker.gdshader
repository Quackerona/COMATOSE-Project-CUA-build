shader_type canvas_item;

uniform float cells = 8.0;
uniform float speed;

void fragment() {
	bool cell = mod(floor((UV.x + TIME * speed) * cells) + floor((UV.y - TIME * speed) * cells), 2.0) == 0.0;
	COLOR = vec4(vec3(cell ? 1.0 : 0.0), COLOR.a);
}