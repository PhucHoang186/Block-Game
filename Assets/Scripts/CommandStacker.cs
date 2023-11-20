using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CommandStacker : MonoBehaviour
{
    [SerializeField] float executeDelayTime = 3f;
    Queue<IAction> commands = new();
    List<ActionInfo> commandInfos = new();
    private Action onExecuteOneCommand;
    private Action onExecuteAllCommands;

    public void InitAction(Action onExecuteOneCommand, Action onExecuteAllCommands)
    {
        this.onExecuteOneCommand = onExecuteOneCommand;
        this.onExecuteAllCommands = onExecuteAllCommands;
    }

    public void ExecuteCommands()
    {
        StartCoroutine(CorExecuteCommands());
    }

    private IEnumerator CorExecuteCommands()
    {
        GenerateCommandQueue();
        if (commands.Count == 0)
            yield break;

        var player = EnvironmentManager.Instance.mapSpawner.Player.gameObject;
        while (commands.Count > 0)
        {
            IAction command = commands.Dequeue();
            command.ExecuteAction(player);
            yield return new WaitForSeconds(executeDelayTime);
            onExecuteOneCommand?.Invoke();
        }
        onExecuteAllCommands?.Invoke();
    }

    public void ResetCommands()
    {
        commandInfos.Clear();
        commands.Clear();
    }

    public void AddActionInfo(ActionInfo actionInfo)
    {
        commandInfos.Add(actionInfo);
    }

    public void UpdateActionInfo(int index, int actionCount)
    {
        if (index < commandInfos.Count && index > -1)
        {
            commandInfos[index].actionCount = actionCount; // update action count
        }
    }

    private void GenerateCommandQueue()
    {
        commands.Clear();
        foreach (var commandInfo in commandInfos)
        {
            for (int i = 0; i < commandInfo.actionCount; i++)
            {
                commands.Enqueue(commandInfo.actionData.actionModifier);
            }
        }
    }
}
