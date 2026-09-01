using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public bool IsActivate
    {
        get => myState == State.NoShow ? false : true;
        
    }
    public bool IsHitable
    {
        get => myState == State.Down ? false : myState == State.NoShow ? false : true;
    }
    public enum Type
    {
        Mole, Sheep, Monster
    }
    public Type myType = Type.Mole;
    public GameObject[] orgModels;
    public enum State
    {
        Create, NoShow, Up, Show, Down
    }
    public State myState = State.Create;

    float myHeight = 0.0f;
    float Dist = 0.0f;
    float mySpeed = 2.0f;

    float playtime = 0.0f;
    bool isHitted = false;
    float showTime = 2.0f;
    void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Create:
                break;
            case State.NoShow:
                break;
            case State.Up:
                break;
            case State.Show:
                break;
            case State.Down:
                Dist = myHeight - Dist;
                if(!isHitted && myType == Type.Monster)
                {
                    CatchMoleMachine.Inst.playTime -= 3.0f;
                }
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case State.Create:
                break;
            case State.NoShow:
                break;
            case State.Up:
                Move(Vector3.up, State.Show);
                break;
            case State.Show:
                playtime += Time.deltaTime;
                if(playtime > showTime)
                {
                    playtime = 0.0f;
                    ChangeState(State.Down);
                }
                break;
            case State.Down:
                Move(Vector3.down, State.NoShow);
                break;
        }
    }
    void Move(Vector3 Dir, State s)
    {
        float delta = mySpeed * Time.deltaTime;
        if (delta > Dist)
        {
            delta = Dist;
        }
        Dist -= delta;
        transform.Translate(Dir * delta);
        if (Mathf.Approximately(Dist, 0.0f))
        {
            ChangeState(s);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(State.NoShow);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    void Initialize()
    {
        foreach(Transform tr in transform)
        {
            Destroy(tr.gameObject);
        }
        GameObject obj = Instantiate(orgModels[(int)myType], transform);
        obj.transform.localPosition = new Vector3(0, 2, 0);
        obj.transform.localRotation = Quaternion.Euler(0, 180, 0);
        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    public void OnActivate(float height, Type type)
    {
        if(myType != type)
        {
            myType = type;
            Initialize();
        }

        switch(myType)
        {
            case Type.Monster:
                showTime = 0.1f;
                break;
            default:
                showTime = 2.0f;
                break;
        }
        isHitted = false;
        Dist = myHeight = height;
        ChangeState(State.Up);
    }

    public void OnHit()
    {
        CatchMoleMachine.Inst.Score += 10;
        CatchMoleMachine.Inst.playTime += 3.0f;
        isHitted = true;
        ChangeState(State.Down);
    }
}
