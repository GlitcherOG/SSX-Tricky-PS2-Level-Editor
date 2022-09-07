using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSX_Modder.FileHandlers.MapEditor;

public class SplineSegmentObject : MonoBehaviour
{
    public Vector3 ProcessedPoint1;
    public Vector3 ProcessedPoint2;
    public Vector3 ProcessedPoint3;
    public Vector3 ProcessedPoint4;
    [Space(10)]
    public Vector3 Point1;
    public Vector3 Point2;
    public Vector3 Point3;
    public Vector3 Point4;
    [Space(10)]
    public Vector4 ScalingPoint;
    public Vector3 NormalizedScalingPoint;
    [Space(10)]
    public int PreviousSegment;
    public int NextSegment; //Model ID Or Object Parent?
    public int SplineParent;
    [Space(10)]
    public Vector3 LowestXYZ;
    public Vector3 HighestXYZ;
    [Space(10)]
    public float SegmentDisatnce;
    public float PreviousSegmentsDistance;
    public int Unknown32;
    
    public LineRenderer lineRenderer;

    private int curveCount = 0;
    private int SEGMENT_COUNT = 50;


    void Start()
    {
        
    }

    void Update()
    {

    }

    public void LoadSplineSegment(SplinesSegments segments)
    {
        ProcessedPoint1 = ConversionTools.Vertex3ToVector3(segments.ControlPoint);
        ProcessedPoint2 = ConversionTools.Vertex3ToVector3(segments.Point2);
        ProcessedPoint3 = ConversionTools.Vertex3ToVector3(segments.Point3);
        ProcessedPoint4 = ConversionTools.Vertex3ToVector3(segments.Point4);

        ScalingPoint = new Vector4(segments.ScalingPoint.X, segments.ScalingPoint.Y, segments.ScalingPoint.Z,segments.ScalingPoint.W);
        NormalizedScalingPoint = ScalingPoint/ScalingPoint.w;

        PreviousSegment = segments.PreviousSegment;
        NextSegment = segments.NextSegment;
        SplineParent = segments.SplineParent;

        LowestXYZ = ConversionTools.Vertex3ToVector3(segments.LowestXYZ);
        HighestXYZ = ConversionTools.Vertex3ToVector3(segments.HighestXYZ);

        SegmentDisatnce = segments.SegmentDisatnce;
        PreviousSegmentsDistance = segments.PreviousSegmentsDistance;
        Unknown32 = segments.Unknown32;

        GeneratePoints();
        SetDataLineRender();
        transform.position = ProcessedPoint1 * TrickyMapInterface.Scale;
        DrawCurve();

    }

    public SplinesSegments GenerateSplineSegment()
    {

    }

    void GeneratePoints()
    {
        Point1 = ProcessedPoint1;
        Point2 = Point1 + ProcessedPoint2 / 3;
        Point3 = Point2 + (ProcessedPoint2 + ProcessedPoint3) / 3;
        Point4 = ProcessedPoint1 + ProcessedPoint2 + ProcessedPoint3 + ProcessedPoint4;
    }

    void SetDataLineRender()
    {
        lineRenderer.positionCount = 4;
        lineRenderer.SetPosition(0, (Point1 - Point1) *TrickyMapInterface.Scale);
        lineRenderer.SetPosition(1, (Point2 - Point1) *TrickyMapInterface.Scale);
        lineRenderer.SetPosition(2, (Point3 - Point1)* TrickyMapInterface.Scale);
        lineRenderer.SetPosition(3, (Point4 - Point1) * TrickyMapInterface.Scale);
    }

    void DrawCurve()
    {
        curveCount = (int)4 / 3;
        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                Vector3 pixel = CalculateCubicBezierPoint(t, (Point1 - Point1) * TrickyMapInterface.Scale, (Point2 - Point1) * TrickyMapInterface.Scale, (Point3 - Point1) * TrickyMapInterface.Scale, (Point4 - Point1) * TrickyMapInterface.Scale);
                lineRenderer.positionCount = ((j * SEGMENT_COUNT) + i);
                lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);
            }

        }
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}