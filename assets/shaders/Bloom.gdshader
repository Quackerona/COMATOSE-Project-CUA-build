shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

const float blurSize = 1.0/512.0;
    
uniform float intensity;

void fragment() {
	vec4 sum = vec4(0);
	vec2 texcoord = FRAGCOORD.xy / (1.0 / SCREEN_PIXEL_SIZE);
	int j;
	int i;

	//thank you! http://www.gamerendering.com/2008/10/11/gaussian-blur-filter-shader/ for the 
	//blur tutorial
	// blur in y (vertical)
	// take nine samples, with the distance blurSize between them
	sum += texture(screen_texture, vec2(texcoord.x - 4.0*blurSize, texcoord.y)) * 0.05;
	sum += texture(screen_texture, vec2(texcoord.x - 3.0*blurSize, texcoord.y)) * 0.09;
	sum += texture(screen_texture, vec2(texcoord.x - 2.0*blurSize, texcoord.y)) * 0.12;
	sum += texture(screen_texture, vec2(texcoord.x - blurSize, texcoord.y)) * 0.15;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y)) * 0.16;
	sum += texture(screen_texture, vec2(texcoord.x + blurSize, texcoord.y)) * 0.15;
	sum += texture(screen_texture, vec2(texcoord.x + 2.0*blurSize, texcoord.y)) * 0.12;
	sum += texture(screen_texture, vec2(texcoord.x + 3.0*blurSize, texcoord.y)) * 0.09;
	sum += texture(screen_texture, vec2(texcoord.x + 4.0*blurSize, texcoord.y)) * 0.05;
	    
	    // blur in y (vertical)
	// take nine samples, with the distance blurSize between them
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y - 4.0*blurSize)) * 0.05;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y - 3.0*blurSize)) * 0.09;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y - 2.0*blurSize)) * 0.12;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y - blurSize)) * 0.15;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y)) * 0.16;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y + blurSize)) * 0.15;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y + 2.0*blurSize)) * 0.12;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y + 3.0*blurSize)) * 0.09;
	sum += texture(screen_texture, vec2(texcoord.x, texcoord.y + 4.0*blurSize)) * 0.05;

	COLOR = sum * intensity + texture(screen_texture, texcoord);
}