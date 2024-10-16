using System;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an arcane satyr corpse")]
    public class ArcaneSatyr : BaseCreature
    {
        private DateTime m_NextArcaneAnthem;
        private DateTime m_NextMysticCrescendo;
        private DateTime m_NextRunicResonance;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ArcaneSatyr()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an arcane satyr";
            Body = 271;
            Hue = 2337; // Unique hue for Arcane Satyr
            BaseSoundID = 0x586;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public ArcaneSatyr(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextArcaneAnthem = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMysticCrescendo = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextRunicResonance = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextArcaneAnthem)
                {
                    ArcaneAnthem();
                }

                if (DateTime.UtcNow >= m_NextMysticCrescendo)
                {
                    MysticCrescendo();
                }

                if (DateTime.UtcNow >= m_NextRunicResonance)
                {
                    RunicResonance();
                }
            }
        }

        private void ArcaneAnthem()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays an arcane anthem *");
            PlaySound(0x1F3);
            FixedEffect(0x37C4, 10, 30);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature creature)
                {
                }
            }

            m_NextArcaneAnthem = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void MysticCrescendo()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Releases a mystic crescendo *");
            PlaySound(0x1F4);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    m.Damage(damage, this);
                    ApplyRandomDebuff(m);
                }
            }

            m_NextMysticCrescendo = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void ApplyRandomDebuff(Mobile target)
        {
            int debuff = Utility.Random(3);

            switch (debuff)
            {
                case 0:
                    target.SendMessage("You feel weaker!");
                    target.AddStatMod(new StatMod(StatType.Str, "MysticCrescendo", -10, TimeSpan.FromSeconds(10)));
                    break;
                case 1:
                    target.SendMessage("You feel slow!");
                    target.AddStatMod(new StatMod(StatType.Dex, "MysticCrescendo", -10, TimeSpan.FromSeconds(10)));
                    break;
                case 2:
                    target.SendMessage("Your mind is clouded!");
                    target.AddStatMod(new StatMod(StatType.Int, "MysticCrescendo", -10, TimeSpan.FromSeconds(10)));
                    break;
            }
        }

        private void RunicResonance()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Engraves runic resonance *");
            PlaySound(0x1F5);
            FixedEffect(0x37C4, 10, 36);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this)
                {
                    ApplyRunicBuffDebuff(m);
                }
            }

            m_NextRunicResonance = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void ApplyRunicBuffDebuff(Mobile target)
        {
            int effect = Utility.Random(2);

            switch (effect)
            {
                case 0:
                    target.SendMessage("You feel invigorated!");
                    target.AddStatMod(new StatMod(StatType.Str, "RunicResonance", 10, TimeSpan.FromSeconds(30)));
                    break;
                case 1:
                    target.SendMessage("You feel drained!");
                    target.AddStatMod(new StatMod(StatType.Str, "RunicResonance", -10, TimeSpan.FromSeconds(30)));
                    break;
            }
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

            m_AbilitiesInitialized = false; // Reset the flag to reinitialize random intervals
        }
    }
}
