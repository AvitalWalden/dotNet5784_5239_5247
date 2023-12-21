﻿namespace BlApi;

public interface IMilestone
{
    /// <summary>
    /// creates new milestone
    /// </summary>
    /// <param name="item">the new milestone to be added</param>
    /// <returns></returns>
    public int Create(BO.Milestone item);
    /// <summary>
    /// returns milestone by given id
    /// </summary>
    /// <param name="id">id of milestone that should be returned</param>
    /// <returns></returns>
    public BO.Milestone? Read(int id);
    /// <summary>
    /// Updates milestone details
    /// </summary>
    /// <param name="item">the detailes of the milestone</param>
    public void Update(BO.Milestone item);
}