using System.Collections;
using UnityEngine;

public class Sword : Weapon {

    // name of trigger for transition to each attack animation 
    private const string slash1AnimParam = "Slash_1";
    private const string slash2AnimParam = "Slash_2";
    private const string slash3AnimParam = "Slash_3";
    private const float slash1ExitTime = 1f;
    private const float slash2ExitTime = 1f;
    private const float slash3ExitTime = 1f;
    // audio clips for each attack
    [SerializeField] private AudioClip slash1Audio;
    [SerializeField] private AudioClip slash2Audio;
    [SerializeField] private AudioClip slash3Audio;
    [SerializeField] private float combo1Window = 0.75f;
    [SerializeField] private float combo2Window = 0.75f;
    // tree root node
    private Node rootNode;
    private Node currNode;
    // bool that represents whether player can attack based on exit time
    private bool attackReady;
    // if transition to next combo is still open
    private bool canCombo;
    // timer and exit time values for when player can attack
    private float currExitTime;
    private float lastAttackTimer;
    // timer and window values for wait between combos
    private float currWindow;
    private float comboTimer;

    private void Start()
    {
        // setup attack tree
        rootNode = new Node(slash1AnimParam, slash1ExitTime, combo1Window, slash1Audio);
        rootNode.childNode = new Node(slash2AnimParam, slash2ExitTime, combo2Window, slash2Audio);
        rootNode.childNode.childNode = new Node(slash3AnimParam, slash3ExitTime, 0f, slash3Audio);

        currNode = rootNode;
        attackReady = true;
        canCombo = false;
    }

    private void Update()
    {
        if (!attackReady)
        {
            ContinueAttackTimer();
        }

        if (canCombo)
        {
            ContinueComboTimer();
        }
    }

    public override bool CanAttack()
    {
        if (attackReady || canCombo)
        {
            return true;
        }

        return false;
    }

    public override void Attack()
    {
        if (canCombo)
        {
            currNode = currNode.childNode;
        }
        else
        {
            currNode = rootNode;
        }

        StartAttack();
    }

    private void StartAttack()
    {
        _animParam = currNode.AnimParam;
        _animExitTime = currNode.ExitTime;

        lastAttackTimer = 0f;
        currExitTime = currNode.ExitTime;
        attackReady = false;

        _animator.SetTrigger(currNode.AnimParam);
        //currNode.SFX;

        if (currNode.childNode != null)
        {
            StartComboTimer();
        }
        else
        {
            currNode = rootNode;
            canCombo = false;
        }
    }

    private void ContinueAttackTimer()
    {
        if (lastAttackTimer < currExitTime)
        {
            lastAttackTimer += Time.deltaTime;
        }
        else
        {
            attackReady = true;
        }
    }

    private void StartComboTimer()
    {
        comboTimer = 0f;
        currWindow = currNode.ComboWindow;
        canCombo = true;
    }

    private void ContinueComboTimer()
    {
        if (comboTimer < currWindow)
        {
            comboTimer += Time.deltaTime;
        }
        else
        {
            canCombo = false;
        }
    }

    private class Node
    {
        private string animParam;
        private float exitTime;
        private float comboWindow;
        private AudioClip sfx;

        public Node childNode;

        public string AnimParam { get { return animParam; } }
        public float ExitTime { get { return exitTime; } }
        public float ComboWindow { get { return comboWindow; } }
        public AudioClip SFX { get { return sfx; } }

        public Node(string animParam, float exitTime, float comboWindow, AudioClip sfx)
        {

            this.animParam = animParam;
            this.exitTime = exitTime;
            this.comboWindow = comboWindow;
            this.sfx = sfx;
            childNode = null;
        }
    }
}
