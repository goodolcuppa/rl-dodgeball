# Reinforcement Learning Dodgeball
Reinforcement Learning agent trained in Unity using the Unity ML Agents package.

# Overview
[![Showcase](http://img.youtube.com/vi/k3DSItfK2MA/0.jpg)](http://www.youtube.com/watch?v=k3DSItfK2MA)

- 1v1 Dodgeball arena
- 2v2 Dodgeball arena
- Model trained using MA-POCA

# Environment
The arena is set up with 3 balls in the center, an agent on each side, and a barrier dividing it so that only balls can pass through.
<p align="center">
  <img src="https://github.com/goodolcuppa/rldodgeball/assets/38227160/7b310d37-749c-4e45-b814-384328f7d8df" height="200">
  <img src="https://github.com/goodolcuppa/rldodgeball/assets/38227160/121f1df0-dd9e-4ed1-b07b-1a45dcdbb989" height="200">
</p>

# Actions
The agent can move along its X and Z axes, and rotate about its Y axis using 3 continuous values.

# Observations
The agent perceives the environment using the `RayPerceptionSensor` component. These sensors were used to detect walls, balls, agents, and the barrier.

![image](https://github.com/goodolcuppa/rldodgeball/assets/38227160/c56c5234-4cda-4235-9e0b-9487ade277f1)

# Reward Signals
To encourage beneficial behaviour, the agent is rewarded or penalised when certain conditions are met:

Reward | Value
--- | ---
Agent touching a ball | 0.005
Agent hitting a ball | 0.01
Scoring a point | 0-1 (variable)
Losing a point | -1
Hitting the walls or barrier | -0.005

# Training
The agent was trained for 5,000,000 steps using the MA-POCA algorithm, stored under the run-id "dodgeball_mapoca" in results.
