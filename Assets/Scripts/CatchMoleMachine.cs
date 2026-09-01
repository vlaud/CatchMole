using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchMoleMachine : MonoBehaviour
{
    public enum State
    {
        Create, Start, Play, GameOver
    }
    public State myState = State.Create;

    public static CatchMoleMachine Inst = null;
    public List<Mole> moleList = new List<Mole>();
    public Transform myMoles;
    public Hammer myHammer;

    public TMPro.TMP_Text ScoreUI;
    public TimeUI myTimeUI;
    [SerializeField] int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            ScoreUI.text = _score.ToString();
        }
    }
    [SerializeField] float MaxTime = 30.0f;
    [SerializeField] float _playTime = 0.0f;
    public float playTime
    {
        get => _playTime;
        set
        {
            _playTime = Mathf.Clamp(value, 0.0f, MaxTime);
            myTimeUI.Value = _playTime / MaxTime;
        }
    }
    public GameObject TitleUI;
    public GameObject GameOverUI;

    [ContextMenu("CreateMole")]
    public void CreateMole()
    {
        moleList.Clear();
        for (int i = 0; i < myMoles.childCount;)
        {
            DestroyImmediate(myMoles.GetChild(i).gameObject);
        }
        
        Vector3 pos = new Vector3(-1.5f, 0.0f, 1.5f);
        
        for(int i = 0; i < 9; ++i)
        {
            pos.x = -1.5f + 1.5f * (i % 3);
            pos.z = 1.5f - 1.5f * (i / 3);
            GameObject obj = Instantiate(Resources.Load("Prefabs/Mole"), myMoles) as GameObject;
            obj.transform.localPosition = pos;
            obj.name = $"Mole{i.ToString("00")}";
            moleList.Add(obj.GetComponent<Mole>());
        }
    }
    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;

        switch(myState)
        {
            case State.Create:
                break;
            case State.Start:
                Score = 0;
                playTime = MaxTime;
                GameOverUI.SetActive(false);
                TitleUI.SetActive(true);
                break;
            case State.Play:
                Cursor.visible = false;
                TitleUI.SetActive(false);
                StartCoroutine(Activating());
                break;
            case State.GameOver:
                Cursor.visible = true;
                GameOverUI.SetActive(true);
                StopAllCoroutines();
                break;
        }
    }

    public void OnRetry()
    {
        ChangeState(State.Start);
    }

    public void OnClose()
    {
        Application.Quit();
    }
    void StateProcess()
    {
        switch (myState)
        {
            case State.Create:
                break;
            case State.Start:
                if(Input.anyKey)
                {
                    ChangeState(State.Play);
                }
                break;
            case State.Play:
                if (Input.GetMouseButtonDown(0) && !myHammer.IsSwing)
                {
                    myHammer.Hit();
                }
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("HitBox")))
                    {
                        myHammer.transform.position = hit.point;
                    }
                }
                playTime -= Time.deltaTime;
                if(Mathf.Approximately(playTime, 0.0f))
                {
                    ChangeState(State.GameOver);
                }
                break;
            case State.GameOver:
                break;
        }
    }
    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(State.Start);
    }

    public void HitAction()
    {
        myHammer.HitCheck();
        /*
        Ray ray = new Ray(myHammer.transform.position + Vector3.up * 3.0f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 3.0f, 1 << LayerMask.NameToLayer("Mole")))
        {
            Mole mole = hit.transform.GetComponent<Mole>();
            if (mole.IsHitable)
            {
               
                mole.OnHit();
            }

        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
        StateProcess();
    }

    IEnumerator Activating()
    {
        bool ready = false;
        do
        {
            ready = true;
            foreach (Mole mole in moleList)
            {
                if (mole.IsActivate)
                {
                    ready = false;
                    break;
                }
            }
            yield return null;
        }
        while (!ready);

        while (true)
        {
            bool check = true;
            do
            {
                int n = Random.Range(0, moleList.Count);
                if (!moleList[n].IsActivate)
                {
                    int rnd = Random.Range(0, 100);
                    if(rnd < 70)
                    {
                        moleList[n].OnActivate(1f, Mole.Type.Mole);
                    }
                    else
                    {
                        moleList[n].OnActivate(1f, Mole.Type.Monster);
                    }
                    check = false;
                }
                yield return null;
            }
            while (check);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
