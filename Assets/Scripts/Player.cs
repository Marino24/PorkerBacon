using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera cam;

    public float speed;
    private Rigidbody2D rb;
    private UIhandler uIhandler;
    public ConvController convController;
    public Conversation introConvo;
    void Start()
    {
        if (introConvo != null)
        {
            uIhandler.StartConversation();

            convController.currentConv = introConvo;
            convController.ConvoStarted();
        }
    }
    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        uIhandler = cam.GetComponent<UIhandler>();

    }

    void Update()
    {
        uIhandler.monologue.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));


    }

    void FixedUpdate()
    {
        float xPos = Input.GetAxis("Horizontal");
        float yPos = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(xPos, yPos);

        rb.velocity = move * speed;

        if (xPos < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        if (xPos > 0) transform.rotation = Quaternion.Euler(0, 0, 0); ;

    }
}