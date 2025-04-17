using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Mobiles;

namespace Server.Achievements
{
    public class TimedAchievementTracker
    {
        public static List<TimedAchievementTracker> Tracker { get; set; }
        public static Timer Timer { get; set; }

        public static void Configure()
        {
            Tracker = new List<TimedAchievementTracker>();
        }

        public static void AddToTracker(TimedAchievementTracker toTrack)
        {
            Tracker.Add(toTrack);

            if (Timer == null)
            {
                Timer = Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), OnTick);
            }
        }

        public static void RemoveTracker(PlayerMobile pm, Achievement achievement)
        {
            var tracker = FindTracker(pm, achievement.Identifier); 

            if (tracker != null)
            {
                RemoveTracker(tracker);
            }
        }

        public static void RemoveTracker(TimedAchievementTracker toTrack)
        {
            Tracker.Remove(toTrack);

            if (Tracker.Count == 0 && Timer != null)
            {
                Timer.Stop();
                Timer = null;
            }
        }

        public static TimedAchievementTracker FindTracker(PlayerMobile pm, Achievement achievement)
        {
            return FindTracker(pm, achievement.Identifier);
        }

        public static TimedAchievementTracker FindTracker(PlayerMobile pm, int id)
        {
            if (Tracker == null || pm == null)
            {
                return null;
            }

            for (int i = 0; i < Tracker.Count; i++)
            {
                var tracker = Tracker[i];

                if (tracker.Player == pm && tracker.ID == id)
                {
                    return tracker;
                }
            }

            return null;
        }

        public static void OnTick()
        {
            for (int i = Tracker.Count - 1; i >= 0; i--)
            {
                var tracker = Tracker[i];

                if (tracker.Expired)
                {
                    RemoveTracker(tracker);
                }
            }
        }

        public static void Save(GenericWriter writer)
        {
            writer.Write(0);

            writer.Write(Tracker.Count);

            for (int i = 0; i < Tracker.Count; i++)
            {
                Tracker[i].Serialize(writer);
            }
        }

        public static void Load(GenericReader reader)
        {
            reader.ReadInt();

            var count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                var tracker = new TimedAchievementTracker(reader);

                if (tracker.Expires > DateTime.UtcNow)
                {
                    AddToTracker(tracker);
                }
            }
        }

        public PlayerMobile Player { get; set; }
        public int ID { get; set; }
        public DateTime Expires { get; set; }
        public List<Type> Data { get; set; }

        public bool Expired { get { return Expires < DateTime.UtcNow; } }

        public TimedAchievementTracker(PlayerMobile pm, Achievement achievement, TimeSpan duration)
        {
            Player = pm;
            ID = achievement.Identifier;
            Expires = DateTime.UtcNow + duration;

            AddToTracker(this);
        }

        public TimedAchievementTracker(GenericReader reader)
        {
            reader.ReadInt(); // version

            Player = reader.ReadMobile<PlayerMobile>();
            ID = reader.ReadInt();
            Expires = reader.ReadDeltaTime();

            if (reader.ReadInt() == 0)
            {
                var count = reader.ReadInt();
                Data = new List<Type>();

                for (int i = 0; i < count; i++)
                {
                    var type = ScriptCompiler.FindTypeByFullName(reader.ReadString());

                    if (type != null)
                    {
                        Data.Add(type);
                    }
                }
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            writer.WriteMobile<PlayerMobile>(Player);
            writer.Write(ID);
            writer.WriteDeltaTime(Expires);

            if (Data != null)
            {
                writer.Write(0);

                writer.Write(Data.Count);

                for (int i = 0; i < Data.Count; i++)
                {
                    writer.Write(Data[i].FullName);
                }
            }
            else
            {
                writer.Write(1);
            }
        }
    }
}
