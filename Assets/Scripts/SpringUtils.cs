using UnityEngine;

public static class SpringUtils
{
    // Luna strips Mathf.Exp from the build; Mathf.Pow compiles to a JS intrinsic and survives.
    private const float EULER_NUMBER = 2.71828183f;

    public class tDampedSpringMotionParams
    {
        public float m_posPosCoef;
        public float m_posVelCoef;
        public float m_velPosCoef;
        public float m_velVelCoef;
    }

    public static void CalcDampedSpringMotionParams(ref tDampedSpringMotionParams pOutParams, float deltaTime, float angularFrequency, float dampingRatio)
    {
        float epsilon = 1e-6f;
        if (angularFrequency < epsilon)
        {
            pOutParams.m_posPosCoef = 1;
            pOutParams.m_posVelCoef = 0;
            pOutParams.m_velPosCoef = 0;
            pOutParams.m_velVelCoef = 1;
            return;
        }

        float d = dampingRatio * angularFrequency;
        float k = angularFrequency * angularFrequency;
        float det = d * d - k;

        if (det > epsilon) // Overdamped
        {
            float r1 = -d + Mathf.Sqrt(det);
            float r2 = -d - Mathf.Sqrt(det);
            float e1 = Mathf.Pow(EULER_NUMBER, r1 * deltaTime);
            float e2 = Mathf.Pow(EULER_NUMBER, r2 * deltaTime);

            float invDenom = 1.0f / (r1 - r2);

            pOutParams.m_posPosCoef = (e1 * r1 - e2 * r2) * invDenom;
            pOutParams.m_posVelCoef = (e1 - e2) * invDenom;
            pOutParams.m_velPosCoef = (e1 * r1 * r1 - e2 * r2 * r2) * invDenom;
            pOutParams.m_velVelCoef = (e1 * r1 - e2 * r2) * invDenom;
        }
        else // Underdamped
        {
            float omegaZeta = angularFrequency * dampingRatio;
            float alpha = angularFrequency * Mathf.Sqrt(1.0f - dampingRatio * dampingRatio);
            float expTerm = Mathf.Pow(EULER_NUMBER, -omegaZeta * deltaTime);
            float cosTerm = Mathf.Cos(alpha * deltaTime);
            float sinTerm = Mathf.Sin(alpha * deltaTime);
            float invAlpha = 1.0f / alpha;

            pOutParams.m_posPosCoef = expTerm * (cosTerm + omegaZeta * invAlpha * sinTerm);
            pOutParams.m_posVelCoef = expTerm * sinTerm * invAlpha;
            pOutParams.m_velPosCoef = -expTerm * sinTerm * alpha;
            pOutParams.m_velVelCoef = expTerm * (cosTerm - omegaZeta * invAlpha * sinTerm);
        }
    }

    public static void UpdateDampedSpringMotion(ref float pPos, ref float pVel, float equilibriumPos, in tDampedSpringMotionParams springParams)
    {
        float oldPos = pPos - equilibriumPos;
        float oldVel = pVel;

        pPos = oldPos * springParams.m_posPosCoef + oldVel * springParams.m_posVelCoef + equilibriumPos;
        pVel = oldPos * springParams.m_velPosCoef + oldVel * springParams.m_velVelCoef;
    }
}
