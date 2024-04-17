using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(PlayerUI))]
public class PlayerController : Character
{
    private PlayerInputAction _input;

    protected override void Awake()
    {
        _input = new PlayerInputAction();
        base.Awake();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    public void Killed()
    {
        _gm.AddKill();
    }
    public override void AddCoin(int coins)
    {
        _gm.AddCoins(coins);
        onUpdateUI?.Invoke();
    }

    private void Update()
    {
        if (isDead)
            return;
        if (_gm.isGameStarted)
        {
            Vector2 dir = _input.Player.Move.ReadValue<Vector2>();

            Move(dir.x, dir.y);
        }
        RotateSwords();
    }
}
