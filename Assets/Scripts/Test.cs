using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class Test : Agent {

    float amount;

    void Start() {
        // Called at the start of the simulation
    }

    public override void OnEpisodeBegin() {
        // Called at the start of each episode
    }

    public override void CollectObservations(VectorSensor sensor) {
        // Called at each time step to collect observational input
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        // Called at each time step to handle manual input
    }

    public override void OnActionReceived(ActionBuffers actions) {
        // Called at each time step to handle all input
    }

    void OnCollisionEnter(Collision collision) {
        // Called when two objects collide
    }

}
