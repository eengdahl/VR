//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering;
//using UnityEngine.UI;

//public class DecalPainter : MonoBehaviour
//{
//    [SerializeField]
//    private DecalTextureData[] decalData;

//    [SerializeField]
//    private GameObject decalPorjectorPrefab;

//    [SerializeField]
//    private int selectedDecalIndex;

//    [SerializeField]
//    private Image decalImage;

//    Material[] decalMaterials;

//    //Setting up UI image to selected decal image
//    private void Start()
//    {
//        decalMaterials = new Material[decalData.Length];
//        selectedDecalIndex = 0;
//        foreach (Image image in FindObjectsOfType<Image>())
//        {
//            if (image.CompareTag("Decal"))
//            {
//                decalImage = image;
//                break;
//            }
//        }
//        //decalImage.sprite = decalData[selectedDecalIndex].sprite;
//    }

//    //Swap the decal image (data) when Q is pressed
//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            selectedDecalIndex++;
//            if (selectedDecalIndex >= decalData.Length)
//                selectedDecalIndex = 0;
//            decalImage.sprite = decalData[selectedDecalIndex].sprite;
//        }
//    }

//    //You could get hit.point and hit.normal by shooting a ray in the direction of the wall
//    //Ex:
//    //RaycastHit hit;
//    //if(Physics.Raycast(transform.position, cameraTransform.forward,out hit, 20)){ ...
//    public void PaintDecal(Vector3 point, Vector3 normal, Collider collider)
//    {
//        //Prepare a decal
//        GameObject decal = Instantiate(decalPorjectorPrefab, point, Quaternion.identity);
//        DecalProjector projector = decal.GetComponent<DecalProjector>();
//        if (decalMaterials[selectedDecalIndex] == null)
//        {
//            decalMaterials[selectedDecalIndex] = new Material(projector.material);
//        }
//        projector.material = decalMaterials[selectedDecalIndex];
//        projector.material.SetTexture("Base_Map", decalData[selectedDecalIndex].sprite.texture);
//        projector.size = decalData[selectedDecalIndex].size;
//        decal.transform.forward = -normal;
//    }

//}

///// <summary>
///// Decal data to store sprite and size
///// </summary>
//[Serializable]
//public class DecalTextureData
//{
//    public Sprite sprite;
//    public Vector3 size;
//}