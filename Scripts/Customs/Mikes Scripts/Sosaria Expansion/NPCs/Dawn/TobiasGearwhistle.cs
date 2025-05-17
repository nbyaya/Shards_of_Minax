using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilenceTheHarbingerQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silence the Harbinger"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Tobias Gearwhistle*, the eccentric Tinker of Dawn.\n\n" +
                    "His goggles glint with oil-smeared lenses, and his hands twitch with nervous energy, stained from recent repairs.\n\n" +
                    "“It was supposed to *play music*, not this!” Tobias exclaims, pulling at a gear-laden vest.\n\n" +
                    "“I built a sound amplifier, a marvel, truly! But now... it broadcasts *curses*—each dusk, the air fills with demonic *sermons*!”\n\n" +
                    "“The source? A fiend they call the **Voicebearer**. He’s twisted pipes and gears in the Doom Dungeon to amplify his foul orations. It’s warping machines—warping minds!”\n\n" +
                    "**Slay the Cult Voicebearer** before his mechanical horns drown Dawn in madness.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may your ears be strong. I fear soon none of us will sleep for the screaming gears...";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The voices grow louder! I hear them in *my machines*, even the clocks are ticking wrong!";
            }
        }

        public override object Complete
        {
            get
            {
                return "Blessed silence! You’ve done it, haven’t you?\n\n" +
                       "*Truthslicer* is yours—a blade honed not just on steel, but on clarity. May it cut through lies as surely as you’ve freed us from the Harbinger’s curse.";
            }
        }

        public SilenceTheHarbingerQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultVoicebearer), "Cult Voicebearer", 1));
            AddReward(new BaseReward(typeof(Truthslicer), 1, "Truthslicer"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silence the Harbinger'!");
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

    public class TobiasGearwhistle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilenceTheHarbingerQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this));
        }

        [Constructable]
        public TobiasGearwhistle()
            : base("the Clockwork Tinker", "Tobias Gearwhistle")
        {
        }

        public TobiasGearwhistle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 70, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Pale, with oil smudges.
            HairItemID = 0x203C; // Short Hair
            HairHue = 1150; // Soot-black
            FacialHairItemID = 0x2041; // Short Beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2500, Name = "Gearsilk Shirt" });
            AddItem(new ShortPants() { Hue = 2207, Name = "Copperbraid Breeches" });
            AddItem(new HalfApron() { Hue = 1820, Name = "Tinker's Belt" });
            AddItem(new Sandals() { Hue = 1810, Name = "Sooty Footpads" });
            AddItem(new FeatheredHat() { Hue = 2115, Name = "Whistlecap" });
            AddItem(new LeatherGloves() { Hue = 2309, Name = "Gearturn Gloves" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1137;
            backpack.Name = "Tobias's Tinker Pack";
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
