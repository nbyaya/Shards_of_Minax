using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the DinoRider Overlord")]
    public class DinoRiderBoss : DinoRider
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0); // Shorter speech delay for a more dynamic encounter

        [Constructable]
        public DinoRiderBoss() : base()
        {
            Name = "DinoRider Overlord";
            Title = "the DinoRider Supreme";

            // Enhanced stats to match or exceed Barracoon and make it a challenging boss
            SetStr(1200); // Higher strength than original
            SetDex(255); // Maximum dexterity for more agility
            SetInt(250); // Increased intelligence

            SetHits(12000); // Increased health to be a formidable foe

            SetDamage(25, 45); // Increased damage for a tougher fight

            SetResistance(ResistanceType.Physical, 80, 90); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 70, 80); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Maximum poison resistance
            SetResistance(ResistanceType.Energy, 60, 70); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // High magic resist for a boss
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics for better strategy
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill
            SetSkill(SkillName.Archery, 100.0); // Some ranged capability

            Fame = 25000; // Higher fame to reflect boss status
            Karma = -25000; // High negative karma for the boss

            VirtualArmor = 80; // Increased virtual armor to make the boss tankier

            AddHorse(); // Mount it on a more powerful mount

            m_SpeechDelay = TimeSpan.FromSeconds(3.0); // Shorter speech delay for more dynamic behavior

            // Attach a random ability (using XmlRandomAbility)
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override loot generation to include 5 MaxxiaScrolls
        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Customized speech when defeated
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "The dinosaurs... will avenge me..."); break;
                case 1: this.Say(true, "The tribe... will reclaim the earth..."); break;
            }

            PackItem(new Bone(Utility.RandomMinMax(10, 20))); // Include bones in loot
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    // Boss speech
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "For the tribe!"); break;
                        case 1: this.Say(true, "Feel the wrath of prehistoric times!"); break;
                        case 2: this.Say(true, "Roar! Face the might!"); break;
                        case 3: this.Say(true, "You no match for DinoRider!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        // Override the Damage method to make the boss more talkative when hit
        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(3);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "You pay for that!"); break;
                        case 1: this.Say(true, "Not strong enough!"); break;
                        case 2: this.Say(true, "DinoRiders never fall easily!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public DinoRiderBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
