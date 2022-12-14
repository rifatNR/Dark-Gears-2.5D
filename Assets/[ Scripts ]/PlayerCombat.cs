using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCtrl))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField]
    private int combo = 0;
    [SerializeField]
    private int maxCombo = 3;
    [SerializeField]
    private int currCombo = 0;
    [SerializeField]
    private bool canAttack = true;
    [SerializeField]
    private bool startAttack = false;
    [SerializeField]
    private bool isAttacking = false;

    [Header("Block")]
    [SerializeField]
    private bool startBlock = false;
    [SerializeField]
    private int blockCombo = 1;


    [SerializeField]
    private float comboTime = 1f;
    private float comboTimerReset;

    [Header("AUDIO:")]
    public AudioClip[] swordLightAudio;
    public AudioClip[] swordHeavyAudio;
    [Range(0, 1)]
    public float swordAudioVolume = 0.5f;

    private Animator Anim;
    private PlayerCtrl playerCtrl;

    void Start()
    {
        Anim = GetComponent<Animator>();
        playerCtrl = GetComponent<PlayerCtrl>();
        comboTimerReset = Time.time + comboTime;
    }

    void Update()
    {
        IsPLayingAttackAnimation();

        playerCtrl.setCanMove(!isAttacking);
        playerCtrl.setCanRotate(!isAttacking);

        Attack();
        Block();

        AnimationHandler();
    }

    public void Attack()
    {
        if (Time.time > comboTimerReset && combo > 0)
        {
            combo = 0;
            currCombo = 0;
        }

        if (combo > 0) startAttack = false;
        if (canAttack && InputHandler.isAttack && combo <= maxCombo)
        {
            if (combo == 0) startAttack = true;
            comboTimerReset = Time.time + comboTime;
            canAttack = false;
            if (!isAttacking || combo != 0) combo++;
        }


        if (!InputHandler.isAttack) canAttack = true;
    }

    public void Block()
    {
        if(InputHandler.isBlock)
        {
            blockCombo = Random.Range(1, 4);
            startBlock = true;
        } else
        {
            blockCombo = 1;
            startBlock = false;
        }
    }

    public void AnimationHandler()
    {
        Anim.SetInteger("Combo", combo);
        Anim.SetBool("startAttack", startAttack);
        Anim.SetBool("startBlock", startBlock);
        Anim.SetFloat("blockIndex", blockCombo);
    }

    private void IsPLayingAttackAnimation()
    {
        isAttacking = Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") || Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 2") || Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack 3");
    }

    // =====================================
    //           Animation Events
    // =====================================
    private void AttackEvent(AnimationEvent animationEvent)
    {
        if (currCombo <= 2) currCombo++;

        if(currCombo == maxCombo)
        {
            combo = 0;
            currCombo = 0;

            if (swordHeavyAudio.Length > 0)
            {
                var index = Random.Range(0, swordHeavyAudio.Length);
                AudioSource.PlayClipAtPoint(swordHeavyAudio[index], transform.position, swordAudioVolume);
            }
        } else
        {
            if (swordLightAudio.Length > 0)
            {
                var index = Random.Range(0, swordLightAudio.Length);
                AudioSource.PlayClipAtPoint(swordLightAudio[index], transform.position, swordAudioVolume);
            }
        }
    }

}
