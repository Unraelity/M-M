using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    [SerializeField] private string weaponName = "None";
    [SerializeField] protected Animator _animator;
    protected string _animParam = "None";
    protected float _animExitTime = 0f;

    public string WeaponName
    {
        get { return weaponName; }
        private set { weaponName = value; }
    }
    public string AnimParam
    {
        get { return _animParam; }
        private set { _animParam = value; }
    }
    public float AnimExitTime
    {
        get { return _animExitTime; }
        private set { _animExitTime = value;}
    }

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();

            if (_animator == null)
            {
                Debug.LogError("No Animator attached to " + WeaponName);
            }
        }
    }

    public abstract bool CanAttack();

    public abstract void Attack();

}