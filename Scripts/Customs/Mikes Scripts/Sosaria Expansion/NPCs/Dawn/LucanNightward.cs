using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PowdersOfRuinQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Powders of Ruin"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Lucan Nightward*, the brooding alchemist of Dawn, hunched over shattered glass and scorched wood.\n\n" +
                    "His eyes, rimmed in dark circles, glance up at you, flashing with urgency and resentment.\n\n" +
                    "“The lab... breached. My reagents stolen. A Goblin Cultist, foul and clever, burst through with bombs that hissed and burned green.”\n\n" +
                    "“He defiled my work, twisted open locks meant for no one’s hand but mine.”\n\n" +
                    "**“Find him in the Doom Dungeon.”**\n\n" +
                    "**“Slay him. Bring ruin to the one who dared steal from Nightward.”**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then begone, coward. I’ll brew my revenge without you.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“He lives? The goblin dances in my mind like acid in a wound. Return only when his screams are real.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“Good... the fumes of his death reach even here.”\n\n" +
                       "“Take this—*Arcspike.* A sliver of pure intent, honed in shadow.”\n\n" +
                       "**“May you wield it as I wield rage.”**";
            }
        }

        public PowdersOfRuinQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GoblinCultist), "Goblin Cultist", 1));
            AddReward(new BaseReward(typeof(Arcspike), 1, "Arcspike"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Powders of Ruin'!");
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

    public class LucanNightward : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PowdersOfRuinQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public LucanNightward()
            : base("the Shadowed Alchemist", "Lucan Nightward")
        {
        }

        public LucanNightward(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 95);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 0x83EA; // Pale
            HairItemID = 0x203C; // Long Hair
            HairHue = 1153; // Midnight Black
            FacialHairItemID = 0x2041; // Goatee
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1157, Name = "Nightward's Robe" }); // Dark Violet-Black
            AddItem(new HalfApron() { Hue = 1154, Name = "Alchemical Apron" }); // Deep Green
            AddItem(new Sandals() { Hue = 1109, Name = "Ashen Sandals" }); // Grey

            AddItem(new LeatherGloves() { Hue = 2405, Name = "Powder-Stained Gloves" }); // Faded Grey
            AddItem(new WizardsHat() { Hue = 1175, Name = "Hat of Fumes" }); // Smoke-blue

            AddItem(new Scepter() { Hue = 1170, Name = "Infuser's Rod" }); // Alchemical Rod
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
