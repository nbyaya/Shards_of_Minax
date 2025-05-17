using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DrainTheBouraWellQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Drain the Boura Well"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Brakk Stonegaze*, the Gargoyle Tamer who tends Yew’s lifeblood—the subterranean streams.\n\n" +
                    "His wings flicker with unease, and his stony eyes narrow.\n\n" +
                    "“You smell that? The water’s turned... foul.”\n\n" +
                    "“I’ve tended the wells of Yew for thirty years, and never have I tasted **ichor** in the stream. This filth flows from the deep, from the roots of Catastrophe itself.”\n\n" +
                    "“There’s a beast down there—**PutridBoura**, bloated and seeping poison into the very veins of the land. My father once drowned in a spring cursed the same way.”\n\n" +
                    "“I won’t lose Yew to rot. **Slay the PutridBoura**, drain its bile, and let the waters run pure again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the roots weep, and Yew wither as the poison spreads.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The stench grows thicker. The Boura still defiles the stream, and Yew’s wells grow darker.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it. The waters... they clear already. Yew owes you more than I can say.\n\n" +
                       "**Take this: StormmarkCrests.** You’ve earned not just coin, but our trust. Should the land turn foul again, I will call upon you.";
            }
        }

        public DrainTheBouraWellQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PutridBoura), "PutridBoura", 1));
            AddReward(new BaseReward(typeof(StormmarkCrests), 1, "StormmarkCrests"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Drain the Boura Well'!");
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

    public class BrakkStonegaze : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DrainTheBouraWellQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer()); // He is a Gargoyle Tamer
        }

        [Constructable]
        public BrakkStonegaze()
            : base("the Gargoyle Tamer", "Brakk Stonegaze")
        {
        }

        public BrakkStonegaze(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 70, 90);

            Female = false;
            Body = 0x2F6; // Gargoyle body
            Hue = 2115; // Stone-gray
            HairItemID = 0; // Bald
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 2106, Name = "Tamer's Carapace" }); // Deep earth brown
            AddItem(new StuddedLegs() { Hue = 2401, Name = "Rootbound Greaves" });
            AddItem(new BoneGloves() { Hue = 2117, Name = "Wellkeeper’s Claws" });
            AddItem(new GargishSash() { Hue = 2212, Name = "Streamwarden's Sash" });
            AddItem(new Boots() { Hue = 1108, Name = "Moss-Stained Boots" });

            AddItem(new QuarterStaff() { Hue = 2110, Name = "Tamer's Staff" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2101;
            backpack.Name = "Waterkeeper's Satchel";
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
