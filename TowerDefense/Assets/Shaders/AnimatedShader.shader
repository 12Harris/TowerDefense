Shader "UVScroll" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ScrollX ("Scroll X", Range(-5,5)) = 1
		_ScrollY ("Scroll Y", Range(-5,5)) = 1
	}
	SubShader {

		
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade
		float _ScrollX;
		float _ScrollY;

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		    _ScrollX *= -_Time;
		    _ScrollY *= _Time;
		    //float2 newuv = IN.uv_MainTex + float2(_ScrollX,_ScrollY);
            float2 newuv = IN.uv_MainTex + float2(_ScrollX,1);
            fixed4 c = tex2D (_MainTex, newuv);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
			//o.Albedo = tex2D (_MainTex, newuv);
            //o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
            
            //o.Albedo = tex2D (_MainTex, IN.uv_MainTex);

		}
		ENDCG
	}
	FallBack "Diffuse"
}