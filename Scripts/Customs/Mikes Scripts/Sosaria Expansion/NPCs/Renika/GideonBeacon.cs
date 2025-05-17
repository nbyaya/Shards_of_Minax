using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ElementalEclipseQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Elemental Eclipse"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Gideon Beacon*, the steadfast Lighthouse Keeper of Renika.\n\n" +
                    "His figure is stooped yet resolute, wrapped in sea-weathered robes, eyes reflecting both moonlight and burdened wisdom.\n\n" +
                    "“The light dims, traveler. Not by oil or storm, but by **iron breath**. Ships crash, lives are lost… and I can no longer hold back the dark alone.”\n\n" +
                    "“From the **Mountain Stronghold**, an **Ironbound Elemental** emerged. Its breath taints the winds, fractures the lenses of my beacon. The seas grow treacherous without their guiding light.”\n\n" +
                    "“Slay this creature, restore the beacon’s clarity. Let the light shine unbroken once more, or Renika will drown in shadowed tides.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the light may flicker, but know this: every shipwreck will weigh upon your soul.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the iron fog clouds the lens? Time thins like glass under strain. Ships vanish in the mist.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The light returns! The seas are safe once more, thanks to your courage.\n\n" +
                       "Take this **GoldRushRelicChest**. A piece of the past, and now, your reward.\n\n" +
                       "May the beacon always shine for you, as you have shone for us.";
            }
        }

        public ElementalEclipseQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IronboundElemental), "Ironbound Elemental", 1));
            AddReward(new BaseReward(typeof(GoldRushRelicChest), 1, "GoldRushRelicChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Elemental Eclipse'!");
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

    public class GideonBeacon : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ElementalEclipseQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this)); // Lighthouse keepers often have nautical knowledge and tools
        }

        [Constructable]
        public GideonBeacon()
            : base("the Lighthouse Keeper", "Gideon Beacon")
        {
        }

        public GideonBeacon(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 70, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Pale sea-weathered skin tone
            HairItemID = 0x2049; // Long Hair
            HairHue = 1109; // Grey-blue sea mist
            FacialHairItemID = 0x204B; // Full Beard
            FacialHairHue = 1109; // Matching misty hue
        }

        public override void InitOutfit()
        {
            AddItem(new Cloak() { Hue = 1119, Name = "Beacon's Cloak" }); // Deep sea blue
            AddItem(new Robe() { Hue = 1153, Name = "Salt-Worn Robe" }); // Pale lighthouse-white
            AddItem(new LeatherGloves() { Hue = 2401, Name = "Lens-Polisher's Gloves" });
            AddItem(new ThighBoots() { Hue = 1102, Name = "Wavewalkers" });
            AddItem(new SkullCap() { Hue = 1118, Name = "Keeper's Cap" });

            AddItem(new ShepherdsCrook() { Hue = 1154, Name = "Lighthouse Staff" }); // Symbolic, for guiding the light

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Beacon Keeper's Satchel";
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
