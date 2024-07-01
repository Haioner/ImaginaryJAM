using UnityEngine;

public class EquipmentHeadbob : MonoBehaviour
{
    [Header("Headbob Settings")]
    [SerializeField] private Transform pivotPos;
    [SerializeField] private float amount = 1;
    [SerializeField] private float frequency = 2;
    [SerializeField] private float scale = 150;
    [SerializeField] private float lerpSpeed = 14;
    private float _time;
    private Vector3 _swayPosition;

    private void FixedUpdate() => CalculateHeadbob();

    private void CalculateHeadbob()
    {
        var targetPosition = LissajousCurve(_time, amount, frequency) / scale;
        _swayPosition = Vector3.Lerp(_swayPosition, targetPosition, Time.smoothDeltaTime * lerpSpeed);

        _time += Time.deltaTime;
        if (_time > 6.3f)
            _time = 0;

        pivotPos.localPosition = _swayPosition;
    }

    private Vector3 LissajousCurve(float Time, float amount, float frequency)
    {
        return new Vector3(Mathf.Sin(Time), amount * Mathf.Sin(frequency * Time + Mathf.PI));
    }
}
