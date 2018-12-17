using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseButtonScale : MonoBehaviour {
    //Simple pulsing script, when an animation isnt the right solution

    #region Member Vars

    //Adjustable parameters for pulsing algorithm
    public float m_initial_delay;
    public float m_pulse_duration;
    public int m_pulses_per_cycle;
    public float m_wait_duration;
    public Vector3 m_pulse_scale;
    public bool m_manually_initiated;

    //Private vars
    private Vector3 m_starting_scale;
    private bool m_pulsing_active;

    #endregion

    void Start ()
    {
        m_starting_scale = transform.localScale;

        if (!m_manually_initiated)
        {
            beginPulsingCoroutine();
        }
	}

    public void activatePulsingButton(float initial_delay = -1)
    {
        if (m_manually_initiated)
        { 
            if(initial_delay >= 0)
            {
                m_initial_delay = initial_delay;
            }
            beginPulsingCoroutine();
        }
        else
        {
            Debug.LogError("Pulsing element is marked to start automatically, cannot be manually activated");
        }
    }

    public void deactivatePulsingButton()
    {
        m_pulsing_active = false;
    }

    private void beginPulsingCoroutine()
    {
        m_pulsing_active = true;
        StartCoroutine(pulseContinuously());
    }


    private IEnumerator pulseContinuously()
    {
        transform.localScale = m_starting_scale;
        yield return new WaitForSeconds(m_initial_delay);

        while(m_pulsing_active)
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
