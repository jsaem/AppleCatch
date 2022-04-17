using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public AudioClip appleSE;
    public AudioClip bombSE;
    AudioSource aud;
    GameObject director;

    LayerMask m_StageMask = -1;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        this.director = GameObject.Find("GameDirector");
        this.aud = GetComponent<AudioSource>();

        m_StageMask = 1 << LayerMask.NameToLayer("stage");
        // stage번 레이어만 피킹
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_StageMask))
            {
                float x = Mathf.RoundToInt(hit.point.x);
                float z = Mathf.RoundToInt(hit.point.z);
                transform.position = new Vector3(x, 0.0f, z);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main;

        if (other.gameObject.tag == "Apple")
        {
            this.director.GetComponent<GameDirector>().GetApple();
            this.aud.PlayOneShot(this.appleSE);
            settings.startColor = new ParticleSystem.MinMaxGradient(new Color(255,255,255));
            GetComponent<ParticleSystem>().Play();
        }
        else
        {
            this.director.GetComponent<GameDirector>().GetBomb();
            this.aud.PlayOneShot(this.bombSE);
            settings.startColor = new ParticleSystem.MinMaxGradient(new Color(0, 0, 0));
            GetComponent<ParticleSystem>().Play();
        }
        Destroy(other.gameObject);
    }


}
