#ifndef VACUUM_CURVEDWORLD_BASE_CGINC
#define VACUUM_CURVEDWORLD_BASE_CGINC 

#include "UnityCG.cginc"
 

/*DO NOT DELETE - CURVED WORLD BEND TYPE*/ #define BENDTYPE_CLASSIC_RUNNER_AXIS_Z_POSITIVE

////////////////////////////////////////////////////////////////////////////
//																		  //
//Variables 															  //
//																		  //
////////////////////////////////////////////////////////////////////////////
uniform float4 _V_CW_PivotPoint_Position;
uniform float4 _V_CW_PivotPoint_2_Position;

uniform float3 _V_CW_BendAxis;
uniform float3 _V_CW_BendOffset;	

uniform float2 _V_CW_Angle;
uniform float2 _V_CW_MinimalRadius;

uniform float _V_CW_Rolloff;
////////////////////////////////////////////////////////////////////////////
//																		  //
//Constants 															  //
//																		  //
////////////////////////////////////////////////////////////////////////////
static const float2 _zero2 = float2(0,0);
static const float3 _zero3 = float3(0,0,0);
static const float2 _one2 = float2(1,1);
static const float3 _one3 = float3(1,1,1);

////////////////////////////////////////////////////////////////////////////
//																		  //
//Defines    															  //
//																		  //
////////////////////////////////////////////////////////////////////////////
#define SIGN(a) (a.x < 0 ? -1.0f : 1.0f)
#define SIGN2(a) (float2(a.x < 0 ? -1.0f : 1.0f, a.y < 0 ? -1.0f : 1.0f))

#define PI     3.14159265359
#define PI_2   6.28318530717
#define PI_0_5 1.57079632679

#define PIVOT   _V_CW_PivotPoint_Position.xyz
#define PIVOT_2 _V_CW_PivotPoint_2_Position.xyz



////////////////////////////////////////////////////////////////////////////
//																		  //
//Vertex Transform														  //
//																		  //
////////////////////////////////////////////////////////////////////////////

inline void V_CW_TransformPoint(inout float4 vertex)
{	
	#if defined(BENDTYPE_CLASSIC_RUNNER_AXIS_X_POSITIVE)
		
		float4 worldPos = mul(unity_ObjectToWorld, vertex);
		worldPos.xyz -= PIVOT;

		float2 xyOff = max(_zero2, worldPos.xx - _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		worldPos = float4(0.0f, _V_CW_BendAxis.x * xyOff.x, -_V_CW_BendAxis.y * xyOff.y, 0.0f) * 0.001;

		vertex += mul(unity_WorldToObject, worldPos);

	#elif defined(BENDTYPE_CLASSIC_RUNNER_AXIS_X_NEGATIVE)

		float4 worldPos = mul(unity_ObjectToWorld, vertex);
		worldPos.xyz -= PIVOT;

		float2 xyOff = min(_zero2, worldPos.xx + _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		worldPos = float4(0.0f, _V_CW_BendAxis.x * xyOff.x, -_V_CW_BendAxis.y * xyOff.y, 0.0f) * 0.001;

		vertex += mul(unity_WorldToObject, worldPos);

	#elif defined(BENDTYPE_CLASSIC_RUNNER_AXIS_Z_NEGATIVE)

		float4 worldPos = mul(unity_ObjectToWorld, vertex);
		worldPos.xyz -= PIVOT;

		float2 xyOff = min(_zero2, worldPos.zz + _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		worldPos = float4(-_V_CW_BendAxis.y * xyOff.y, _V_CW_BendAxis.x * xyOff.x, 0.0f, 0.0f) * 0.001;

		vertex += mul(unity_WorldToObject, worldPos);

	#elif defined(BENDTYPE_CLASSIC_RUNNER_AXIS_Z_POSITIVE)

		float4 worldPos = mul(unity_ObjectToWorld, vertex);
		worldPos.xyz -= PIVOT;

		float2 xyOff = max(_zero2, worldPos.zz - _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		worldPos = float4(-_V_CW_BendAxis.y * xyOff.y, _V_CW_BendAxis.x * xyOff.x, 0.0f, 0.0f) * 0.001;

		vertex += mul(unity_WorldToObject, worldPos);

	#else
		
		//Do nothing

	#endif
} 

inline void V_CW_TransformPointAndNormal(inout float4 vertex, inout float3 normal, float3 worldPos, float3 worldTangent, float3 worldBinormal)
{

	float3 v0 = worldPos;
	#if defined(BENDTYPE_CLASSIC_RUNNER_AXIS_X_POSITIVE) || defined(BENDTYPE_CLASSIC_RUNNER_AXIS_X_NEGATIVE) || defined(BENDTYPE_CLASSIC_RUNNER_AXIS_Z_POSITIVE) || defined(BENDTYPE_CLASSIC_RUNNER_AXIS_Z_NEGATIVE)
		v0 -= PIVOT;	
	#endif

	float3 v1 = v0 + worldTangent;
	float3 v2 = v0 + worldBinormal;
	

	#if defined(BENDTYPE_CLASSIC_RUNNER_AXIS_X_POSITIVE)

		float2 xyOff = max(_zero2, v0.xx - _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		float3 transformedVertex = float3(0.0f, _V_CW_BendAxis.x * xyOff.x, -_V_CW_BendAxis.y * xyOff.y) * 0.001;
		v0 += transformedVertex;			


		xyOff = max(_zero2, v1.xx - _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		v1 += float3(0.0f, _V_CW_BendAxis.x * xyOff.x, -_V_CW_BendAxis.y * xyOff.y) * 0.001;
			

		xyOff = max(_zero2, v2.xx - _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		v2 += float3(0.0f, _V_CW_BendAxis.x * xyOff.x, -_V_CW_BendAxis.y * xyOff.y) * 0.001;


		vertex.xyz += mul((float3x3)unity_WorldToObject, transformedVertex);
		normal = normalize(mul((float3x3)unity_WorldToObject, normalize(cross(v2 - v0, v1 - v0))));

	#elif defined(BENDTYPE_CLASSIC_RUNNER_AXIS_X_NEGATIVE)

		float2 xyOff = min(_zero2, v0.xx + _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		float3 transformedVertex = float3(0.0f, _V_CW_BendAxis.x * xyOff.x, -_V_CW_BendAxis.y * xyOff.y) * 0.001;
		v0 += transformedVertex;
			

		xyOff = min(_zero2, v1.xx + _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		v1 += float3(0.0f, _V_CW_BendAxis.x * xyOff.x, -_V_CW_BendAxis.y * xyOff.y) * 0.001;

			
		xyOff = min(_zero2, v2.xx + _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		v2 += float3(0.0f, _V_CW_BendAxis.x * xyOff.x, -_V_CW_BendAxis.y * xyOff.y) * 0.001;


		vertex.xyz += mul((float3x3)unity_WorldToObject, transformedVertex);
		normal = normalize(mul((float3x3)unity_WorldToObject, normalize(cross(v2 - v0, v1 - v0))));

	#elif defined(BENDTYPE_CLASSIC_RUNNER_AXIS_Z_NEGATIVE)

		float2 xyOff = min(_zero2, v0.zz + _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		float3 transformedVertex = float3(-_V_CW_BendAxis.y * xyOff.y, _V_CW_BendAxis.x * xyOff.x, 0.0f) * 0.001;
		v0 += transformedVertex;
			

		xyOff = min(_zero2, v1.zz + _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		v1 += float3(-_V_CW_BendAxis.y * xyOff.y, _V_CW_BendAxis.x * xyOff.x, 0.0f) * 0.001; 

			
		xyOff = min(_zero2, v2.zz + _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		v2 += float3(-_V_CW_BendAxis.y * xyOff.y, _V_CW_BendAxis.x * xyOff.x, 0.0f) * 0.001; 


		vertex.xyz += mul((float3x3)unity_WorldToObject, transformedVertex);
		normal = normalize(mul((float3x3)unity_WorldToObject, normalize(cross(v2 - v0, v1 - v0))));

	#elif defined(BENDTYPE_CLASSIC_RUNNER_AXIS_Z_POSITIVE)

		float2 xyOff = max(_zero2, v0.zz - _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		float3 transformedVertex = float3(-_V_CW_BendAxis.y * xyOff.y, _V_CW_BendAxis.x * xyOff.x, 0.0f) * 0.001;
		v0 += transformedVertex;
			

		xyOff = max(_zero2, v1.zz - _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		v1 += float3(-_V_CW_BendAxis.y * xyOff.y, _V_CW_BendAxis.x * xyOff.x, 0.0f) * 0.001; 

			
		xyOff = max(_zero2, v2.zz - _V_CW_BendOffset.xy);
		xyOff *= xyOff;
		v2 += float3(-_V_CW_BendAxis.y * xyOff.y, _V_CW_BendAxis.x * xyOff.x, 0.0f) * 0.001; 


		vertex.xyz += mul((float3x3)unity_WorldToObject, transformedVertex);
		normal = normalize(mul((float3x3)unity_WorldToObject, normalize(cross(v2 - v0, v1 - v0))));
		
	#else

		//Do nothing

	#endif
}

inline void V_CW_TransformPointAndNormal(inout float4 vertex, inout float3 normal, float4 tangent)
{	
	float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz; 
	float3 worldNormal = UnityObjectToWorldNormal(normal);
	float3 worldTangent = UnityObjectToWorldDir(tangent.xyz);
	float3 worldBinormal = cross(worldNormal, worldTangent) * -1;// * tangent.w;

	V_CW_TransformPointAndNormal(vertex, normal, worldPos, worldTangent, worldBinormal);
}



//Defines for integration
#define CURVED_WORLD_ENABLED
#define CURVED_WORLD_TRANSFORM_POINT_AND_NORMAL(vertex,normal,tangent) V_CW_TransformPointAndNormal(vertex, normal, tangent);
#define CURVED_WORLD_TRANSFORM_POINT(vertex)                           V_CW_TransformPoint(vertex);

#endif 
