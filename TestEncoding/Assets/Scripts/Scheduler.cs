using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class Scheduler
{
    private uint _curAllotID = 0;
    private uint _curFrame = 0;
    private List<FrameScheduler> _frameDelegates = new List<FrameScheduler>();
    private List<TimeScheduler> _timeSchedulers = new List<TimeScheduler>();
    private List<OnScheduler> _updateScheduler = new List<OnScheduler>();
    public static readonly Scheduler Instance = new Scheduler();

    private Scheduler()
    {
    }

    public void AddFrame(uint frame, bool loop, OnScheduler callback)
    {
        this._curAllotID++;
        FrameScheduler item = new FrameScheduler {
            ID = this._curAllotID,
            Frame = frame,
            RealFrame = frame + this._curFrame,
            IsLoop = loop,
            Callback = callback
        };
        this._frameDelegates.Add(item);
    }

    public void AddTimer(float time, bool loop, OnScheduler callback)
    {
        this._curAllotID++;
        TimeScheduler item = new TimeScheduler {
            ID = this._curAllotID,
            Time = time,
            RealTime = time + Time.time,
            IsLoop = loop,
            Callback = callback
        };
        this._timeSchedulers.Add(item);
    }

    public void AddUpdator(OnScheduler callback)
    {
        this._updateScheduler.Add(callback);
    }

    ~Scheduler()
    {
        this._frameDelegates.Clear();
        this._frameDelegates = null;
        this._timeSchedulers.Clear();
        this._timeSchedulers = null;
        this._updateScheduler.Clear();
        this._updateScheduler = null;
    }

    public void RemoveFrame(OnScheduler callback)
    {
        for (int i = 0; i < this._frameDelegates.Count; i++)
        {
            FrameScheduler scheduler = this._frameDelegates[i];
            if (scheduler.Callback == callback)
            {
                this._frameDelegates.RemoveAt(i);
                break;
            }
        }
    }

    public void RemoveTimer(OnScheduler callback)
    {
        for (int i = 0; i < this._timeSchedulers.Count; i++)
        {
            TimeScheduler scheduler = this._timeSchedulers[i];
            if (scheduler.Callback == callback)
            {
                this._timeSchedulers.RemoveAt(i);
                break;
            }
        }
    }

    public void RemoveUpdator(OnScheduler callback)
    {
        this._updateScheduler.Remove(callback);
    }

    public void Update()
    {
        this._curFrame++;
        this.UpdateFrameScheduler();
        this.UpdateTimeScheduler();
        this.UpdateUpdator();
    }

    private void UpdateFrameScheduler()
    {
        int index = 0;
        while (index < this._frameDelegates.Count)
        {
            FrameScheduler scheduler = this._frameDelegates[index];
            if (scheduler.RealFrame <= this._curFrame)
            {
                scheduler.Callback();
                if (scheduler.IsLoop)
                {
                    scheduler.RealFrame += scheduler.Frame;
                }
                else
                {
                    this._frameDelegates.RemoveAt(index);
                    continue;
                }
            }
            index++;
        }
    }

    private void UpdateTimeScheduler()
    {
        int index = 0;
        while (index < this._timeSchedulers.Count)
        {
            TimeScheduler scheduler = this._timeSchedulers[index];
            if (scheduler.RealTime <= Time.time)
            {
                scheduler.Callback();
                if (scheduler.IsLoop)
                {
                    scheduler.RealTime += scheduler.Time;
                }
                else
                {
                    this._timeSchedulers.RemoveAt(index);
                    continue;
                }
            }
            index++;
        }
    }

    private void UpdateUpdator()
    {
        if (this._updateScheduler.Count > 0)
        {
            for (int i = 0; i < this._updateScheduler.Count; i++)
            {
                this._updateScheduler[i]();
            }
        }
    }

    private class FrameScheduler
    {
        public Scheduler.OnScheduler Callback;
        public uint Frame;
        public uint ID;
        public bool IsLoop;
        public uint RealFrame;
    }

    public delegate void OnScheduler();

    private class TimeScheduler
    {
        public Scheduler.OnScheduler Callback;
        public uint ID;
        public bool IsLoop;
        public float RealTime;
        public float Time;
    }
}

