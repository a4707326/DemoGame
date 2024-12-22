using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotGame : MonoBehaviour
{

    //private Image _slotIcon1_Img;
    //private Image _slotIcon2_Img;
    //private Image _slotIcon3_Img;

    public GameObject SpinLine01;
    public GameObject SpinLine02;
    public GameObject SpinLine03;
    public Sprite[] SpinLineSprit;
    public Sprite BlankSpinSprit;

    public GameObject[] SpinLineIcon;

    public Sprite[] IconSpin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Spin");

            


        }   
    }
}
