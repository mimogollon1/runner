using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{           
    private static int sizeLane = 3;
    public static float gravity = 25f;

    public float laneSpeed = 20f;    
    public int currentLane = 0;    
        
    private float timeChangeDirection = 0;
    private float timeSlide = 0;
    public bool isChangeLane;    
    public bool isJump;
    public bool isSlide;    
    public Vector3 verticalTargetPosition;

    //public float slideLength;
    //public int maxLife = 3;
    //public float minSpeed = 10f;
    //public float maxSpeed = 30f;
    //public float invincibleTime;
    //public GameObject model;

    //private float isJumpStart;
    //private bool sliding = false;
    //private float slideStart;

    //private bool isSwipping = false;
    //private Vector2 startingTouch;
    //private int currentLife;
    //private bool invincible = false;
    //static int blinkingValue;
    //private UIManager uiManager;
    //[HideInInspector]
    //public int coins;
    //[HideInInspector]
    //public float score;
    //private bool canMove;

    #region Propiedades y Atributos
        
    private CharacterController controller;
    private Animator animator;       
    
    #endregion

    #region Métodos Unity3D

    void Start()
    {
        //canMove = false;        
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        
        //currentLife = maxLife;

        //blinkingValue = Shader.PropertyToID("_BlinkingValue");
        //uiManager = FindObjectOfType<UIManager>();
        //GameManager.gm.StartMissions();

        //Invoke("StartRun", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!canMove)
        //	return;

        //score += Time.deltaTime * speed;
        //uiManager.UpdateScore((int)score);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!isChangeLane)
                ChangeLane(-1);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!isChangeLane)
                ChangeLane(1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isJump)
                Jump();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!isSlide)
                Slide();
        }               
        
        #region antiguo 
        //if(Input.touchCount == 1)
        //{
        //	if (isSwipping)
        //	{
        //		Vector2 diff = Input.GetTouch(0).position - startingTouch;
        //		diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);
        //		if(diff.magnitude > 0.01f)
        //		{
        //			if(Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
        //			{
        //				if(diff.y < 0)
        //				{
        //					Slide();
        //				}
        //				else
        //				{
        //					Jump();
        //				}
        //			}
        //			else
        //			{
        //				if(diff.x < 0)
        //				{
        //					ChangeLane(-1);
        //				}
        //				else
        //				{
        //					ChangeLane(1);
        //				}
        //			}

        //			isSwipping = false;
        //		}
        //	}

        //	if (Input.GetTouch(0).phase == TouchPhase.Began)
        //	{
        //		startingTouch = Input.GetTouch(0).position;
        //		isSwipping = true;
        //	}

        //	else if (Input.GetTouch(0).phase == TouchPhase.Ended)
        //	{
        //		isSwipping = false;
        //	}
        //}        		
        #endregion

        if (isChangeLane)
        {
            timeChangeDirection += Time.deltaTime;

            if (timeChangeDirection > 0.40f)
            {
                timeChangeDirection = 0;
                isChangeLane = false;

                animator.SetInteger("Direction", 0);
            }            
        }

        if (isJump)
        {
            verticalTargetPosition.y = verticalTargetPosition.y - (gravity * Time.deltaTime);
            controller.Move(new Vector3(transform.position.x, verticalTargetPosition.y, transform.position.z) * Time.deltaTime);

            if (animator.GetInteger("Jump") != 1){
                // Animación inicio de salto
                animator.SetInteger("Jump", 1);
            }

            if (controller.isGrounded)
            {
                animator.SetInteger("Jump", 2);
                isJump = false;
            }            
        }

        if (isSlide)
        {
            timeSlide += Time.deltaTime;

            if (timeSlide > 0.40f)
            {
                controller.height = 2;
                controller.center = new Vector3(0, controller.center.y * 2, 0);

                animator.SetBool("Slide", false);

                timeSlide = 0;
                isSlide = false;
            }
        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

    #endregion

    #region Métodos privados

    /// <summary>
    /// Asigna y limita la dirección horizontal hacia donde se desplaza el player
    /// </summary>
    /// <param name="direction">dirección horizontal hacia donde se desplaza el player.</param>
    private void ChangeLane(int direction)
    {
        int targetLine = currentLane + (direction * sizeLane);
        
        if (targetLine < -3 || targetLine > 3)
            return;
        
        animator.SetInteger("Direction", direction);

        currentLane = targetLine;
        verticalTargetPosition.x = currentLane;

        isChangeLane = true;
    }
    
    /// <summary>
    /// Ejecuta salto del player
    /// </summary>    
    private void Jump()
    {        
        verticalTargetPosition.y = 12f;
        isJump = true;      
    }

    /// <summary>
    /// Ejecuta deslizamiento del player
    /// </summary>
    private void Slide()
    {
        controller.height = 1;
        controller.center = new Vector3(0, controller.center.y / 2, 0);

        animator.SetBool("Slide", true);
        isSlide = true;
    }

    //void Slide()
    //{
    //    if (!isJump && !sliding)
    //    {
    //        slideStart = transform.position.z;
    //        anim.SetFloat("JumpSpeed", speed / slideLength);
    //        anim.SetBool("Sliding", true);
    //        Vector3 newSize = boxCollider.size;
    //        newSize.y = newSize.y / 2;
    //        boxCollider.size = newSize;
    //        sliding = true;

    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.CompareTag("Coin"))
    //    {
    //        PlayServices.IncrementAchievment(EndlessRunnerServices.achievement_colete_100_peixes, 1);
    //        coins++;
    //        uiManager.UpdateCoins(coins);
    //        other.transform.parent.gameObject.SetActive(false);
    //    }

    //    if (invincible)
    //        return;

    //    if (other.CompareTag("Obstacle"))
    //    {
    //        canMove = false;
    //        currentLife--;
    //        uiManager.UpdateLives(currentLife);
    //        anim.SetTrigger("Hit");
    //        speed = 0;
    //        if (currentLife <= 0)
    //        {
    //            speed = 0;
    //            anim.SetBool("Dead", true);
    //            uiManager.gameOverPanel.SetActive(true);

    //            //if(score > PlayServices.GetPlayerScore(EndlessRunnerServices.leaderboard_ranking))
    //            //{
    //            //PlayServices.PostScore((long)score, EndlessRunnerServices.leaderboard_ranking);
    //            //}

    //            Invoke("CallMenu", 2f);
    //        }
    //        else
    //        {
    //            Invoke("CanMove", 0.75f);
    //            StartCoroutine(Blinking(invincibleTime));
    //        }
    //    }
    //}

    //void CanMove()
    //{
    //    canMove = true;
    //}

    //IEnumerator Blinking(float time)
    //{
    //    invincible = true;
    //    float timer = 0;
    //    float currentBlink = 1f;
    //    float lastBlink = 0;
    //    float blinkPeriod = 0.1f;
    //    bool enabled = false;
    //    yield return new WaitForSeconds(1f);
    //    speed = minSpeed;
    //    while (timer < time && invincible)
    //    {
    //        model.SetActive(enabled);
    //        //Shader.SetGlobalFloat(blinkingValue, currentBlink);
    //        yield return null;
    //        timer += Time.deltaTime;
    //        lastBlink += Time.deltaTime;
    //        if (blinkPeriod < lastBlink)
    //        {
    //            lastBlink = 0;
    //            currentBlink = 1f - currentBlink;
    //            enabled = !enabled;
    //        }
    //    }
    //    model.SetActive(true);
    //    //Shader.SetGlobalFloat(blinkingValue, 0);
    //    invincible = false;
    //}

    //void CallMenu()
    //{
    //    GameManager.gm.coins += coins;
    //    GameManager.gm.EndRun();
    //}

    //public void IncreaseSpeed()
    //{
    //    speed *= 1.15f;
    //    if (speed >= maxSpeed)
    //        speed = maxSpeed;
    //}

    #endregion

}
