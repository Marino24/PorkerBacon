using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Animator anim;
    public float speed;
    private Rigidbody2D rb;
    private UIhandler uIhandler;
    private ConvController convController;
    public Conversation introConvo;
    private bool isWalking;
    public static Player instance;
    public float npcReach = 15f;
    public Vector3 hooklinePos; private int hooklineSetupState = 0;
    public bool level1Over;
    private TextWritter textWritter;

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
        textWritter = Camera.main.GetComponent<TextWritter>();
        uIhandler = Camera.main.GetComponent<UIhandler>();
        convController = Camera.main.GetComponent<ConvController>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        instance = this;
    }

    void Update()
    {
        uIhandler.monologue.transform.position = Camera.main.WorldToScreenPoint(transform.position + uIhandler.monologueBGOffset);
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
            if (!EventSystem.current.IsPointerOverGameObject() && !Inventory.instance.importantMessage)
            {
                Debug.Log("Sending a ray!");
                RaycastHit2D hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    NPC npc = hit.collider.GetComponent<NPC>();
                    if (npc != null && !npc.useItem.isItemInHand)
                    {
                        if (Vector2.Distance(transform.position, npc.transform.position) < npcReach)
                        {
                            npc.StartConvoWithMe();
                        }
                        else
                        {
                            textWritter.Write(npc.outOfReachDialogue, uIhandler.monologueText, false, true, false);
                        }
                    }
                }
            }
        }

    }

    public void InConversation()
    {
        rb.velocity = Vector3.zero;
        isWalking = false;
        anim.SetBool("isWalking", false);
        enabled = false;
    }

    void FixedUpdate()
    {
        //normal movement
        if (hooklineSetupState == 0)
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

            Vector3 move = new Vector2(xPos, yPos);

            rb.velocity = move * speed;

            if (xPos < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            if (xPos > 0) transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        if (hooklineSetupState == 1)
        {
            isWalking = true;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0.5f, -9, -5), step);

            if (transform.position.x < 0.5f) transform.rotation = Quaternion.Euler(0, 0, 0);
            if (transform.position.x > 0.5f) transform.rotation = Quaternion.Euler(0, 180, 0);

            if (Vector2.Distance(transform.position, new Vector3(0.5f, -9, -5)) < 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                hooklineSetupState = 2;
            }
        }
        if (hooklineSetupState == 2)
        {
            isWalking = true;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, hooklinePos, step);

            if (Vector2.Distance(transform.position, hooklinePos) < 0.15f)
            {
                isWalking = false;
                hooklineSetupState = 3;
                UseHookline();
            }
        }
    }

    //for once we refactor, something like this replaces above
    public IEnumerator SetUpPosition(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length;)
        {
            isWalking = true;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, positions[i], step);

            if (transform.position.x < positions[i].x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);

            if (Vector2.Distance(transform.position, positions[i]) < 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); //right
                i++;
            }
            yield return new WaitForFixedUpdate();
        }
        isWalking = false;
        //do X
    }
    public void DigIt()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isDiggingMud", true);
    }

    public void UseHookline()
    {
        if (hooklineSetupState != 0)
        {
            anim.SetBool("isUsingHookline", true);
            anim.SetBool("isWalking", false);

            hooklineSetupState = 0;
        }
        else hooklineSetupState = 1;
    }

    public void OpenGATEAfterHookline()
    {
        uIhandler.OpenTheGate();
    }

    public void StopDig()
    {
        anim.SetBool("isDiggingMud", false);
    }
    public void StopHookline()
    {
        anim.SetBool("isUsingHookline", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Day1End") && !level1Over)
        {
            //End day1, show credits
            level1Over = true;
            LvlLoader.instance.LoadScene(2);
            Debug.Log("CONGRATS YOU FREE!?");
        }
    }

}