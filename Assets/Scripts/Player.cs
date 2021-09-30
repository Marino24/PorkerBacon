using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public float npcReach = 15f;

    void Start()
    {
        if (introConvo != null)
        {
            Debug.Log("Setting it up");
            uIhandler.conversationStage.SetActive(true); //for initial 
            ConvController.startConvo?.Invoke(introConvo);
            Debug.Log("Setup done");
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
        if (isWalking)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }


        //Check if found a npc available (Hamilton)
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Sending a ray!");
                RaycastHit2D hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    NPC npc = hit.collider.GetComponent<NPC>();
                    if (npc != null && !npc.useItem.isItemInHand && Vector2.Distance(transform.position, npc.transform.position) < npcReach)
                    {
                        npc.StartConvoWithMe();
                    }
                }
            }
        }

    }

    void FixedUpdate()
    {
        float xPos = Input.GetAxis("Horizontal");
        float yPos = Input.GetAxis("Vertical");

        if (xPos != 0 || yPos != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        Vector3 move = new Vector3(xPos, yPos);

        rb.velocity = move * speed;

        if (xPos < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        if (xPos > 0) transform.rotation = Quaternion.Euler(0, 0, 0); ;

    }

    public void DigIt()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isDiggingMud", true);
    }

    public void StopDig()
    {
        anim.SetBool("isDiggingMud", false);
    }

}