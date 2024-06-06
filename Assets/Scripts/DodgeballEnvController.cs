using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using TMPro;

public class DodgeballEnvController : MonoBehaviour {

    [System.Serializable]
    public class PlayerInfo {
        public AgentDodgeball agent;
        [HideInInspector]
        public Vector3 startingPos;
        [HideInInspector]
        public Quaternion startingRot;
        [HideInInspector]
        public Rigidbody rb;
    }

    [System.Serializable]
    public class BallInfo {
        public GameObject ball;
        [HideInInspector]
        public Vector3 startingPos;
        [HideInInspector]
        public Quaternion startingRot;
        [HideInInspector]
        public Rigidbody rb;
    }

    [SerializeField] int maxEnvSteps;
    [SerializeField] int envSteps;

    public List<PlayerInfo> agents;
    public List<BallInfo> balls;

    private DodgeballSettings settings;

    SimpleMultiAgentGroup blueGroup;
    SimpleMultiAgentGroup orangeGroup;
    int blueScore;
    int orangeScore;
    [SerializeField] TextMeshProUGUI blueText;
    [SerializeField] TextMeshProUGUI orangeText;

    void Start() {
        settings = FindObjectOfType<DodgeballSettings>();
        blueGroup = new SimpleMultiAgentGroup();
        orangeGroup = new SimpleMultiAgentGroup();

        foreach (PlayerInfo item in agents) {
            Vector3 pos = item.agent.transform.position;
            Quaternion rot = item.agent.transform.rotation;
            item.startingPos = new Vector3(pos.x, pos.y, pos.z);
            item.startingRot = new Quaternion(rot.x, rot.y, rot.z, rot.w);
            item.rb = item.agent.GetComponent<Rigidbody>();

            if (item.agent.team == Team.Blue) {
                blueGroup.RegisterAgent(item.agent);
            }
            if (item.agent.team == Team.Orange) {
                blueGroup.RegisterAgent(item.agent);
            }
        }

        foreach (BallInfo item in balls) {
            Vector3 pos = item.ball.transform.position;
            Quaternion rot = item.ball.transform.rotation;
            item.startingPos = new Vector3(pos.x, pos.y, pos.z);
            item.startingRot = new Quaternion(rot.x, rot.y, rot.z, rot.w);
            item.rb = item.ball.GetComponent<Rigidbody>();
        }
        ResetScene();
    }

    void FixedUpdate() {
        envSteps++;
        if (envSteps >= maxEnvSteps && maxEnvSteps > 0) {
            blueGroup.GroupEpisodeInterrupted();
            orangeGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    public void AwardPoint(Team teamToReward) {
        if (teamToReward == Team.Blue) {
            if (maxEnvSteps > 0) blueGroup.AddGroupReward(1 - (float)envSteps / maxEnvSteps);
            else blueGroup.AddGroupReward(1);
            orangeGroup.AddGroupReward(-1);
            blueScore++;
            if (blueText) { blueText.text = blueScore.ToString(); }
        }
        else {
            if (maxEnvSteps > 0) orangeGroup.AddGroupReward(1 - (float)envSteps / maxEnvSteps);
            else orangeGroup.AddGroupReward(1);
            blueGroup.AddGroupReward(-1);
            orangeScore++;
            if (orangeText) { orangeText.text = orangeScore.ToString(); }
        }
        Debug.Log("Point awarded to " + teamToReward + ".");
        try {
            blueGroup.EndGroupEpisode();
            orangeGroup.EndGroupEpisode();
        }
        catch {
            Debug.LogError("Can't end episode in testing mode.");
        }
        ResetScene();
    }

    public void ResetScene() {    
        // Reset agents
        foreach (var item in agents) {
            var newStartPos = item.startingPos;
            item.agent.transform.SetPositionAndRotation(newStartPos, item.startingRot);

            item.rb.velocity = Vector3.zero;
            item.rb.angularVelocity = Vector3.zero;
        }

        // Reset balls
        foreach (BallInfo item in balls) {
            item.ball.transform.SetPositionAndRotation(
                item.startingPos, 
                Quaternion.Euler(item.startingRot.x, item.startingRot.y, item.startingRot.z)
            );
            item.rb.velocity = Vector3.zero;
            item.rb.angularVelocity = Vector3.zero;
        }

        envSteps = 0;

        Debug.Log("Scene reset.");
    }
}
