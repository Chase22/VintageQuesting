# Vintage Questing

## Eleemnts

### Quest
Quests are a container with a title, description, objectives, pre and post actions. Quests are tracked per player and may or may not be repeatable.

### Task
A task is a single objective to be complected. Tasks may either be completed based on a state of the player or world, like having a specific item or being in a specific location or by triggering an event like killing a specific creature a number of times. Tasks are tracked player specific.

### Pre/Post Actions
Pre and post actions are executed upon a quest being started or completed. These could include giving items, spawning entities or teleporting the player.

## Task Types

### Boolean Task
The simplest type of task stores a single boolean value. This type never completes by itself. It is triggered via some external code and can be used by other mods to easily integrate into the quest system.

### Counter Task
A counter task stores a single integer value. This type of task is completed when the counter reaches a specific value. The counter can be incremented or decremented by external code.

### Item Collection Task
An item collection task is completed when the player has a specific number of a specific item in their inventory. The task is configured with an item id and a target amount. It can be configured to require the items to be submitted to complete the task.

### Location Task
A location task is completed when the player is in a specific location. The task is configured with a location id and a radius.

## Pre/Post Actions

### Give Item Action
A give item action gives the player a specific item when the action is executed. The action is configured with an item id and an amount.

### Teleport Action
A teleport action teleports the player to a specific location when the action is executed. The action is configured with the coordinates of the location.

### Activate Quest Action
This action activates a different quest for the Player. This is useful for chaining quests. Using it as a pre-action is probably not desired.

## Model

### Quest
- id: string -> Unique identifier. Colon (:) separated. Recommended format: modid:questlineId:questId
- title: string -> The title of the quest.
- description: string -> The description of the quest.
- objectives: list of Task -> The objectives of the quest.
- pre_actions: list of Pre Action -> The actions to execute when the quest is started.
- post_actions: list of Post Action -> The actions to execute when the quest is completed.

### Task
- id: string -> Unique identifier. Colon (:) separated. Recommended format: modid:questlineId:questId:taskId
- type: string -> The type of the task.
- data: dict -> The data specific to the task type.

### Pre/Post Action
- id: string -> Unique identifier. Colon (:) separated. Recommended format: modid:questlineId:questId:actionId
- type: string -> The type of the action.
- data: dict -> The data specific to the action type.