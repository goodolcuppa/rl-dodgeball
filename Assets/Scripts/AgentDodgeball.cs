using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public enum Team {
    Blue = 0,
    Orange = 1
}

public class AgentDodgeball : Agent {

    public Team team;
    [SerializeField] float rotationSpeed;
    [SerializeField] float basePower;
    float hitPower;
    

    [Header("Reward Coefficients")]
    [SerializeField] float hitReward;
    [SerializeField] float touchReward;
    [SerializeField] float existentialReward;
    [SerializeField] float barrierPenalty;

    [Header("Environment")]
    [SerializeField] GameObject area;
    DodgeballEnvController envController;
    [SerializeField] string floorTag;
    Rigidbody rb;
    DodgeballSettings settings;

    void Start() {
        rb = GetComponent<Rigidbody>();
        settings = FindObjectOfType<DodgeballSettings>();
        envController = area.GetComponent<DodgeballEnvController>();
    }

    public override void OnEpisodeBegin() {

    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        
        // Forward input
        if (Input.GetKey(KeyCode.W)) continuousActions[0] = 1;
        if (Input.GetKey(KeyCode.S)) continuousActions[0] = -1;

        // Lateral input
        if (Input.GetKey(KeyCode.D)) continuousActions[1] = 1;
        if (Input.GetKey(KeyCode.A)) continuousActions[1] = -1;

        // Rotation input
        continuousActions[2] = Input.GetAxis("Mouse X");
    }

    void MoveAgent(ActionSegment<float> actions) {
        hitPower = 0f;
        float forward = actions[0];
        float lateral = actions[1];
        float rotation = actions[2] * rotationSpeed;

        Vector3 direction = Vector3.zero;
        if (forward != 0) {
            if (forward > 0) hitPower = 1f;
            direction = forward * transform.forward;
        }
        direction += lateral * transform.right;

        rb.velocity = direction.normalized * settings.agentSpeed;
        transform.Rotate(transform.up * rotation * Time.deltaTime);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, -transform.up, out hit, 1f) || !hit.collider.gameObject.CompareTag(floorTag)) {
            envController.ResetScene();
        }

        MoveAgent(actions.ContinuousActions);
    }

    void OnCollisionEnter(Collision collision) {
        float force = basePower * hitPower;

        if (collision.gameObject.CompareTag("BallActive") || collision.gameObject.CompareTag("BallInactive")) {
            AddReward(touchReward);
            //Vector3 hitDirection = collision.contacts[0].point - transform.position;
            //hitDirection = hitDirection.normalized;
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            ballRb.AddForce(transform.forward * force);
            
            if (force > 0) {
                collision.gameObject.GetComponent<DodgeballController>().Activate(tag, this);
            }
        }

        if (collision.gameObject.CompareTag("Barrier") || collision.gameObject.CompareTag("Wall"))
            AddReward(barrierPenalty);
    }

    public void OnBallHit() {
        AddReward(hitReward);
    }
}
