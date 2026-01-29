using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public static float s_RotationAngle = 45f;

    public bool isEnabled = false;

    [SerializeField] private Text _text;
    [SerializeField] private float _duration = 1.0f;
    
    public void Display(Color color, float value, Vector3 position)
    {
        this.transform.position = position + ((new Vector3(0f, 1f, -1f)).normalized * 10f);
        this.transform.rotation.SetEulerAngles(new Vector3(s_RotationAngle, 0f, 0f));
        _text.color = color;
        _text.text = value.ToString();
        StartCoroutine(LifeTimer(_duration));
    }

    IEnumerator LifeTimer(float duration)
    {
        this.name = "Active indicator";
        isEnabled = true;
        _text.enabled = true;
        yield return new WaitForSeconds(duration);
        _text.enabled = false;
        this.name = "deactivated indicator";
        isEnabled = false;
    }
}
