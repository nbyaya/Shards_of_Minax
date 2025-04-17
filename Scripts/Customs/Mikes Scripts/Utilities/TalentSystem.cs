using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Mobiles
{
    public enum TalentID
    {
        AncientKnowledge = 1
    }

    public class Talent
    {
        public TalentID ID { get; private set; }
        public int Points { get; set; }

        public Talent(TalentID id)
        {
            ID = id;
        }

        public Talent(GenericReader reader)
        {
            Deserialize(reader);
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0); // version
            writer.Write((int)ID);
            writer.Write(Points);
        }

        public void Deserialize(GenericReader reader)
        {
            reader.ReadInt(); // version
            ID = (TalentID)reader.ReadInt();
            Points = reader.ReadInt();
        }
    }

    public class TalentProfile
    {
        public PlayerMobile Owner { get; private set; }
        public Dictionary<TalentID, Talent> Talents { get; private set; } = new Dictionary<TalentID, Talent>();

        // NEW: XP and Level properties
        public int XP { get; set; }
        public int Level { get; set; }

        public TalentProfile(PlayerMobile owner)
        {
            Owner = owner;
            XP = 0;
            Level = 1;
        }

        public TalentProfile(GenericReader reader)
        {
            Deserialize(reader);
        }

		public void Serialize(GenericWriter writer)
		{
			writer.Write(1); // profile version (1)
			// (You may not need to write Owner if you use a unique key elsewhere)
			writer.Write(Talents.Count);
			foreach (var talent in Talents.Values)
			{
				talent.Serialize(writer);
			}
			writer.Write(XP);
			writer.Write(Level);
		}


		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();
			int talentCount = reader.ReadInt();
			while (--talentCount >= 0)
			{
				var talent = new Talent(reader);
				if (Enum.IsDefined(typeof(TalentID), talent.ID))
				{
					Talents[talent.ID] = talent;
				}
			}
			if (version >= 1)
			{
				XP = reader.ReadInt();
				Level = reader.ReadInt();
			}
		}

    }

    public static class Talents
    {
        public static Dictionary<PlayerMobile, TalentProfile> Profiles { get; private set; } = new Dictionary<PlayerMobile, TalentProfile>();

        public static void Configure()
        {
            EventSink.WorldSave += e => Persistence.Serialize(@"Saves\\Talents\\Profiles.bin", Save);
            EventSink.WorldLoad += () => Persistence.Deserialize(@"Saves\\Talents\\Profiles.bin", Load);
        }

        public static void Save(GenericWriter writer)
        {
            writer.Write(0); // version
            writer.Write(Profiles.Count);
            foreach (var profile in Profiles.Values)
            {
                profile.Serialize(writer);
            }
        }

        public static void Load(GenericReader reader)
        {
            reader.ReadInt(); // version
            var profileCount = reader.ReadInt();
            while (--profileCount >= 0)
            {
                var profile = new TalentProfile(reader);
                if (profile.Owner != null)
                {
                    Profiles[profile.Owner] = profile;
                }
            }
        }

        public static TalentProfile AcquireTalents(this PlayerMobile player)
        {
            if (!Profiles.TryGetValue(player, out var profile))
            {
                Profiles[player] = profile = new TalentProfile(player);
            }
            return profile;
        }

        // NEW: Helper to calculate cumulative XP threshold for a given level.
        // In this example, leveling from level 1 to 2 requires 1000 XP,
        // from level 2 to 3 requires an additional 1500 (total 2500), etc.
		public static int GetXPThresholdForLevel(int level)
		{
			const int baseXP = 50; // XP needed for level 2
			if (level <= 1)
				return 0;

			double totalXP = 0;
			for (int i = 2; i <= level; i++)
			{
				double growthFactor = 1.2 + (i - 2) * 0.05; // Dynamic scaling
				totalXP += baseXP * Math.Pow(growthFactor, i - 2);
			}

			return (int)totalXP;
		}


    }

    // (TalentDeed remains unchanged below...)
    public class TalentDeed : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public TalentID Talent { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Points { get; set; }

        [Constructable]
        public TalentDeed() : this(TalentID.AncientKnowledge)
        {
        }

        [Constructable]
        public TalentDeed(TalentID talent) : this(talent, 1)
        {
        }

        [Constructable]
        public TalentDeed(TalentID talent, int points) : base(0x14F0)
        {
            Talent = talent;
            Points = points;
        }

        public TalentDeed(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (!profile.Talents.TryGetValue(Talent, out var talent))
                {
                    profile.Talents[Talent] = talent = new Talent(Talent);
                }
                talent.Points += Points;

                int totalTalentPoints = profile.Talents.Values.Sum(t => t.Points);
                player.SendMessage($"You gained {Points:N0} in {Talent}!");
                player.SendMessage($"Your total talent points: {totalTalentPoints:N0}");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write((int)Talent);
            writer.Write(Points);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
            Talent = (TalentID)reader.ReadInt();
            Points = reader.ReadInt();
        }
    }
}
