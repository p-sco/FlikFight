using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPlayerSimulatneousInputScript : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerScript;

    private Vector2 LeftStartingPoint;
    private Vector2 RightStartingPoint;

    private int leftTouch = 99;
    private int rightTouch = 99;

    private float[] timeTouchBegan;
    private bool[] touchDidMove;
    [SerializeField]
    private float tapTimeThreshold = 0.5f;

    private float swipeDistanceY;
    private float swipeDistanceX;

    private float minDistanceForSwipe = 1f;

    // Start is called before the first frame update
    void Start()
    {
        timeTouchBegan = new float[10];
        touchDidMove = new bool[10];
        print("PlayerInputScript starting");

    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (i < Input.touchCount)
        {

            Touch t = Input.GetTouch(i);
            Vector2 touchPos = getTouchPosition(t.position) * -1;
            int fingerIndex = t.fingerId;

            if (t.phase == TouchPhase.Began)
            {
                timeTouchBegan[fingerIndex] = Time.time;
                touchDidMove[fingerIndex] = false;
                if (t.position.x > Screen.width / 2)
                {
                    //execute player 2 things on right side
                    rightTouch = t.fingerId;
                    RightStartingPoint = touchPos;
                }
                else
                {
                    //execute player 1 things on left side
                    leftTouch = t.fingerId;
                    LeftStartingPoint = touchPos;
                }
            }
            if (t.phase == TouchPhase.Moved)
            {
                touchDidMove[fingerIndex] = true;
                if (leftTouch == t.fingerId && _playerScript._playerId == 1)
                {
                    Vector2 offset = touchPos - LeftStartingPoint;
                    DetectSwipe(offset);
                }
                if (rightTouch == t.fingerId && _playerScript._playerId == 2)
                {
                    Vector2 offset = touchPos - RightStartingPoint;
                    DetectSwipe(offset);
                }
            }
            if (t.phase == TouchPhase.Ended)
            {
                float tapTime = Time.time - timeTouchBegan[fingerIndex];
                if (tapTime <= tapTimeThreshold && touchDidMove[fingerIndex] == false)
                {
                    if (t.position.x < Screen.width / 2 && t.position.y < Screen.height / 2
                        && _playerScript._playerId == 1)
                    {
                        SendBottomTap();
                    }
                    if (t.position.x > Screen.width / 2 && t.position.y < Screen.height / 2
                        && _playerScript._playerId == 2)
                    {
                        SendBottomTap();
                    }
                    if (t.position.x < Screen.width / 2 && t.position.y > Screen.height / 2
                        && _playerScript._playerId == 1)
                    {
                        SendTopTap();
                    }
                    if (t.position.x > Screen.width / 2 && t.position.y > Screen.height / 2
                        && _playerScript._playerId == 2)
                    {
                        SendTopTap();
                    }
                }
                leftTouch = 99;
                rightTouch = 99;
            }
            ++i;
        }
    }
    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }

    private void DetectSwipe(Vector2 direction)
    {
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (Mathf.Abs(direction.y) > minDistanceForSwipe)
            {
                var swipeDir = direction.y < 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(swipeDir);
            }
        }
        else if (Mathf.Abs(direction.y) < Mathf.Abs(direction.x))
        {
            if (Mathf.Abs(direction.x) > minDistanceForSwipe)
            {
                var swipeDir = direction.x < 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(swipeDir);
            }
        }
    }

    void SendBottomTap()
    {
        _playerScript.SwipeAttack();
    }

    void SendTopTap()
    {
        _playerScript.SpikeAttack();
    }

    private void SendSwipe(SwipeDirection direction)
    {
        //_playerScript._movementScript.ReceiveAction(direction);
        
    }
}

