Shader "Custom/PortalShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Main Color", Color) = (0.2, 0.5, 1, 1)
        _Emission("Emission Strength", Range(0, 5)) = 1
        _Distortion("Distortion Strength", Range(0, 1)) = 0.2
        _Opacity("Opacity", Range(0, 1)) = 0.5
        _Speed("Rotation Speed", Range(0, 5)) = 1
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha // ���� ����
            ZWrite Off
            Cull Back // ���� �鸸 ������

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 viewDir : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _Color;
                float _Emission;
                float _Distortion;
                float _Opacity;
                float _Speed;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;

                    // View ���� ���
                    o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv - 0.5; // �߾� ����
                    float dist = length(uv) * 2.0; // �Ÿ� ���
                    float angle = atan2(uv.y, uv.x); // �ҿ뵹�� ����

                    // �ҿ뵹�� ȿ��
                    float swirl = sin(angle * 3.0 + _Time.y * _Speed) * _Distortion;

                    // UV �ְ�
                    uv += swirl * uv;
                    float4 texColor = tex2D(_MainTex, uv + 0.5) * _Color;

                    // �߽� �߱� ȿ��
                    float glow = smoothstep(1.0, 0.2, dist) * _Emission;

                    // �ܰ� ���� ���� (�߾ӿ��� �����ڸ��� ��������)
                    float alpha = smoothstep(0.9, 0.3, dist) * _Opacity;

                    return float4(texColor.rgb + glow, alpha);
                }
                ENDCG
            }
        }
}
