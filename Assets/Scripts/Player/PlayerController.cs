using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour
{

    private CharacterStats characterStats;
    private CharacterMovement characterMovement;
    private RogueAbilities rogueAbilities;


    [SerializeField] private LayerMask characterLayer;

    private Vector2 startPos;

    private Vector2 endPos;
    
    [SerializeField] private float swipeDisFrom800Width = 200f;

    // Start is called before the first frame update
    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        characterMovement = GetComponent<CharacterMovement>();
        rogueAbilities = GetComponent<RogueAbilities>();
    }

    // Update is called once per frame
    void Update()
    {
        characterMovement.SetMoveDir(Joystick.Instance.GetNormalizedMovement());

        TouchInputs();
    }

    private void TouchInputs()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(Input.touchCount-1).phase == TouchPhase.Began)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(Input.touchCount - 1).position);

                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector3.forward, 100f, characterLayer);

                if (hit.collider)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats cs))
                    {
                        // Target
                        if (cs.enabled) characterStats.SetTarget(cs);
                    }
                }
                else
                {
                }
            }

            SwipeChecks(Input.GetTouch(0));
        }
    }

    private void SwipeChecks(Touch touch)
    {

        if (touch.phase == TouchPhase.Began)
        {
            endPos = touch.position;
            startPos = touch.position;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            startPos = touch.position;
            CheckTheSwipe();
        }

        if (touch.phase == TouchPhase.Ended)
        {
            startPos = touch.position;
            CheckTheSwipe();
        }
    }

    void CheckTheSwipe()
    {
        if (Vertical() > Camera.main.pixelHeight / 2 && Vertical() > Horizontal())
        {
            if (startPos.y - endPos.y > 0)
            {
                OnSwipeUp();
            }
            else if (startPos.y - endPos.y < 0)
            {
                OnSwipeDown();
            }
            endPos = startPos;
        }

        else if (Horizontal() > Camera.main.pixelWidth / 2 && Horizontal() > Vertical())
        {
            if (startPos.x - endPos.x > 0)
            {
                OnSwipeRight();
            }
            else if (startPos.x - endPos.x < 0)
            {
                OnSwipeLeft();
            }
            endPos = startPos;
        }
    }

    float Vertical()
    {
        return Mathf.Abs(startPos.y - endPos.y);
    }

    float Horizontal()
    {
        return Mathf.Abs(startPos.x - endPos.x);
    }

    void OnSwipeUp()
    {
        if (rogueAbilities) rogueAbilities.SelectAbilitySet(1);
    }

    void OnSwipeDown()
    {
    }

    void OnSwipeLeft()
    {
    }

    void OnSwipeRight()
    {
        if (rogueAbilities) rogueAbilities.SelectAbilitySet(0);
    }

    public void OnDrawGizmos()
    {
        if (Input.touchCount > 0)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Debug.DrawRay(touchPos, Vector3.forward * 100, Color.red);
        }
    }
}
