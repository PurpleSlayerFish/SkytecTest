using UnityEngine;

namespace PurpleSlayerFish.Core.Services
{
    public class MathUtils
    {
        private float _nextAngle;
        private float _delta;
        
        public Vector2 RandomPerimeterPoint(Vector2 coord0, Vector2 coord1)
        {
            var xEdge = Random.Range(0, 2) > 0;
            var range = xEdge ? new Vector2(coord0.x, coord1.x) : new Vector2(coord0.y, coord1.y);
            var yEdge = Random.Range(0, 2) > 0 ? coord0 : coord1;
            return xEdge ? new Vector2(Random.Range(range.x, range.y), yEdge.y) : new Vector2(yEdge.x, Random.Range(range.x, range.y));
        }

        public bool OverlapRectangle(Vector2 point, Vector2 rectCoord0, Vector2 rectCoord1) =>
            point.x > rectCoord0.x && point.x < rectCoord1.x && point.y > rectCoord0.y && point.y < rectCoord1.y;

        public float Attenuation(float currentDuration, float polarity, float maxValue) =>
            Attenuation(currentDuration, polarity, 0, maxValue);
        public float TwoSideAttenuation(float currentDuration, float polarity, float maxValue) =>
            Attenuation(currentDuration, polarity, -maxValue, maxValue);
        private float Attenuation(float currentDuration, float polarity, float minValue, float maxValue) => 
            Mathf.Clamp(currentDuration + Time.deltaTime * polarity,
                minValue, maxValue);
        
        public float Polarity(bool value) => value ? 1 : -1;
        public int GetRandomPolarity() => Random.Range(0, 2) > 0 ? 1 : -1;

        public Vector2 DirectionFromRotate(float rotation) => DirectionFromRotate(rotation, Vector2.up);
        public Vector2 DirectionFromRotate(float rotation, Vector2 direction) => Quaternion.AngleAxis(rotation, Vector3.forward) * direction;

        public Vector2 Vector2Intersection(Vector2 point0, Vector2 point1) => (point0 - point1) / 2 + point1;
        
        public float RotateAngleUntil(float sourceAngle, float targetAngle, float step)
        {
            _delta = Mathf.DeltaAngle(sourceAngle, targetAngle);
            if (Mathf.Abs(_delta) <= step)
                return targetAngle;
            _nextAngle = sourceAngle + Mathf.Sign(_delta) * step;
            if (_nextAngle > 180)
                _nextAngle -= 360;
            else if (_nextAngle < -180)
                _nextAngle += 360;
            return _nextAngle;
        }
        
        public float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle > 180f)
                angle -= 360f;
            else if (angle < -180f)
                angle += 360f;
            return angle;
        }
        
        public float RandomAngle() => Random.Range(-180f, 180f);
        
        public Vector2 RandomDirection() => DirectionFromRotate(RandomAngle());
    }
}