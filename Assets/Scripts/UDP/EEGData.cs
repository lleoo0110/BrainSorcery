using System;
using UnityEngine;

[Serializable]
public class PredictionData
{
    public int label;
    public float score;
    public string classifier;
}

[Serializable]
public class PowerFeature
{
    public float delta;
    public float theta;
    public float alpha;
    public float beta;
    public float gamma;
}

[Serializable]
public class FAAFeature
{
    public float faa;
    public string arousal;
}

[Serializable]
public class ABRatioFeature
{
    public float ratio;
    public string state;
}

[Serializable]
public class EmotionFeature
{
    public string state;
    public Vector2 coordinates;
    public float[] emotionCoords;
}

[Serializable]
public class FeatureData
{
    public PowerFeature power;
    public FAAFeature faa;
    public ABRatioFeature abRatio;
    public EmotionFeature emotion;
}

[Serializable]
public class EEGData
{
    public PredictionData prediction;
    public FeatureData features;
    public double time;
    public int sample;
}