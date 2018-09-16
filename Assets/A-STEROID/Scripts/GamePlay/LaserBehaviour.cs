using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum LaserBehaviourType
{
    Straight,
    CircularLaser,
}

public class LaserBehaviour : Projectile
{
    public LaserBehaviourType type;
    public Sprite laserSprite;
    public Color laserColor;
    public Vector3 spawnScale;
    [HideInInspector]
    public bool useCircleCollider;
    [HideInInspector]
    public Vector2 circleColliderOffset;
    [HideInInspector]
    public Vector2 boxColliderOffset;
    [HideInInspector]
    public Vector2 colliderSize;
    [HideInInspector]
    public float colliderRadius;

    // Circular laser settings
    public float radius = 0.5f;
    public float rotationSpeed = 20f;
    public float speed = 30f;
    public float wobwob = 0.2f;
    public float wobwobSpeed = 1f;

    // Global params
    public float startTime;

    float currentTime;
    Vector3 startPosition;
    Vector3 startScale;
    Vector3 startForward;
    SpriteRenderer _spriteRenderer;
    SpriteRenderer spriteRenderer
    {
        get { return _spriteRenderer = _spriteRenderer ?? GetComponent<SpriteRenderer>(); }
    }
    CircleCollider2D _circleCollider;
    CircleCollider2D circleCollider
    {
        get { return _circleCollider = _circleCollider ?? GetComponent<CircleCollider2D>(); }
    }
    BoxCollider2D _boxCollider;
    BoxCollider2D boxCollider
    {
        get { return _boxCollider = _boxCollider ?? GetComponent<BoxCollider2D>(); }
    }

    Dictionary<LaserBehaviourType, Action> behaviourUpdates;
    protected override void Start()
    {
        behaviourUpdates = new Dictionary<LaserBehaviourType, Action>
        {
            {LaserBehaviourType.Straight, StraightUpdate},
            {LaserBehaviourType.CircularLaser, CircularLaserUpdate},
        };
    }

    public void OnLaserSpawned()
    {
        transform.localScale = spawnScale;
        spriteRenderer.sprite = laserSprite;
        spriteRenderer.color = laserColor;

        if (boxCollider)
        {
            boxCollider.enabled = !useCircleCollider;
            boxCollider.size = colliderSize;
            boxCollider.offset = boxColliderOffset;
        }
        if (circleCollider)
        {
            circleCollider.enabled = useCircleCollider;
            circleCollider.radius = colliderRadius;
            circleCollider.offset = circleColliderOffset;
        }

        startPosition = transform.position;
        startScale = transform.localScale;
        startTime = Time.time;
        startForward = transform.right;
        AudioController.instance.PlayLaserAtPosition(startPosition);
    }

    public void CopyFrom(LaserBehaviour referenceLaser)
    {
        type = referenceLaser.type;
        rotationSpeed = referenceLaser.rotationSpeed;
        speed = referenceLaser.speed;
        radius = referenceLaser.radius;
        laserSprite = referenceLaser.laserSprite;
        laserColor = referenceLaser.laserColor;
        wobwob = referenceLaser.wobwob;
        wobwobSpeed = referenceLaser.wobwobSpeed;
        useCircleCollider = referenceLaser.useCircleCollider;
        spawnScale = referenceLaser.spawnScale;
    }

    void FixedUpdate()
    {
       
        if ((transform.position - GameManager.instance.playerPos).sqrMagnitude < GameManager.instance.playerSizeSqr + 9f)
        { 
            GameObject tmp;
            if ((tmp = NoColDebritManager.instance.DebritCollisionCheck(transform.position)) != null)
                tmp.GetComponent<NoColDebritController>().ToDoWhenLaserHit(this);
        }
        currentTime = Time.time - startTime;
        behaviourUpdates[type]();
    }

    void StraightUpdate()
    {
        transform.position = startPosition + startForward * currentTime * speed;
    }

    void CircularLaserUpdate()
    {
        Vector2 circlePos = new Vector2(Mathf.Sin(currentTime * rotationSpeed), Mathf.Cos(currentTime * rotationSpeed)) * radius;
        transform.position = startPosition + startForward * currentTime * speed + (Vector3)circlePos;
        Vector3 scaleModifier = new Vector3(Mathf.Sin(currentTime * wobwobSpeed), Mathf.Cos(currentTime * wobwobSpeed), 0);
        transform.localScale = startScale + wobwob * scaleModifier;
    }

    protected override void DestroyProjectile()
    {
        LaserPool.instance.FreeLaser(this);
    }
}
