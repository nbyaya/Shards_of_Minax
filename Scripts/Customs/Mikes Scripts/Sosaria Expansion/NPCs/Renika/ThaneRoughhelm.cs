using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ElementalMeltdownQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Elemental Meltdown"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Thane Roughhelm*, Guard Captain of Renika.\n\n" +
                    "Clad in battered plate, his eyes scan the horizon constantly, even in conversation.\n\n" +
                    "“This isn’t about glory, traveler. This is about **survival**.”\n\n" +
                    "“There’s a **Plutonium Elemental** loose near the Mountain Stronghold. The guards who returned from the last patrol are burning with fever—water’s turning foul.”\n\n" +
                    "“I’ve held back bandits, pirates, even cursed sea-beasts—but this? I’ve never seen anything like it.”\n\n" +
                    "“We can't let it poison our waters. **Slay the Plutonium Elemental**, before the heart of Renika falls to rot.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then pray the winds change, traveler. Because if you won’t stand, we may all fall.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still walks? Then every breath we take here is a gamble. Renika’s time runs short.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it. The elemental’s gone—and the waters run clearer already.\n\n" +
                       "You’ve saved this city more than you know. **Take this**—the bounty is yours, but you’ve earned more than gold today.\n\n" +
                       "You’ve earned Renika’s gratitude.";
            }
        }

        public ElementalMeltdownQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PlutoniumElemental), "Plutonium Elemental", 1));
            AddReward(new BaseReward(typeof(BountyHuntersCache), 1, "Bounty Hunter's Cache"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Elemental Meltdown'!");
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

    public class ThaneRoughhelm : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ElementalMeltdownQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBInnKeeper());
        }

        [Constructable]
        public ThaneRoughhelm()
            : base("the Guard Captain", "Thane Roughhelm")
        {
        }

        public ThaneRoughhelm(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Weathered Tan
            HairItemID = Race.RandomHair(this);
            HairHue = 1109; // Iron-grey
            FacialHairItemID = 0x203E; // Full Beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateHelm() { Hue = 2101, Name = "Roughhelm's Vigil" }); // Dark steel hue
            AddItem(new PlateChest() { Hue = 2101, Name = "Captain's Ironheart Cuirass" });
            AddItem(new PlateArms() { Hue = 2101, Name = "Renikan Guard Bracers" });
            AddItem(new PlateGloves() { Hue = 2101, Name = "Warden's Gauntlets" });
            AddItem(new PlateLegs() { Hue = 2101, Name = "Stormplate Greaves" });
            AddItem(new Cloak() { Hue = 1175, Name = "Cloak of Renika's Flame" }); // Deep crimson
            AddItem(new Boots() { Hue = 1819, Name = "Battleworn Boots" });

            AddItem(new Broadsword() { Hue = 2401, Name = "Steelbane" }); // Slightly radiant hue for distinction

            Backpack backpack = new Backpack();
            backpack.Hue = 0;
            backpack.Name = "Guard Captain's Satchel";
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
