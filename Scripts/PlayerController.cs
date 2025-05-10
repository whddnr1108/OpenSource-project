using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private KeyCode keyCodeAttack = KeyCode.Space;
    private bool isDie = false;
    private Movement2D movement2D;
    private Weapon weapon;
    private Animator animator;

    private int score;
    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDie == true) return;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement2D.MoveTo(new Vector3(x, y, 0));

        if (Input.GetKeyDown(keyCodeAttack))
        {
            weapon.StartFiring();
        }
        else if (Input.GetKeyUp(keyCodeAttack))
        {
            weapon.StopFiring();
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y),
            transform.position.z);
    }
    public void OnDie()
    {
        movement2D.MoveTo(Vector3.zero);
        animator.SetTrigger("onDie");
        Destroy(GetComponent<CircleCollider2D>());
        isDie = true;
    }
    private IEnumerator LoadGameOverSceneAfterAnimation()
    {
        yield return new WaitForSeconds(1f);
        OnDieEvent();
    }

    public void OnDieEvent()
    {
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene(nextSceneName);
    }
}
