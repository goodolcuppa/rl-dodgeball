using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeballController : MonoBehaviour {

    [SerializeField] GameObject area;
    DodgeballEnvController envController;
    [SerializeField] string blueAgentTag;
    [SerializeField] string orangeAgentTag;
    [SerializeField] string wallTag;
    [SerializeField] string inactiveTag;
    [SerializeField] string activeTag;

    bool active = false;
    string teamActivated;
    AgentDodgeball agentActivated;

    Rigidbody rb;

    void Start() { // change to Initialize if there are issues
        envController = area.GetComponent<DodgeballEnvController>();
        rb = GetComponent<Rigidbody>();
        Debug.Log(GetComponent<Rigidbody>() == null);
    }

    void FixedUpdate() {
        if (rb.velocity.magnitude < 0.05f) {
            Deactivate();
        }
    }

    public void Activate(string team, AgentDodgeball agent) {
        active = true;
        teamActivated = team;
        agentActivated = agent;
        tag = activeTag;
    }

    public void Deactivate() {
        active = false;
        tag = inactiveTag;
    }

    void OnCollisionEnter(Collision collision) {
        if (active && !collision.gameObject.CompareTag(teamActivated)){
            if (collision.gameObject.CompareTag(blueAgentTag)) {
                Debug.Log("Blue hit.");
                envController.AwardPoint(Team.Orange);
            }
            if (collision.gameObject.CompareTag(orangeAgentTag)) {
                Debug.Log("Orange hit.");
                envController.AwardPoint(Team.Blue);
            }
        }

        if (collision.gameObject.CompareTag(wallTag)) {
            Deactivate();
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (active && collider.CompareTag("Barrier")) {
            agentActivated.OnBallHit();
            Debug.Log("Ball hit.");
        }
    }
}
