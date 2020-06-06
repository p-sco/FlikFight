using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{

    private Vector2 LeftStartingPoint;
    private Vector2 RightStartingPoint;

    private int leftTouch = 99;
    private int rightTouch = 99;

    private float[] timeTouchBegan;
    private bool[] touchDidMove;
    private float tapTimeThreshold = 0.25f;

    private float swipeDistanceY;
    private float swipeDistanceX;

    private float minDistanceForSwipe = 1f;

    void Start() {
        timeTouchBegan = new float[10];
        touchDidMove = new bool[10];
    }

    void Update()
    {
        int i = 0;
        while(i < Input.touchCount) {

            Touch t = Input.GetTouch(i);
            Vector2 touchPos = getTouchPosition(t.position) * -1;
            int fingerIndex = t.fingerId;

            if (t.phase == TouchPhase.Began) {
                timeTouchBegan[fingerIndex] = Time.time;
                touchDidMove[fingerIndex] = false;
                if (t.position.x > Screen.width / 2) {
                    //execute player 2 things on right side
                    rightTouch = t.fingerId;
                    RightStartingPoint = touchPos;
                } else {
                    //execute player 1 things on left side
                    leftTouch = t.fingerId;
                    LeftStartingPoint = touchPos;
                }
            } else if(t.phase == TouchPhase.Moved) {
                touchDidMove[fingerIndex] = true;
                if (leftTouch == t.fingerId) {
                    Vector2 offset = touchPos - LeftStartingPoint;
                    int player = 1;
                    DetectSwipe(offset, player);
                }
                if (rightTouch == t.fingerId) {
                    Vector2 offset = touchPos - RightStartingPoint;
                    int player = 2;
                    DetectSwipe(offset, player);
                }
            } else if(t.phase == TouchPhase.Ended) {
                float tapTime = Time.time - timeTouchBegan[fingerIndex];
                if (tapTime <= tapTimeThreshold && touchDidMove[fingerIndex] == false) {
                    if (t.position.x < Screen.width / 2 && t.position.y < Screen.height / 2) {
                        SendBottomTap("Player1");
                    }
                    if (t.position.x > Screen.width / 2 && t.position.y < Screen.height / 2) {
                        SendBottomTap("Player2");
                    }
                    if (t.position.x < Screen.width / 2 && t.position.y > Screen.height / 2) {
                        SendTopTap("Player1");
                    }
                    if (t.position.x > Screen.width / 2 && t.position.y > Screen.height / 2) {
                        SendTopTap("Player2");
                    }
                }
                leftTouch = 99;
                rightTouch = 99;
            }
            ++i;
        }
    }

    Vector2 getTouchPosition(Vector2 touchPosition) {
        return Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }

    private void DetectSwipe(Vector2 direction, int player) {
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {
            if (Mathf.Abs(direction.y) > minDistanceForSwipe) {
                var swipeDir = direction.y < 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(swipeDir, player);
            }
        } else if (Mathf.Abs(direction.y) < Mathf.Abs(direction.x)) {
            if (Mathf.Abs(direction.x) > minDistanceForSwipe) {
                var swipeDir = direction.x < 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(swipeDir, player);
            }
        }
    }

    void SendBottomTap(string player) {
        GameObject.Find(player).SendMessage("SwipeAttack");
    }

    void SendTopTap(string player) {
        GameObject.Find(player).SendMessage("SpikeAttack");
    }

    private void SendSwipe(SwipeDirection direction, int player) {
        if (player == 1) {
            GameObject.Find("Player1").SendMessage("ReceiveAction", direction);
        } else if (player == 2) {
            GameObject.Find("Player2").SendMessage("ReceiveAction", direction);
        }
    }
}

public enum SwipeDirection {
    None,
    Up,
    Down,
    Left,
    Right
}
