Shader "CustomRenderTexture/StencilMask"
{
    Properties
    {
        _StencilRef ("Stencil Ref", Int) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry" }

        Pass
        {
            Stencil
            {
                Ref [_StencilRef]  
                Comp Always
                Pass Replace
            }

            ZWrite Off
            //　ColorMask0 - 透明
            ColorMask 0
        }
    }
    
}
