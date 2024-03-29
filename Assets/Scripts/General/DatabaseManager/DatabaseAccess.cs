using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DatabaseAccess : MonoBehaviour
{
    [SerializeField] private float timeToRefresh;

    // Make changes from editor only
    [field: SerializeField] public int Acorns { get; private set; }
    [field: SerializeField] public int LotteryTickets { get; private set; }
    [field: SerializeField] public int LevelsCount { get; private set; } = 5;
    [field: SerializeField] public int[] LevelsTime { get; private set; }

    [SerializeField] private bool ResetPlayerPrefs;

    private bool coroutineStarted = false;

    public DatabaseAccess()
    {
        LevelsTime = new int[LevelsCount];
    }

    private void Start()
    {
        Database.SetAccessTimeArea("level0", -1);

        if (!coroutineStarted)
        {
            coroutineStarted = true;
            StartCoroutine(IUpdateDatabase());
        }
    }

    private void OnValidate()
    {
        Database.IncrAcorns(Acorns);
        Database.IncrLotteryTickets(LotteryTickets);

        for (int i = 0; i < LevelsCount; i++)
        {
            Database.IncrAccessTimeArea($"level{i}", LevelsTime[i]);
            LevelsTime[i] = 0;
        }

        if (ResetPlayerPrefs) { PlayerPrefs.DeleteAll(); ResetPlayerPrefs = false; }
    }

    IEnumerator IUpdateDatabase()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToRefresh);
            UpdateAcorns();
            UpdateLotteryTickets();
            UpdateAccessTimes();
            UpdateTimeSinceStarted();
        }
    }

    #region Acorns
    public void UpdateAcorns()
    {
        Acorns = 0;
    }
    #endregion

    #region Lottery Tickets
    public void UpdateLotteryTickets()
    {
        LotteryTickets = 0;
    }
    #endregion

    #region Access To Area
    public void UpdateAccessTimes()
    {
        for (int i = 0; i < LevelsCount; i++)
        {
            Database.IncrAccessTimeArea($"level{i}", -(int)timeToRefresh);
        }

    }
    #endregion

    #region Keep Track of Time
    public void UpdateTimeSinceStarted()
    {
        Database.SetLastConnection((int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds);
    }
    #endregion
}
