using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FallenKnightQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Fallen Knight"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Captain Sorin Denning*, Guard Captain of Grey's coastal watch.\n\n" +
                    "His armor, though polished, bears scars of a battle long past—eyes sharp, but shadowed.\n\n" +
                    "“I was there when the *Fallen Knight of Exodus* rose. My men... they didn’t stand a chance. I sealed it with my own hands, but not before it claimed something of mine—our standard, soaked in their blood.”\n\n" +
                    "“It taunts me still. The banner’s curse sings on the winds, promising false glories to those who dare approach.”\n\n" +
                    "**Slay the Fallen Knight of Exodus**, and return our banner. Lay their spirits to rest—and mine with them.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the banner will remain in the hands of a monster. And I, a captain with no cause, no closure.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? Then the banner mocks me still, and my men do not sleep.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve faced it? And returned...\n\n" +
                       "**The banner...** It shimmers still, but no longer with unspoken promises. Only memories.\n\n" +
                       "Take this. A candlestick from our quarters. It burned through our darkest nights—and may it light your path.";
            }
        }

        public FallenKnightQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FallenKnightOfExodus), "Fallen Knight of Exodus", 1));
            AddReward(new BaseReward(typeof(CandleStick), 1, "CandleStick"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Fallen Knight'!");
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

    public class CaptainSorinDenning : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FallenKnightQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSwordWeapon());
        }

        [Constructable]
        public CaptainSorinDenning()
            : base("the Guard Captain", "Captain Sorin Denning")
        {
        }

        public CaptainSorinDenning(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(95, 90, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Weathered sailor's tan
            HairItemID = 8251; // Long hair
            HairHue = 1107; // Dark sea-gray
            FacialHairItemID = 8267; // Trimmed beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1157, Name = "Tide-Worn Breastplate" }); // Darkened steel
            AddItem(new StuddedLegs() { Hue = 1109, Name = "Mariner's Guard Greaves" });
            AddItem(new PlateArms() { Hue = 1157, Name = "Salt-Rusted Pauldrons" });
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Deckhand's Grips" });
            AddItem(new TricorneHat() { Hue = 1153, Name = "Captain’s Crest" });
            AddItem(new Cloak() { Hue = 2407, Name = "Grey Watch Cloak" });
            AddItem(new Boots() { Hue = 1102, Name = "Tidewalker Boots" });

            AddItem(new Cutlass() { Hue = 1155, Name = "Honor's Edge" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Captain's Satchel";
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
