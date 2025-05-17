using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GarrikUnreadyQuest : BaseQuest
    {
        public override object Title { get { return "Not Today, Hero"; } }

        public override object Description
        {
            get
            {
                return
                    "*Garrik looks up, sweat beading on his brow despite the chill.*\n\n" +
                    "\"Okay... okay, I admit it. I thought I was ready. I was gonna be a hero, you know? But as soon as I saw what’s inside that dungeon... no way. Look, just help me get back to town before something big, ugly, and angry finds me here. I’ll make it worth your while. Some... friends of mine left a crate behind. You can have it.\"";
            }
        }

        public override object Refuse { get { return "*Garrik shrinks back.* \"Oh... yeah, no, sure, I’ll just... hide here forever.\""; } }
        public override object Uncomplete { get { return "*Garrik fidgets nervously.* \"We’re not safe yet! Please, keep moving!\""; } }

        public GarrikUnreadyQuest() : base()
        {
            AddObjective(new EscortObjective("at or near a Dungeon Entrance"));
            AddReward(new BaseReward(typeof(SmugglersCrate), "SmugglersCrate – Contains random valuable items smuggled from the depths."));
        }

        public override void GiveRewards()
        {
            base.GiveRewards();
            Owner.SendMessage("*Garrik grins sheepishly as he hands over a crate.* \"Hey, thanks! Maybe next time I’ll, uh, try fishing instead of heroics. Take this—found it near the dungeon, but I don’t need it anymore.\"", null, 0x59B);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class GarrikUnreadyEscort : BaseEscort
    {
        public override Type[] Quests { get { return new Type[] { typeof(GarrikUnreadyQuest) }; } }
        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBVagabond()); // Closest to a failed adventurer.
        }

        private DateTime m_NextTalkTime;

        [Constructable]
        public GarrikUnreadyEscort() : base()
        {
            Name = "Garrik";
            Title = "the Unready";
            NameHue = 0x21; // Pale Yellow, to give a sense of nervousness
        }

		public GarrikUnreadyEscort(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 40, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1005; // Slightly pale
            HairItemID = 0x203B; // Short Hair
            HairHue = 1109; // Brown
            FacialHairItemID = 0x2040; // Short Beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherCap() { Hue = 1157, Name = "Helm of Bold Regrets" }); // Slightly rusty red, comically dented
            AddItem(new LeatherChest() { Hue = 2413, Name = "Chestplate of False Courage" }); // Faded gold
            AddItem(new StuddedArms() { Hue = 2101, Name = "Bracers of Hesitation" }); // Dull bronze
            AddItem(new LeatherLegs() { Hue = 2120, Name = "Greaves of Retreat" }); // Muddy green
            AddItem(new Sandals() { Hue = 2301, Name = "Wanderer's Soles" }); // Worn brown
            AddItem(new Cloak() { Hue = 1175, Name = "Cape of Cowardice" }); // Faintly ragged

            AddItem(new Dagger() { Hue = 1150, Name = "Pointy Stick of Mild Defense" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Garrik's Emergency Pack";
            AddItem(backpack);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTalkTime && this.Controlled)
            {
                if (Utility.RandomDouble() < 0.15)
                {
                    string[] lines = new string[]
                    {
                        "*Garrik mutters.* I swear I saw it move... something’s watching us.",
                        "*He glances nervously back.* Do you hear that? No? ...Exactly.",
                        "*Garrik clutches his dagger.* I knew I should’ve stayed at the tavern...",
                        "*He sighs.* This was supposed to be my big break...",
                        "*Garrik tries to smile.* You’re doing great, by the way. Really brave. Not like me.",
                        "*He fumbles with his pack.* Maybe I can bribe whatever’s out there... right?",
                        "*Garrik stumbles.* Rocks! Just rocks... whew..."
                    };

                    Say(lines[Utility.Random(lines.Length)]);
                    m_NextTalkTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextTalkTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextTalkTime = reader.ReadDateTime();
        }
    }
}
