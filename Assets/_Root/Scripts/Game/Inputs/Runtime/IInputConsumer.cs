﻿namespace _Root.Scripts.Game.Inputs.Runtime
{
    public interface IInputConsumer
    {
        public bool IsInputEnabled { get; set; }
        public bool HasInputThisFrame { get; set; }
    }
}