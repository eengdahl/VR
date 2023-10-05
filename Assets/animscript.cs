using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class animscript : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    int counter;
    SpriteRenderer _image;
    private void Start()
    {
        Invoke(nameof(ChangeSprite), 1);
        _image = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void ChangeSprite()
    {
        _image.sprite = sprites[counter];

        counter++;
        if (counter >= sprites.Count)
        {
            counter = 0;
        }

        Invoke(nameof(ChangeSprite), 1);
    }
}
