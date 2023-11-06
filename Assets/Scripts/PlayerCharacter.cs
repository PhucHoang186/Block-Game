using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.Mathematics;
using System.Collections.Generic;
using System;

public enum AnimationName
{
    Move,
    Rotate,
    Idle,
    Jump,
    Celebrate,
    Lose,
    Losev2,
    Losev3
}

public enum PlayerState
{
    Movement,
    Celebrate,
    Lose
}

public enum LoseType
{
    MoveToWrongNode,
    MoveWithWrongAngle,
    MoveToEdge,
}

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject loseIcon;
    Animator anim;
    private PlayerState playerState;
    private Vector3 moveDirection;
    private Vector3 desAngle;
    private Vector3 desPoint;
    private LoseType loseType;
    private bool reachDesMove;
    private bool reachDesAngle;
    private bool isLoseGame;
    private int currentFacingDirection; // 0 is front, 1 is right, 2 is back 3 is left
    public Node CurrentNodeOn { get; set; }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void StartMove(Vector3 direction)
    {
        if (playerState != PlayerState.Movement)
            return;

        // this one here is a mess
        reachDesMove = false;
        moveDirection = direction;
        var nextNode = GetNextNode(direction);
        if (nextNode != null)
        {
            if (!nextNode.IsPlaced)
            {

                CurrentNodeOn = nextNode;
                desPoint = CurrentNodeOn.transform.position;
                isLoseGame = CheckIfMoveDirectionMatchFacingDirection();
                if (isLoseGame)
                {
                    // move but not rotate
                    loseType = LoseType.MoveWithWrongAngle;
                    PlayAnim(AnimationName.Losev2);
                }
                else
                {
                    PlayAnim(AnimationName.Move);
                }
            }
            else
            {
                // call endgame
                // hit to node
                isLoseGame = true;
                PlayAnim(AnimationName.Move);
                loseType = LoseType.MoveToWrongNode;
                desPoint = CalculateDesPointOnLose(nextNode.transform.position, direction);
            }
        }
        else
        {
            // move to edge
            isLoseGame = true;
            loseType = LoseType.MoveToEdge;
            PlayAnim(AnimationName.Losev3);
            desPoint = CalculateDesPointOnLose(CurrentNodeOn.transform.position + direction, direction * 2);
        }
    }

    private Vector3 CalculateDesPointOnLose(Vector3 nextNodePosition, Vector3 direction)
    {

        if (CurrentNodeOn.transform.position.x == nextNodePosition.x)
        {
            return CurrentNodeOn.transform.position + Vector3.forward * direction.z;
        }
        else
        {
            return CurrentNodeOn.transform.position + Vector3.right * direction.x;
        }
    }

    private void Move()
    {
        if (reachDesMove)
            return;
        if (Vector3.Distance(transform.position, desPoint) > 0.1f) // check distance threshold
        {

            transform.position = Vector3.Lerp(transform.position, transform.position + moveDirection * moveSpeed, Time.deltaTime);
        }
        else
        {
            ReachDesMove();
        }
    }

    private void ReachDesMove()
    {
        reachDesMove = true;
        if (isLoseGame)
        {
            PlayLoseAnimation();
            OnLoseGame();
        }
        else
        {
            PlayAnim(AnimationName.Idle);
        }
    }

    private void ReachDesAngle()
    {
        reachDesAngle = true;
        PlayAnim(AnimationName.Idle);
    }

    public void StartRotate(float angle)
    {
        if (playerState != PlayerState.Movement)
            return;

        reachDesAngle = false;
        desAngle = transform.eulerAngles;
        desAngle.y += angle;
        currentFacingDirection = CalculateFacingDirection(angle);
    }

    private int CalculateFacingDirection(float angle)
    {
        if (angle > 0)
        {
            return (currentFacingDirection + 1) % 4;
        }
        else
        {
            return (4 + (currentFacingDirection - 1)) % 4;
        }
    }

    private bool CheckIfMoveDirectionMatchFacingDirection()
    {
        return moveDirection != Direction.FaceDirection[currentFacingDirection];
    }


    private void Rotate()
    {
        if (reachDesAngle)
            return;
        if (desAngle.y != transform.eulerAngles.y)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(desAngle), Time.deltaTime * rotateSpeed);
        else
            ReachDesAngle();
    }

    private void Update()
    {
        if (playerState != PlayerState.Movement)
            return;
        Move();
        Rotate();
    }

    public void PlayAnim(AnimationName animName, float transitionTime = 0.1f)
    {
        if (anim == null)
            return;
        anim.CrossFade(animName.ToString(), transitionTime);
    }

    private Node GetNextNode(Vector3 direction)
    {
        return GridManager.Instance.GetNextNode(direction, CurrentNodeOn);
    }

    public void Celebrate()
    {
        playerState = PlayerState.Celebrate;
        transform.DORotate(new Vector3(0f, 180f, 0f), 1f).OnComplete(() =>
        {
            PlayCelebrateAnimation();
        });
    }

    private void PlayCelebrateAnimation()
    {
        StartCoroutine(CorPlayCelebrateAnimation());
    }

    private IEnumerator CorPlayCelebrateAnimation()
    {
        PlayAnim(AnimationName.Celebrate);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.OnChangeState(GameState.Win);
    }

    private void OnLoseGame()
    {
        StartCoroutine(CorOnLoseGame());
    }

    private IEnumerator CorOnLoseGame()
    {
        playerState = PlayerState.Lose;
        yield return new WaitForSeconds(0.5f);
        loseIcon.SetActive(true);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.OnChangeState(GameState.Lose);
    }

    private void PlayLoseAnimation()
    {
        switch (loseType)
        {
            case LoseType.MoveToWrongNode:
                PlayAnim(AnimationName.Lose);
                break;
            case LoseType.MoveWithWrongAngle:
                break;
            case LoseType.MoveToEdge:
                break;
        }
    }
}
[Serializable]
public static class Direction
{
    public static List<Vector3> FaceDirection = new List<Vector3>
    {
        Vector3Int.forward,
        Vector3Int.right,
        Vector3Int.back,
        Vector3Int.left,
    };
}
