sampler uImage0 : register(s0);
float3 uColor;
float uTime;
float4 uSourceRect;
float2 uImageSize0;

float4 CoronaFx(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{	
	float2 centreCoords = float2(coords.x - 0.5, coords.y - 0.5) * 2;
	float dotField = dot(centreCoords, centreCoords);

	// Intensity will range from 0.1 to 0.25
	// Anything smaller will not read, anything bigger will look bad.
    centreCoords *= dotField * (1.5 + sin(6 * uTime) * (noise(10000 - uTime) + 1) / 2);

	float4 distortSample = tex2D(uImage0, coords - centreCoords);
    float4 colourSample = tex2D(uImage0, coords);
    distortSample.rgb = distortSample.rgb * (1 - colourSample.a) * uColor;
    colourSample.rgb += distortSample.rgb * (1 - dotField);
    
    return float4(colourSample.rgb, 1 - sampleColor.a);
}

float4 Bloat(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 centreCoords = float2(coords.x - 0.5f, coords.y - 0.5f) * 2;
    float dotField = dot(centreCoords, centreCoords);
    centreCoords *= dotField;
    float4 distortSample = tex2D(uImage0, coords - centreCoords);
    return distortSample;
}

technique Technique1
{
    pass CoronaFx
    {
        PixelShader = compile ps_2_0 CoronaFx();
    }
    pass Bloat
    {
        PixelShader = compile ps_2_0 Bloat();
    }
}