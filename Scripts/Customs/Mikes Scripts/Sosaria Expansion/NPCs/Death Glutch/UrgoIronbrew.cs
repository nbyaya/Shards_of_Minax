using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BrewOfRuinQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Brew of Ruin"; } }

        public override object Description
        {
            get
            {
                return
                    "Urgo Ironbrew, the rugged innkeeper of Death Glutch, wipes a mug with a cloth stained in more than ale.\n\n" +
                    "His eyes narrow, the smell of burnt herbs clinging to his clothes.\n\n" +
                    "“You smell that? That’s not my brew—it’s *poison*. Some damned **Orcish Alchemist** has been cursing my barrels, scaring off patrons, twisting their guts with foul brews.”\n\n" +
                    "“Last night, he waltzed into my inn, shattered a glowing vial on the counter, and said my next batch would be my last. I keep the pieces...”\n\n" +
                    "*He holds out the shattered glass—pulsing faintly.*\n\n" +
                    "“That monster’s holed up in the **Malidor Witches Academy**, brewing doom in the dark. If my alehouse dies, this whole town loses its heart.”\n\n" +
                    "**End his cursed experiments. Slay the Orcish Alchemist before we all choke on ruin.**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then drink something strong, and pray it’s not laced with his spite.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still he brews? My patrons grow pale, their stomachs rebel. We can’t hold out much longer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "He’s done for, then? Bless the bold, friend. My ale will flow clean again.\n\n" +
                       "Take this—**ExodusBarrier**. A relic from better times. May it ward you as you’ve saved my hall.";
            }
        }

        public BrewOfRuinQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(OrcishAlchemist), "Orcish Alchemist", 1));
            AddReward(new BaseReward(typeof(ExodusBarrier), 1, "ExodusBarrier"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Brew of Ruin'!");
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

    public class UrgoIronbrew : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BrewOfRuinQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBInnKeeper());
        }

        [Constructable]
        public UrgoIronbrew()
            : base("the Gruff Innkeeper", "Urgo Ironbrew")
        {
        }

        public UrgoIronbrew(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 80);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1020; // Weathered skin tone
            HairItemID = 8253; // Long hair
            HairHue = 1175; // Deep auburn
            FacialHairItemID = 8267; // Thick beard
            FacialHairHue = 1175;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 1109, Name = "Ale-Stained Tunic" });
            AddItem(new StuddedLegs() { Hue = 1154, Name = "Ember-Flecked Breeches" });
            AddItem(new LeatherGloves() { Hue = 1823, Name = "Brewmaster's Grips" });
            AddItem(new HalfApron() { Hue = 1871, Name = "Ironbrew Apron" });
            AddItem(new ThighBoots() { Hue = 1801, Name = "Dust-Tread Boots" });

            AddItem(new Pitchfork() { Hue = 0, Name = "Keg-Stirrer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Innkeeper's Pack";
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
