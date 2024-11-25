using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the BeetleJuice Overlord")]
    public class BeetleJuiceSummonerBoss : BeetleJuiceSummoner
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(5.0); // Boss speaks faster than normal
        private DateTime m_NextSpeechTime;

        [Constructable]
        public BeetleJuiceSummonerBoss() : base()
        {
            Name = "BeetleJuice Overlord";
            Title = "the Ultimate Summoner";
            Hue = 0x455; // BeetleJuice's pale hue.
            Body = 0x190; // Male body form.

            // Update stats to match or exceed a boss-level difficulty
            SetStr(1200, 1600); // Higher strength than the original
            SetDex(255, 300); // Higher dexterity than the original
            SetInt(250, 350); // Higher intelligence than the original

            SetHits(15000); // Increased health for a boss-tier fight
            SetDamage(50, 75); // Higher damage output than the original

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances than the original
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // Poison resistance stays high
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 120.0, 150.0); // Higher magic resistance
            SetSkill(SkillName.Magery, 120.0, 150.0); // Higher magery skill
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Maximum magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Higher combat tactics
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Higher wrestling skill

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 100; // More virtual armor for survivability

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime && Combatant != null)
            {
                int phrase = Utility.Random(3);

                switch (phrase)
                {
                    case 0: Say("It's showtime, mortal!"); SummonBeetle(); break;
                    case 1: Say("Prepare for chaos!"); SummonBeetle(); break;
                    case 2: Say("You can't stop me!"); SummonBeetle(); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }
        }

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
                        case 0: Say("It's showtime, mortal!"); SummonBeetle(); break;
                        case 1: Say("Prepare for chaos!"); SummonBeetle(); break;
                        case 2: Say("You can't stop me!"); SummonBeetle(); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;                
                }
            }

            return base.Damage(amount, from);
        }

        public void SummonBeetle()
        {
            BaseCreature creature;

            switch (Utility.Random(3))
            {
                default:
                case 0: creature = new Beetle(); break;
                case 1: creature = new FireBeetle(); break;
                case 2: creature = new FireBeetle(); break; // New variant for the boss
            }

            creature.MoveToWorld(this.Location, this.Map);
            creature.Combatant = this.Combatant;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public BeetleJuiceSummonerBoss(Serial serial) : base(serial)
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
