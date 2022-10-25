using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    #region Fields

    static List<Hole> ballDestroyInvokers = new();
    static List<UnityAction<GameObject>> ballDestroyListeners = new();

    static List<BallsSpawner> checkBallNumberInvokers = new();
    static List<UnityAction<int, int>> checkBallNumberListeners = new();

    #endregion
    #region Ball Destroy support

    /// <summary>
    /// Adds the given script as a ball died invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddBallDestroyInvoker(Hole invoker)
    {
        // add invoker to list and add all listeners to invoker
        ballDestroyInvokers.Add(invoker);
        foreach (UnityAction<GameObject> listener in ballDestroyListeners) invoker.AddBallDestroyListener(listener);
    }

    /// <summary>
    /// Adds the given method as a ball died listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddBallDestroyListener(UnityAction<GameObject> listener)
    {
        // add listener to list and to all invokers
        ballDestroyListeners.Add(listener);
        foreach (Hole invoker in ballDestroyInvokers) invoker.AddBallDestroyListener(listener);
    }

    /// <summary>
    /// Remove the given script as a ball died invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void RemoveBallDestroyInvoker(Hole invoker) { ballDestroyInvokers.Remove(invoker); }

    #endregion

    #region Check Ball Number support

    /// <summary>
    /// Adds the given script as a ball died invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void AddCheckBallNumberInvoker(BallsSpawner invoker)
    {
        // add invoker to list and add all listeners to invoker
        checkBallNumberInvokers.Add(invoker);
        foreach (UnityAction<int, int> listener in checkBallNumberListeners) invoker.AddCheckBallNumberListener(listener);
    }

    /// <summary>
    /// Adds the given method as a ball died listener
    /// </summary>
    /// <param name="listener">listener</param>
    public static void AddCheckBallNumberListener(UnityAction<int, int> listener)
    {
        // add listener to list and to all invokers
        checkBallNumberListeners.Add(listener);
        foreach (BallsSpawner invoker in checkBallNumberInvokers) invoker.AddCheckBallNumberListener(listener);
    }

    /// <summary>
    /// Remove the given script as a ball died invoker
    /// </summary>
    /// <param name="invoker">invoker</param>
    public static void RemoveCheckBallNumberInvoker(BallsSpawner invoker) { checkBallNumberInvokers.Remove(invoker); }

    #endregion
}
