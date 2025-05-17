using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShiftingPreyQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shifting Prey"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself before *Kellan Skyfeather*, Falconer of Castle British.\n\n" +
                    "Clad in sky-hued leathers and bearing a falconer's gauntlet with no bird, he stares toward the horizon.\n\n" +
                    "“I raised her from a hatchling… *Aelith*, proud and swift. She could outfly storms.”\n\n" +
                    "“But two nights ago, something unnatural came from the Vault—*the Phasebeak*. It *phased* through stone itself, snatched her, and vanished into that cursed place.”\n\n" +
                    "“The scholars say it hunts by scent and sound, slipping through walls like mist. If it escapes again, it will prey on more than my falcons—it'll take the captive wyverns kept in the vault's lower halls.”\n\n" +
                    "“I’ve crafted nets woven with ethereal threads, meant to *bind* its shifting form. But I need a hunter brave enough to face it.”\n\n" +
                    "**Slay the Phasebeak** before it phases out again. Aelith’s spirit demands justice.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let the vault's stones echo with her cries. But mark me, the Phasebeak will not stop.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it flies, unseen? Each night grows colder without her. Hurry, before it phases free.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve slain it? Aelith… she’s free now. And the vault’s creatures are safe.\n\n" +
                       "Take these—*UndertideTreads*. They are light as air, firm as the sky. May they carry you as far as your heart dares fly.";
            }
        }

        public ShiftingPreyQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Phasebeak), "Phasebeak", 1));
            AddReward(new BaseReward(typeof(UndertideTreads), 1, "UndertideTreads"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shifting Prey'!");
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

    public class KellanSkyfeather : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShiftingPreyQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public KellanSkyfeather()
            : base("the Falconer", "Kellan Skyfeather")
        {
        }

        public KellanSkyfeather(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Wind-blown silver
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2101, Name = "Skybound Jerkin" }); // Light blue
            AddItem(new StuddedArms() { Hue = 2101, Name = "Wingspan Sleeves" });
            AddItem(new StuddedLegs() { Hue = 2115, Name = "Falconer’s Greaves" }); // Deep grey-blue
            AddItem(new LeatherGloves() { Hue = 2405, Name = "Talonguard Gauntlets" }); // Dark grey
            AddItem(new FeatheredHat() { Hue = 2117, Name = "Skyfeather’s Plume" }); // Azure with a silver feather
            AddItem(new Boots() { Hue = 1819, Name = "Windwalker Boots" }); // Storm-grey

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Ethereal Net Pouch";
            AddItem(backpack);

            AddItem(new ShepherdsCrook() { Hue = 2101, Name = "Falconer’s Crook" }); // Aesthetic staff for bird calling
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
