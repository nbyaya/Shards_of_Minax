using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SanctifyTheFallenQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Sanctify the Fallen"; } }

        public override object Description
        {
            get
            {
                return
                    "*Father Baldric Gravewarden* stands before the scorched chapel of Death Glutch, a heavy iron cross clutched tightly in his hands.\n\n" +
                    "His eyes bear the weight of sleepless nights and a sorrow no words can soothe.\n\n" +
                    "“I am the keeper of the dead, the warden of their peace. And now, I have failed them.”\n\n" +
                    "“A *Romero Zombie*—twisted mockery of life—has risen within the chapel crypts of *Malidor Witches Academy*. Once a sanctified resting place, now a pit of unholy rot.”\n\n" +
                    "“This desecration... it must end. I cannot cleanse it alone. **Slay the Romero Zombie** and let me sanctify those halls once more.”\n\n" +
                    "“Take this relic cross. It shall weaken the undead. Bring me proof, and I shall reward you with the shadows’ own cloak—woven by hands unseen.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the fallen shall know no peace, and I... I shall bear their suffering alone.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Romero Zombie still mocks us? I feel its presence every night, clawing at my mind. End it, for their sake... and mine.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have done what I could not. The crypts stir no more, and the sacred ground breathes again.\n\n" +
                       "Take this, the *RoguesShadowCloak*. May it shield you in the dark, as you have shielded the souls of the dead.";
            }
        }

        public SanctifyTheFallenQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(RomeroZombie), "Romero Zombie", 1));
            AddReward(new BaseReward(typeof(RoguesShadowCloak), 1, "RoguesShadowCloak"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Sanctify the Fallen'!");
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

    public class FatherBaldric : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SanctifyTheFallenQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHolyMage());
        }

        [Constructable]
        public FatherBaldric()
            : base("the Gravewarden", "Father Baldric")
        {
        }

        public FatherBaldric(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 80, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1150; // Pale, worn complexion
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // Dark grey
            FacialHairItemID = 0x2041; // Goatee
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1109, Name = "Gravewarden's Robe" }); // Ash-grey robe
            AddItem(new Sandals() { Hue = 1150, Name = "Dust-Worn Sandals" });
            AddItem(new Cloak() { Hue = 1151, Name = "Veil of the Vigilant" });

            // Relic cross
            AddItem(new HolyKnightSword() { Hue = 2401, Name = "Relic Cross of Sanctity" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Sanctified Satchel";
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
