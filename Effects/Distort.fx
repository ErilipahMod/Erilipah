sampler BackingTex : register(s0);
float2 TexSize;
float2 BackingTexSize;
float4 Frame;

float4 Invert(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 backingCoords = (coords * TexSize - Frame.xy) / BackingTexSize;
    float4 color = tex2D(BackingTex, backingCoords);
    return float4(1 - color.rgb, color.a);
}

technique Default
{
    pass Invert
    {
        PixelShader = compile ps_2_0 Invert();
    }
}