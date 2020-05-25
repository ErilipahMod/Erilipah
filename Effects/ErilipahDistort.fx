sampler uImage0 : register(s0); // The contents of the screen.
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition; // Camera position
float2 uTargetPosition; // Player position
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uTint; // Custom. Defines the amount the color is darkened
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uDesaturation; // Custom. Defines the amount the color is desaturated
float4 uSourceRect;
float2 uZoom;

float3 Grayscale(float3 color)
{
    float gray = dot(color.rgb, float3(0.299, 0.587, 0.114));
    return lerp(color, gray, uOpacity * uDesaturation);
}

float3 Tint(float3 color)
{
    return lerp(color, uColor, uOpacity * uTint);
}

float4 Distort(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 targetPixelCoords = uTargetPosition - uScreenPosition;
    
    float2 targetCoords = targetPixelCoords / uScreenResolution;
    
    // Get the center of the texture (texture = screen, in this case)
    float2 centerCoords = (coords - targetCoords);
    
    // A dot product of a vector with itself generates a nice circular gradient around the vector as a point. Dot normalizes that to 0-1 I think
    float dotField = dot(centerCoords, centerCoords);
    
    // Get the color of the current pixel
    float4 trueColor = tex2D(uImage0, coords);
    
    // Desaturate it, then darken it
    float3 grayColor = Tint(Grayscale(trueColor.rgb));
    
    // Dampen the color according to the dotField
    float3 darkenedColor = grayColor * max(1.0 - dotField, 0.15); // 0.15 = minimum brightness
    
    float zoomAverage = (uZoom.x + uZoom.y) / 2;
    zoomAverage *= zoomAverage;
    
    // Lerp the color according to opacity & intensity
    float3 retColor = lerp(grayColor, darkenedColor, uOpacity * uIntensity / zoomAverage);
    
    return float4(retColor, trueColor.a);
}

technique Technique1
{
    pass Distort
    {
        PixelShader = compile ps_2_0 Distort();
    }
}