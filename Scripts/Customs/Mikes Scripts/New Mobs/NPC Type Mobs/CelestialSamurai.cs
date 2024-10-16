using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("corpse of a celestial samurai")]
    public class CelestialSamurai : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(30.0); // Time between celestial samurai's speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public CelestialSamurai() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Celestial Samurai should have a radiant hue, representing their celestial nature
            Hue = 0x482; // Adjust the hue to fit your shard's celestial theme

            // Body value could remain humanoid or could be something more ethereal
            Body = 0x190; // Use the shard's specific body value for samurai if available
            Name = NameList.RandomName("male");
            Title = "the Celestial Guardian"; // A title that denotes their role as a protector

            // Equip the Celestial Samurai with appropriate armor and weapons
            AddItem(new LeatherDo());
            AddItem(new StuddedHaidate());
            AddItem(new LeatherHiroSode());
            AddItem(new SamuraiTabi());
            AddItem(new DecorativePlateKabuto());

            // Set stats and skills to reflect a masterful warrior
            SetStr(700, 850);
            SetDex(200, 600);
            SetInt(100, 600);
            SetHits(700, 1400);

            SetDamage(120, 130);

            SetSkill(SkillName.Bushido, 120.0, 150.0);
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.Fencing, 120.0, 200.0);
            SetSkill(SkillName.Macing, 120.0, 200.0);
            SetSkill(SkillName.MagicResist, 120.0, 200.0);
            SetSkill(SkillName.Swords, 120.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 200.0);
            SetSkill(SkillName.Wrestling, 120.0, 200.0);

            // Adjust the karma and fame to reflect their noble stature
            Fame = 20000;
            Karma = 20000;

            // Celestial Samurai's speech could be wise and authoritative
            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Let justice be done!"); break;
                        case 1: this.Say(true, "By the stars, I will protect this land!"); break;
                        case 2: this.Say(true, "Celestial might flows through me!"); break;
                        case 3: this.Say(true, "Your malevolence ends here!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "The heavens shall strike you down!"); break;
                        case 1: this.Say(true, "You face a force beyond your ken!"); break;
                        case 2: this.Say(true, "I am unyielding!"); break;
                        case 3: this.Say(true, "My spirit is eternal!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        // Implementing the loot generation
        public override void GenerateLoot()
        {
            // Celestial Samurai might carry unique artifacts or heavenly-themed items
            PackGold(1000, 1500);
            // Add celestial-themed loot here
        }

        // Serialization methods
        public CelestialSamurai(Serial serial) : base(serial)
        {
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
