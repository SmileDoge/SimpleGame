#version 330

out vec4 outputColor;

uniform vec3 objectColor;

void main()
{
    outputColor = vec4(objectColor, 1.0);
}


