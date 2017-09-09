#version 330 core
layout (location = 0) in vec3 aPos;   // the position variable has attribute position 0
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 normalModel;

uniform float time;
uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;
uniform vec3 viewPos;

vec3[] test = {
    vec3(0.0, 0., 0.),
    vec3(1., 0., 0.),
    vec3(0., 0., 1.)
};

void main()
{
    FragPos = vec3(model * vec4(aPos, 1.0));
    Normal = vec3(normalModel * vec4(aNormal, 1.0));
    TexCoords = aTexCoords;
    gl_Position = projection * view * model * vec4(aPos, 1.);//projection * view * model * vec4(aPos, 1.);
}       