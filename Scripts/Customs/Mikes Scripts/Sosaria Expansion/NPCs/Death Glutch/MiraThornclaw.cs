using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SpellClawQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Paws of Power"; } }

        public override object Description
        {
            get
            {
                return
                    "*Mira Thornclaw*, Ranger of the Glutch, leans on her longbow, eyes sharp with vigilance and sorrow.\n\n" +
                    "Her armor is scorched in places, a testament to recent battles.\n\n" +
                    "“There’s a beast loose near the fences of Malidor’s old academy—**the SpellClaw**. No ordinary predator. Its claws rend not just flesh, but the weave of traps I’ve set along our borders.”\n\n" +
                    "“It’s clever. And worse—it’s magic. I lost my hawk, *Ashwing*, to its sorcery. One moment she soared, the next, twisted mid-flight by a shimmer in the air.”\n\n" +
                    "“We can't keep it at bay. **Track it. Kill it.** Or Death Glutch will lose more than a hawk.”";
            }
        }

        public override object Refuse
        {
            get { return "Then you’d best tread carefully near the academy’s fences. It’s out there, and it won’t stop at birds."; }
        }

        public override object Uncomplete
        {
            get { return "Still no word of the beast’s death? My traps remain shredded. The forest doesn’t sleep when the SpellClaw stalks."; }
        }

        public override object Complete
        {
            get
            {
                return "**You’ve slain the SpellClaw?**\n\n" +
                       "*Mira’s eyes soften for the first time, though her face remains weathered by grief.*\n\n" +
                       "“Then Ashwing rests easy, and so will the Glutch.”\n\n" +
                       "“Take this—*BeastmastersCrown*. It’s yours now. May it guide you as well as Ashwing guided me.”";
            }
        }

        public SpellClawQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(SpellClaw), "SpellClaw", 1));
            AddReward(new BaseReward(typeof(BeastmastersCrown), 1, "BeastmastersCrown"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Paws of Power'!");
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

    public class MiraThornclaw : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SpellClawQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRanger());
        }

        [Constructable]
        public MiraThornclaw()
            : base("the Ranger", "Mira Thornclaw")
        {
        }

        public MiraThornclaw(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 80);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 2101; // Wind-worn skin tone
            HairItemID = 0x2044; // Long hair
            HairHue = 1107; // Ash-gray
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 1820, Name = "Thornwood Vest" }); // Deep forest green
            AddItem(new LeatherLegs() { Hue = 1825, Name = "Tracker's Breeches" });
            AddItem(new LeatherGloves() { Hue = 1835, Name = "Falconer's Grasp" });
            AddItem(new HoodedShroudOfShadows() { Hue = 2207, Name = "Cloak of the Glutch" });
            AddItem(new ThighBoots() { Hue = 1816, Name = "Pathfinder's Boots" });
            AddItem(new BodySash() { Hue = 2401, Name = "Ashwing's Band" });

            AddItem(new RangersCrossbow() { Hue = 2412, Name = "Thornclaw's Bow" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1151;
            backpack.Name = "Ranger's Gear";
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
