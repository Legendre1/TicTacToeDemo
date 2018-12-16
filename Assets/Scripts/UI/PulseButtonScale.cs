using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseButtonScale : MonoBehaviour {

    public float m_initial_delay;

    public float m_pulse_duration;
    public int m_pulses_per_cycle;

    public float m_wait_duration;

    public Vector3 m_pulse_scale;

    private Vector3 m_starting_scale;

	// Use this for initialization
	void Start () {
        m_starting_scale = transform.localScale;
        StartCoroutine(pulseContinuously());
	}
	
	private IEnumerator pulseContinuously()
    {
        yield return new WaitForSeconds(m_initial_delay);

        while(true)
        {
            for(int n = 0; n < m_pulses_per_cycle; n++)
            {
                yield return StartCoroutine(pulse());
            }

            yield return new WaitForSeconds(m_wait_duration);
        }
    }

    private IEnumerator pulse()
    {
        float pulse_lifetime = 0.0f;
        float pulse_half_duration = m_pulse_duration / 2;

        while(pulse_lifetime < pulse_half_duration)
        {
            float param = pulse_lifetime / pulse_half_duration;
            Vector3 lerped_scale = Vector3.Lerp(m_starting_scale, m_pulse_scale, param);
            transform.localScale = lerped_scale;

            pulse_lifetime += Time.deltaTime;
            yield return 0;
        }

        while (pulse_lifetime < m_pulse_duration)
        {
            float param = (pulse_lifetime - pulse_half_duration) / pulse_half_duration;
            Vector3 lerped_scale = Vector3.Lerp(m_pulse_scale, m_starting_scale, param);
            transform.localScale = lerped_scale;

            pulse_lifetime += Time.deltaTime;
            yield return 0;
        }
    }
}
