using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera cam;
    private Animator anim;

    public float speed;
    private Rigidbody2D rb;
    private UIhandler uIhandler;
    public ConvController convController;
    public Conversation introConvo;
    public List<string> reachOptions = new List<string>();
    private bool isWalking;

    public static Player instance;

    void Start()
    {
        if (introConvo != null)
        {
            uIhandler.conversationStage.SetActive(true); //for initial 
            ConvController.startConvo?.Invoke(introConvo);

        }
    }
    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        uIhandler = cam.GetComponent<UIhandler>();
        anim = GetComponent<Animator>();
        instance = this;
    }

    void Update()
    {
        uIhandler.monologue.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));
        if(isWalking)
        {
            anim.SetBool("isWalking",true);
        }else{
            anim.SetBool("isWalking",false);
        }

    }

    void FixedUpdate()
    {
        float xPos = Input.GetAxis("Horizontal");
        float yPos = Input.GetAxis("Vertical");

        if(xPos != 0 || yPos != 0)
        {
            isWalking = true;
        }else{
            isWalking = false;
        }

        Vector3 move = new Vector3(xPos, yPos);

        rb.velocity = move * speed;

        if (xPos < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        if (xPos > 0) transform.rotation = Quaternion.Euler(0, 0, 0); ;

    }

    public void DigIt()
    {
        anim.SetBool("isDiggingMud",true);
    }

    public void StopDig()
    {
        anim.SetBool("isDiggingMud",false);
    }
}