using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicScript : MonoBehaviour
{

    public List<AudioClip> backgroundNoise;
    public List<AudioClip> backGroundMusic;
    private int count, count1;

    private AudioSource aS;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        aS = GetComponent<AudioSource>();

        StartCoroutine(StartBackground());

        StartCoroutine(StartMusic());
    }
    IEnumerator StartBackground()
    {
        aS.PlayOneShot(backgroundNoise[count1], 0.1f);

        yield return new WaitForSeconds(backgroundNoise[count1].length);
        count1++;
        if (count1 >= backgroundNoise.Count) count1 = 0;
        StartCoroutine(StartBackground());
    }

    IEnumerator StartMusic()
    {

        aS.PlayOneShot(backGroundMusic[count], 0.2f);
        yield return new WaitForSeconds(backGroundMusic[count].length);
        count++;
        if (count >= backGroundMusic.Count) count = 0;

        StartCoroutine(StartMusic());
    }
}
