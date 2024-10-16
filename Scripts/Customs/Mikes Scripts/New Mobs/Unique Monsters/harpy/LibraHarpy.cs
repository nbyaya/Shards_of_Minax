using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a libra harpy corpse")]
    public class LibraHarpy : BaseCreature
    {
        private DateTime m_NextBalanceBeam;
        private DateTime m_NextJudgment;
        private DateTime m_NextHarmonicWave;
        private bool m_AbilitiesActivated; // Flag to track initial ability activation

        [Constructable]
        public LibraHarpy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Libra Harpy";
            Body = 30; // Harpy body
            Hue = 2071; // Unique hue for Libra Harpy
			BaseSoundID = 402; // Harpy sound

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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public LibraHarpy(Serial serial)
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
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextBalanceBeam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextJudgment = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextHarmonicWave = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextBalanceBeam)
                {
                    BalanceBeam();
                }

                if (DateTime.UtcNow >= m_NextJudgment)
                {
                    Judgment();
                }

                if (DateTime.UtcNow >= m_NextHarmonicWave)
                {
                    HarmonicWave();
                }
            }
        }

        private void BalanceBeam()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes Balance Beam! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 0);
                }
            }

            m_NextBalanceBeam = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
        }

        private void Judgment()
        {
            if (Combatant == null || !Combatant.Alive) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Executes Judgment! *");
            FixedEffect(0x376A, 10, 16);

            int highestResistance = Math.Max(Math.Max(Combatant.PhysicalResistance, Combatant.FireResistance), 
                                            Math.Max(Combatant.ColdResistance, Combatant.PoisonResistance));
            int damage = Utility.RandomMinMax(20, 30) + (highestResistance / 5);

            AOS.Damage(Combatant, this, damage, 0, 0, 0, 0, 0);

            m_NextJudgment = DateTime.UtcNow + TimeSpan.FromSeconds(35); // Reset cooldown
        }

        private void HarmonicWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sends out Harmonic Wave! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(8, 12);
                    int debuff = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 0);

                    // Apply debuff (e.g., reduce resistance)
                    m.SendMessage("You feel weakened by the Harmonic Wave!");
                    m.Damage(debuff, this);
                }
            }

            m_NextHarmonicWave = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Reset cooldown
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (Utility.RandomDouble() < 0.1 && !willKill) // 10% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Libra Harpy enrages and gains strength! *");
                this.SetDamage(this.DamageMin + 5, this.DamageMax + 5);
                this.SetResistance(ResistanceType.Physical, this.PhysicalResistance + 10);
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

            m_AbilitiesActivated = false; // Reset flag on deserialize
        }
    }
}
