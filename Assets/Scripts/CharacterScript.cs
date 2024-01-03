using System.Collections;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    float horizontal;
    float jump;
    Vector2 lookDirection = new Vector2(1, 0);
    Animator animator;

    public float timeInvincible = 1.5f;
    bool isInvincible = false;
    float invincibleTimer;

    [SerializeField] public float speed;
    [SerializeField] public float jumpHeight = 5.0f;
    bool isGrounded;

    [SerializeField] float attackIntervalTime = 1f;
    float attackTimer;

    [Header("Stats")]
    readonly int ATK = KaguraBachiData.Atk;

    int attackDamage = 1;

    [Header("Attack VFXs")]
    [SerializeField] GameObject normalAttack;
    [SerializeField] GameObject specialAttack1;
    [SerializeField] GameObject specialAttack2;
    ParticleSystem restoreHPVFX;

    [SerializeField] float restoreHPTime = 2f;
    float restoreHPStartTime = 0f;

    [SerializeField] bool testingEnvironment = true;

    // Character size
    float width;
    float height;

    AudioSource[] audioSource;

    public AudioClip attack;
    public AudioClip specialAttackOne;
    public AudioClip specialAttackTwo;

    public AudioClip getHit;

    public AudioClip restore;

    public AudioClip Jump;

    public GameObject backgroundMusic;
    AudioSource bgm;

    public AudioClip bossFightBGM;
    bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isGrounded = true;
        width = GetComponent<Renderer>().bounds.size.x;
        height = GetComponent<Renderer>().bounds.size.y;
        restoreHPVFX = transform.Find("RestoreHPVFX").GetComponent<ParticleSystem>();
        audioSource = GetComponents<AudioSource>();
        bgm = backgroundMusic.GetComponent<AudioSource>();
        Healthbar.instance.setFillAmount(KaguraBachiData.Health - KaguraBachiData.MaxHealth);
        Manabar.instance.setFillAmount(KaguraBachiData.Ki);
        EssenceCollected.instance.setNormalEssence(KaguraBachiData.ParasiteEssence, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        animator.SetFloat("yVelocity", rb2d.velocity.y);
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");

        Vector2 move = new Vector2(horizontal, rb2d.velocity.y);
        if (Mathf.Abs(move.x) > 0.05f)
        {
            lookDirection.Set(move.x, 0);
            lookDirection.Normalize();
            animator.SetFloat("xVelocity", move.x);
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
            PlaySound(Jump);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (attackTimer <= 0)
            {
                Attack1();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (attackTimer <= 0)
            {
                Attack2();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (attackTimer <= 0)
            {
                Attack3();
            }
        }
        // Testing purpose
        if (Input.GetKey(KeyCode.A))
        {
            if (KaguraBachiData.Health == KaguraBachiData.MaxHealth || KaguraBachiData.Ki < (int)KaguraBachiData.KiConsumption.RestoreHP)
            {
                return;
            }
            if (restoreHPStartTime == 0)
            {
                restoreHPVFX.Play();
                PlaySoundHeal(restore);
            }
            restoreHPStartTime += Time.deltaTime;
            if (restoreHPStartTime >= restoreHPTime)
            {
                changeHealth(1);
                ChangeKi(-(int)KaguraBachiData.KiConsumption.RestoreHP);
                restoreHPStartTime = 0f;
            }
        }
        else
        {
            restoreHPVFX.Stop();
            audioSource[1].Stop();
            restoreHPStartTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (testingEnvironment)
            {
                changeHealth(-1);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (testingEnvironment)
            {
                ChangeKi(100);
            }
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PureHeartEssenceNotification.instance.openNotification();
            BackgroundMusic.instance.changeBossBGM();
            EssenceCollected.instance.setNormalEssence(KaguraBachiData.ParasiteEssence, 300);
            KaguraBachiData.ParasiteEssence += 300;
            KaguraBachiData.PureParasiteHeart += 1;

        }
    }

    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(speed * horizontal, rb2d.velocity.y);
        Bounds boxbounds = gameObject.GetComponent<CapsuleCollider2D>().bounds;
        RaycastHit2D hit = Physics2D.Raycast(boxbounds.center, Vector2.down, boxbounds.extents.y + 1.1f, LayerMask.GetMask("Map"));
        if (hit.collider != null)
        {
            animator.SetBool("Jump", false);
            isGrounded = true;
        }
        else
        {
            animator.SetBool("Jump", true);
            isGrounded = false;
        }
    }

    void Attack1()
    {
        attackDamage = (int)(ATK * KaguraBachiData.AttackMultiplier.Normal);
        animator.SetTrigger("Attack1");
        PlaySound(attack);
        attackTimer = attackIntervalTime;
    }

    void NormalAttackVFX(float direction)
    {
        var skillOffset = new Vector2(width * direction, height * 0.75f) * transform.localScale / 1.5f;
        GameObject vfx = Instantiate(normalAttack, rb2d.position + skillOffset, normalAttack.transform.rotation);
        vfx.GetComponent<NormalAttack>().SetDirection(direction);
    }

    void Attack2()
    {
        if (KaguraBachiData.Ki < (int)KaguraBachiData.KiConsumption.SpecialAttack1)
        {
            return;
        }
        ChangeKi(-(int)KaguraBachiData.KiConsumption.SpecialAttack1);
        attackDamage = (int)(ATK * KaguraBachiData.AttackMultiplier.Special1);
        animator.SetTrigger("Attack2");
        PlaySound(specialAttackOne);
        attackTimer = attackIntervalTime;
    }

    void SpecialAttack1VFX(float direction)
    {
        var skillOffset = new Vector2(width * direction, 0) * transform.localScale / 1.5f;
        GameObject vfx = Instantiate(specialAttack1, rb2d.position + skillOffset, Quaternion.identity);
        vfx.GetComponent<SpecialAttack1>().SetDirection(direction);
    }

    void Attack3()
    {
        if (KaguraBachiData.Ki < (int)KaguraBachiData.KiConsumption.SpecialAttack2)
        {
            return;
        }
        ChangeKi(-(int)KaguraBachiData.KiConsumption.SpecialAttack2);
        attackDamage = (int)(ATK * KaguraBachiData.AttackMultiplier.Special2);
        animator.SetTrigger("Attack3");
        PlaySound(specialAttackTwo);
        attackTimer = attackIntervalTime;
    }

    void SpecialAttack2VFX(float direction)
    {
        var skillOffset = new Vector2(width * direction, 1f + height / 2) * transform.localScale / 1.5f;
        GameObject vfx = Instantiate(specialAttack2, rb2d.position + skillOffset, Quaternion.identity);
        vfx.GetComponent<SpecialAttack2>().SetDirection(direction);
    }

    void Dead()
    {
        animator.SetBool("Dead", true);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(3f);
        isDead = false;
        GameOverScreen.Instance.OpenScreen();
        BossHealthbar.instance.SetEnable(false);
        KaguraBachiData.Health = KaguraBachiData.MaxHealth;
        KaguraBachiData.Ki = 0;
        Healthbar.instance.setFillAmount(3);
        Manabar.instance.setFillAmount(0);
        animator.SetBool("Dead", false);
        // respawn;
        if (Save_Point.savePointData.respawnPoint.map == ""){
           Debug.Log("hello");
            // go to instructtion;
            if (gameObject.scene.name == "VillageScene") {
                var temp = GameObject.Find("InGameUI_group");
                Navigator navigator = temp.transform.Find("navigator").GetComponent<Navigator>();
                StartCoroutine(navigator.Teleport("VillageScene", new Vector3(-336,-102,0)));
                BackgroundMusic.instance.Stop();
            }
            else if (gameObject.scene.name == "ForestScene") {
                var temp = GameObject.Find("InGameUI_group");
                Navigator navigator = temp.transform.Find("navigator").GetComponent<Navigator>();
                StartCoroutine(navigator.Teleport("ForestScene", new Vector3(-34,-8.6f,0)));
                BackgroundMusic.instance.Stop();
            }
            else if (gameObject.scene.name == "CavernScene") {
                var temp = GameObject.Find("InGameUI_group");
                Navigator navigator = temp.transform.Find("navigator").GetComponent<Navigator>();
                StartCoroutine(navigator.Teleport("CavernScene", new Vector3(-46,-6.8f,0)));
                BackgroundMusic.instance.Stop();
            }
        else{
            Debug.Log(Save_Point.savePointData.respawnPoint.location);
            var temp = GameObject.Find("InGameUI_group");
            Navigator navigator = temp.transform.Find("navigator").GetComponent<Navigator>();
            StartCoroutine(navigator.Teleport(Save_Point.savePointData.respawnPoint.map, Save_Point.savePointData.respawnPoint.location));
            BackgroundMusic.instance.Stop();
        }
    }
    }

    public float getATK
    {
        get { return attackDamage; }
    }

    public void changeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible || isDead)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hurt");
            animator.Play("ChangeColour");
            PlaySound(getHit);
        }
        KaguraBachiData.Health += amount;
        Healthbar.instance.setFillAmount(amount);


        if (!isDead && KaguraBachiData.Health  <=0) {
            Dead();
            isDead = true;
        }
            

    }

    public void ChangeKi(int amount)
    {
        KaguraBachiData.Ki += amount;
        Manabar.instance.setFillAmount(amount);
    }

    public void ChangeParasiteEssence(int amount)
    {
        EssenceCollected.instance.setNormalEssence(KaguraBachiData.ParasiteEssence, amount);
        KaguraBachiData.ParasiteEssence += amount;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource[0].PlayOneShot(clip);
    }

    public void PlaySoundHeal(AudioClip clip)
    {
        audioSource[1].PlayOneShot(clip);
    }

    /*IEnumerator changeBackgroundMusic(AudioClip clip){
        bgm.Stop();
        yield return new WaitForSeconds(2f);
        bgm.clip = clip;
        bgm.Play();
    }*/

}