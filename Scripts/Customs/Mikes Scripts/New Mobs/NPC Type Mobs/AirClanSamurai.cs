using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of an air clan samurai")]
    public class AirClanSamurai : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(25.0); // Slightly more frequent speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public AirClanSamurai() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Air Clan Samurai should have a hue that represents their affinity to the air
            Hue = 0x9C2; // Adjust the hue to something air-like or sky-colored

            // Body value could be a variant for air-themed NPCs
            Body = 0x190; // Modify if there's a specific body value for Air Clan NPCs
            Name = NameList.RandomName("male");
            Title = "the Wind Walker"; // A title that reflects their mastery over the air

            // Equip the Air Clan Samurai with lighter, more wind-themed armor
            AddItem(new LeatherDo() { Hue = 0x9C2 });
            AddItem(new LeatherHaidate() { Hue = 0x9C2 });
            AddItem(new LeatherSuneate() { Hue = 0x9C2 });
            AddItem(new LightPlateJingasa() { Hue = 0x9C2 }); // Custom lightweight helmet
            AddItem(new SamuraiTabi());

            // Stats should reflect agility and speed rather than brute force
            SetStr(450, 600);
            SetDex(400, 500);
            SetInt(150, 250);
            SetHits(500, 800);

            SetDamage(70, 85);

            SetSkill(SkillName.Bushido, 90.0, 110.0);
            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Fencing, 90.0, 110.0);
            SetSkill(SkillName.Macing, 90.0, 110.0);
            SetSkill(SkillName.MagicResist, 175.0, 250.0); // Reflects their enhanced agility
            SetSkill(SkillName.Swords, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 15000;
            Karma = 15000;

            // Air Clan Samurai's speech could be enigmatic and serene
            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "The wind is my ally..."); break;
                        case 1: this.Say(true, "I strike like a sudden gust!"); break;
                        case 2: this.Say(true, "You cannot grasp the wind!"); break;
                        case 3: this.Say(true, "Feel the storm's wrath!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            amount = base.Damage(amount, from);

            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Like the breeze, I cannot be contained!"); break;
                        case 1: this.Say(true, "My blade cuts like the sharpest zephyr!"); break;
                        case 2: this.Say(true, "You will falter like a leaf in the tempest!"); break;
                        case 3: this.Say(true, "The gale force is with me!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return amount;
        }

        // Implementing the loot generation for an Air Clan Samurai
        public override void GenerateLoot()
        {
            PackGold(700, 1000);
            // Add more air-themed loot, possibly with wind or agility enhancements
        }

        // ... Other overrides and methods ...

        public AirClanSamurai(Serial serial) : base(serial)
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
