using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PhantomDiplomacyQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Phantom Diplomacy"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Thorne Veilwalker*, the enigmatic Diviner of Castle British, draped in garments that shimmer like morning mist over a mirror.\n\n" +
                    "He gazes beyond you, eyes veiled in a soft glow.\n\n" +
                    "“Once, the ethereal were guests at court. I knew their ways, their voices—*their desires.* But something has twisted one of them. Now, it siphons life from those who wander the old Vault." + "\n\n" +
                    "**The Ethereal Citizen** stalks the Preservation Vault 44, mocking what it once was—a dignitary now lost to hunger and madness.”\n\n" +
                    "“You must end it. Not just for our safety, but for the memory of what once was. **Slay the Ethereal Citizen** and bring peace to both worlds.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the Vault grow darker still, and the echoes of our past consume those who tread too close.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Vault still weeps. Its air trembles with stolen breaths. You *must* stop the Ethereal Citizen before it claims more souls.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? Then the Vault may rest, for now.\n\n" +
                       "You’ve slain more than a monster—you’ve severed a tether to a forgotten diplomacy.\n\n" +
                       "Take this: *SoftstepsDelight*. May your tread be light, and your presence untroubled by those who linger between worlds.";
            }
        }

        public PhantomDiplomacyQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EtheralCitizen), "Ethereal Citizen", 1));
            AddReward(new BaseReward(typeof(SoftstepsDelight), 1, "SoftstepsDelight"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Phantom Diplomacy'!");
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

    public class ThorneVeilwalker : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PhantomDiplomacyQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic()); // As a Diviner, he deals in mystical items.
        }

        [Constructable]
        public ThorneVeilwalker()
            : base("the Diviner", "Thorne Veilwalker")
        {
        }

        public ThorneVeilwalker(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Pale spectral hue
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Moonlit silver
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1150, Name = "Veil-Touched Robe" }); // Ethereal gray
            AddItem(new Cloak() { Hue = 2101, Name = "Shroud of Reflections" }); // Deep shimmering blue
            AddItem(new Sandals() { Hue = 2106, Name = "Steps of Silence" }); // Light soft blue
            AddItem(new WizardsHat() { Hue = 1150, Name = "Moon Whisperer’s Hat" });

            AddItem(new SpellWeaversWand() { Hue = 2117, Name = "Veilwalker’s Focus" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Diviner’s Pack";
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
