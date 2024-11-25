using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the sumo overlord")]
    public class SumoWrestlerBoss : SumoWrestler
    {
        [Constructable]
        public SumoWrestlerBoss() : base()
        {
            Name = "Sumo Overlord";
            Title = "the Supreme Wrestler";

            // Enhance stats to match or exceed Barracoon's boss-tier
            SetStr(1400); // Matching or exceeding Barracoon's strength
            SetDex(150);  // Use a higher dexterity value for a more challenging fight
            SetInt(100);  // Boss-tier intelligence

            SetHits(1600); // Enhanced health for a more formidable fight
            SetDamage(30, 40); // Higher damage for boss-tier NPCs

            SetResistance(ResistanceType.Physical, 85); // Max physical resistance for a boss
            SetResistance(ResistanceType.Fire, 60); // High fire resistance
            SetResistance(ResistanceType.Cold, 60); // High cold resistance
            SetResistance(ResistanceType.Poison, 80); // High poison resistance
            SetResistance(ResistanceType.Energy, 60); // High energy resistance

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Higher skill for tougher fight
            SetSkill(SkillName.Wrestling, 120.0, 140.0); // Stronger wrestling skill
            SetSkill(SkillName.MagicResist, 100.0, 120.0); // Magic resistance for more challenge
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Tactics skill for enhanced strategy

            Fame = 10000;  // Increased fame for a boss-tier enemy
            Karma = -10000; // Negative karma for a hostile boss

            VirtualArmor = 80; // Increased armor for better defense

            // Attach a random ability to add variety to the fight
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Add 5 MaxxiaScrolls in addition to the original loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Optional: You can customize the speech or the final phrase after defeat here
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "I have been defeated..."); break;
                case 1: this.Say(true, "You were stronger... this time..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20))); // Original loot
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optional: More powerful speech effects could be added for the boss, enhancing immersion
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Prepare to be crushed!"); break;
                        case 1: this.Say(true, "Feel the strength of a sumo!"); break;
                        case 2: this.Say(true, "You cannot escape my grasp!"); break;
                        case 3: this.Say(true, "I will push you to defeat!"); break;
                    }

                }
            }
        }

        public SumoWrestlerBoss(Serial serial) : base(serial)
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
