using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MummysWakeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Mummy’s Wake"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Alys Coalflare*, Firewatcher of Devil Guard’s smoldering mines.\n\n" +
                    "Her gaze is sharp, her armor singed by years near the forges, and yet she seems burdened.\n\n" +
                    "“I’ve seen too much flame in these tunnels. But last fortnight, the fire whispered something different.”\n\n" +
                    "“Deep in the coal seams, we cracked open a forgotten tomb—**sarcophagi**, etched with runes, spilling ash that moves on its own.”\n\n" +
                    "“Something rose from them. An ancient **Entombed Mummy**, cursed and bound to this place. Its presence flares every fortnight. My watch holds the fire—but not for long.”\n\n" +
                    "“Slay this thing. Seal it. Before the mines become its pyre, and us with them.”";
            }
        }

        public override object Refuse
        {
            get { return "Then hope the flame holds. But curses don’t burn clean forever."; }
        }

        public override object Uncomplete
        {
            get { return "Still it stirs? I can feel the heat twist... The flame’s losing its hold."; }
        }

        public override object Complete
        {
            get
            {
                return
                    "The flame breathes easier now. The tomb is silent.\n\n" +
                    "You’ve done more than kill—it’s **contained**. The coal will burn again, untainted.\n\n" +
                    "Take this: *SakurawindRobe*. A gift from the East, once given to firewalkers. It’ll guard your spirit like you’ve guarded ours.";
            }
        }

        public MummysWakeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EntombedMummy), "Entombed Mummy", 1));
            AddReward(new BaseReward(typeof(SakurawindRobe), 1, "SakurawindRobe"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Mummy’s Wake'!");
            Owner.PlaySound(CompleteSound);
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

    public class AlysCoalflare : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MummysWakeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        [Constructable]
        public AlysCoalflare()
            : base("the Firewatcher", "Alys Coalflare")
        {
        }

        public AlysCoalflare(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Slightly ashen skin tone
            HairItemID = 0x203B; // Short hair
            HairHue = 1358; // Flame-red
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2309, Name = "Ashenplate Cuirass" });
            AddItem(new StuddedLegs() { Hue = 2401, Name = "Charred Greaves" });
            AddItem(new LeatherGloves() { Hue = 1899, Name = "Sootbound Mitts" });
            AddItem(new StuddedGorget() { Hue = 1810, Name = "Coalflare Gorget" });
            AddItem(new Cloak() { Hue = 1358, Name = "Flamewatch Cloak" });
            AddItem(new Boots() { Hue = 1815, Name = "Cinderboots" });

            AddItem(new WarAxe() { Hue = 2405, Name = "Embercleave" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Firewatch Pack";
            AddItem(backpack);
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
}
