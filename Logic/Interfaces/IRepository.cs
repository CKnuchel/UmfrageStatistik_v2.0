﻿namespace Logic.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
}