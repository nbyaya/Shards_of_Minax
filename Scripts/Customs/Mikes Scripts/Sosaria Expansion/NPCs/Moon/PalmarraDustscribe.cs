using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EntwinedPerilQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Entwined Peril"; } }

        public override object Description
        {
            get
            {
                return
                    "Ah, the stars align that you would arrive now, traveler.\n\n" +
                    "I am Palmarra Dustscribe, keeper of the Archive of Silver Sands, where ancient wisdom slumbers. Yet, a perilous vine—cursed and hungry—strangles our most precious scrolls. Its spores consume ink, erasing truths that cannot be replaced.\n\n" +
                    "**Slay the Cursed Vine** entwined in the deepest chamber of the Archive, where starlight cannot reach. Beware, for the spores glow faintly in the dark, revealing runes not meant for mortal eyes.";
            }
        }

        public override object Refuse { get { return "Then the dust shall claim us all, along with forgotten truths."; } }

        public override object Uncomplete { get { return "The vine still feeds upon our history. You must return and finish the task."; } }

        public override object Complete { get { return "You have severed the vine's grip on our past. Take this relic, and let the lineage of your deeds roar through time."; } }

        public EntwinedPerilQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CursedVine), "CursedVine", 1));

            AddReward(new BaseReward(typeof(ChestOfTheRoaringLineage), 1, "Chest of the Roaring Lineage"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Entwined Peril'!");
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

    public class PalmarraDustscribe : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EntwinedPerilQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFortuneTeller()); 
        }

        [Constructable]
        public PalmarraDustscribe() : base("Palmarra Dustscribe", "Scrollkeeper")
        {
            Title = "Scrollkeeper";
			Body = 0x191; // Male human
			Female = true;

            // Outfit
            AddItem(new Robe { Hue = 1153, Name = "Stargazer's Robe" }); // Deep midnight blue
            AddItem(new HoodedShroudOfShadows { Hue = 2403, Name = "Celestial Hood" }); // Pale, almost silver-white
            AddItem(new Sandals { Hue = 2117, Name = "Dustwalker Sandals" }); // Soft grey, muted
            AddItem(new BodySash { Hue = 1175, Name = "Sash of Forgotten Truths" }); // A magical teal hue

            // Gear
            AddItem(new MagicWand { Hue = 1165, Name = "Inkweaver's Wand" }); // Glow of soft starlight, linked to archives

            SetStr(60, 70);
            SetDex(75, 85);
            SetInt(95, 105);

            SetDamage(4, 8);
            SetHits(180, 200);
        }

        public PalmarraDustscribe(Serial serial) : base(serial) { }

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
