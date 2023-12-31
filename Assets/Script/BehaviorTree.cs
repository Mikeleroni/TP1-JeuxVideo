using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum NodeState { Running, Success, Failure }

//public static class GlobalVariables
//{
//    public static bool Chase { get; set; }
//}
public abstract class Node
{
    protected NodeState State { get; set; }
    public Node Parent { get; set; }
    protected List<Node> children = new List<Node>();

    Dictionary<string, object> data = new Dictionary<string, object>();

    public void SetData(string key, object value)
    {
        data[key] = value;
    }

    public object GetData(string key)
    {
        if (data.TryGetValue(key, out object value))
        {
            return value;
        }
        if (Parent != null)
        {
            return Parent.GetData(key);
        }
        return null;
    }

    public bool TryRemoveData(string key)
    {
        if (data.Remove(key))
        {
            return true;
        }
        if (Parent != null)
        {
            return Parent.TryRemoveData(key);
        }
        return false;
    }
    public Node()
    {
        Parent = null;
        State = NodeState.Running;
    }

    public Node(List<Node> pChildren)
    {
        foreach (Node child in pChildren)
        {
            Attach(child);
        }
    }

    public void Attach(Node n)
    {
        children.Add(n);
        n.Parent = this;
    }

    public abstract NodeState Evaluate();

}

public class Sequence : Node
{
    public Sequence(List<Node> n) : base(n) { }

    public override NodeState Evaluate()
    {
        foreach (Node n in children)
        {
            State = n.Evaluate();
            if (State != NodeState.Success)
            {
                return State;
            }
        }
        State = NodeState.Success;
        return NodeState.Success;
    }
}

public class Selector : Node
{
    public Selector(List<Node> n) : base(n)
    {
        if (n.Count == 0)
        {
            throw new ArgumentException();
        }
    }
    public override NodeState Evaluate()
    {
        foreach (Node n in children)
        {
            State = n.Evaluate();
            if (State != NodeState.Failure)
            {
                return State;
            }
        }
        State = NodeState.Failure;
        return NodeState.Failure;
    }
}

public class Inverter : Node
{
    public Inverter(List<Node> n) : base(n)
    {
        if (n.Count == 0)
        {
            throw new ArgumentException();
        }
    }
    public override NodeState Evaluate()
    {
        NodeState childState = children[0].Evaluate();
        if (childState == NodeState.Success)
        {
            State = NodeState.Failure;
        }
        else if (childState == NodeState.Failure)
        {
            State = NodeState.Success;
        }
        else
        {
            State = NodeState.Running;
        }
        return State;
    }
}



public class GoToTarget : Node
{
    Transform target;
    NavMeshAgent agent;
    Animator animator;

    public GoToTarget(Transform target, NavMeshAgent agent, Animator animator) : base()
    {
        this.target = target;
        this.agent = agent;
        this.animator = animator;
        
    }

    public override NodeState Evaluate()
    {
        agent.destination = target.position;
        if (agent.SetDestination(target.position))
        {

            State = NodeState.Running;
            animator.SetBool("Walking", true);
        }
        else
        {
            State = NodeState.Failure;
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            State = NodeState.Success;
        }
        return State;
    }
}

public class IsWithInRage : Node
{
    Transform target;
    Transform self;
    float detectionRange;
    public IsWithInRage(Transform target, Transform self, float detectionRange) : base()
    {
        this.target = target;
        this.self = self;
        this.detectionRange = detectionRange;
    }
    public override NodeState Evaluate()
    {
        State = NodeState.Failure;
        if (Vector3.Distance(self.position, target.position) <= detectionRange)
        {
            //Debug.Log("J'tai vue");
            State = NodeState.Success;
        }
        return State;
    }
}

public class Tombe : Node
{
    Animator animator;
    float waitTime = 0;
    float elapsedTime = 0;
    bool tombe=false;
    float time =4.6f ;
    NavMeshAgent agent;
    public Tombe(NavMeshAgent agent, Animator animator, float waitingTime)
    {
        this.animator = animator;
        this.waitTime = waitingTime;
        this.agent = agent;
    }
    public override NodeState Evaluate()
    {
        State = NodeState.Failure;
        if (!tombe)
        {
            if (time >= 4.5f)
                agent.speed = 1.25f;
            if (time <= 1.5f)
                agent.speed = 1.25f;
            else if (time <= 4.5f)
                agent.speed = 0;

            time += Time.deltaTime;
            elapsedTime += Time.deltaTime;
            //Debug.Log(elapsedTime + "/" + agent.speed + "/" + animator.GetBool("Walking"));
            if(animator.GetBool("Walking") == false && elapsedTime >= waitTime)
            {
                elapsedTime= waitTime/1.2f;
            }
            else if(elapsedTime >= waitTime && animator.GetBool("Walking")) 
            {
                time = 0;
                tombe = true;
                elapsedTime = 0;
                animator.SetBool("tombe", true);
                agent.speed = 0f;

                State = NodeState.Success;
            }
        }
        else
        {
            tombe = false;
            animator.SetBool("tombe", false);
            
        }
        return State;
    }
}

public class PatrolTask : Node
{
    List<Transform> targets;
    NavMeshAgent agent;
    int targetIndex = 0;
    Animator animator;
    float waitTime = 0;
    float elapsedTime = 0;
    bool isWaiting = false;

    public PatrolTask(List<Transform> targets, NavMeshAgent agent, float waitingTime, Animator animator)
    {
        this.targets = targets;
        this.agent = agent;
        this.waitTime = waitingTime;
        this.animator = animator;
    }
    public override NodeState Evaluate()
    {
        State = NodeState.Running;
        if (isWaiting)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitTime)
            {
                isWaiting = false;
                elapsedTime = 0;
                targetIndex = (targetIndex + 1) % targets.Count;
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            if (!agent.SetDestination(targets[targetIndex].position))
            {
                State = NodeState.Failure;
            }
            if (Vector3.Distance(agent.transform.position, targets[targetIndex].position) <= agent.stoppingDistance)
            {
                isWaiting = true;
                animator.SetBool("Walking", false);
            }
        }
        return State;

        /*agent.destination = targets[targetIndex].position;
        if (agent.SetDestination(targets[targetIndex].position))
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                targetIndex++;
                if(targetIndex == targets.Count)
                {
                    targetIndex= 0;
                }
            }
            return State = NodeState.Running;
        }
        return NodeState.Failure;*/


    }
}

