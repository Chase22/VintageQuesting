## Json Definitions
Quests are defined in the `quests` directory in the assets folder. It's a key-value pair where the key is the full quest id and the value is an object containing quest details.

Field | Description
---- | ----
Id | The Id again, obsolete and will be removed
Name | The name of the quest
Description | A longer text for the quest.
Tasks | An array of tasks that need to be completed for the quest.

The tasks have the following fields:

Field | Description
---- | ----
Type | The type of the task which is used to connect a task to its implementation
Name | The name of the task
Data | An object or field of data defined by the task type. This is used to pass data to the task implementation.

## Implementation
The `api` namespace contains the public facing API of the framework. The QuestDefintion.cs defines the Data model of the quests.json file. 
Task.cs contains the base classes for tasks as well as the TaskType class attribute used to connect a task and its implementation.

QuestManager will be the central class for managing quests, offering methods to start, query, complete or cancel quests.

## Tasks
The idea behind tasks is to have a simple way of defining a task type. There will be 2 general types of tasks: Event-driven and Polled. 
Event-driven tasks register themselves with the games event system to update their own status when a player causes a specific event.
Polled tasks will have to be checked regularly to implement things like Inventory Content or a players position. The QuestManager will keep track of this.

Task classes should be stateless aside from the data they are given. A single task may be called multiple times with different players to check for their individual status.