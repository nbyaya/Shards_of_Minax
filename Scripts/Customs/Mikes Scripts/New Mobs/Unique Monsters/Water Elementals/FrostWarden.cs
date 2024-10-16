using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frost warden corpse")]
    public class FrostWarden : BaseCreature
    {
        private DateTime m_NextFrostBreath;
        private DateTime m_NextFrostNova;
        private DateTime m_NextIcyBarrier;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostWarden()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a frost warden";
            this.Body = 16; // Water Elemental body
            this.BaseSoundID = 278;
			Hue = 2505; // Blue hue for storm effect

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
            this.CanSwim = true;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FrostWarden(Serial serial)
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
                    Random rand = new Random();
                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextIcyBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostBreath)
                {
                    FrostBreath();
                }

                if (DateTime.UtcNow >= m_NextIcyBarrier)
                {
                    IcyBarrier();
                }

                if (DateTime.UtcNow >= m_NextFrostNova)
                {
                    FrostNova();
                }
            }
        }

        private void FrostBreath()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    // Apply cold damage and slow down
                    target.Damage(Utility.RandomMinMax(10, 15), this);
                    target.SendMessage("You are chilled by the Frost Warden's breath!");
                    target.Freeze(TimeSpan.FromSeconds(2)); // Reduce movement speed

                    m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown
                }
            }
        }

        private void IcyBarrier()
        {
            // Absorb a portion of incoming damage
            this.VirtualArmor += 20; // Example value, adjust as needed
            this.SendMessage("The Frost Warden is protected by an icy barrier!");

            m_NextIcyBarrier = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown
        }

        private void FrostNova()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    // Apply cold damage and slow down
                    m.Damage(Utility.RandomMinMax(20, 30), this);
                    m.SendMessage("You are caught in a Frost Nova!");
                    m.Freeze(TimeSpan.FromSeconds(4)); // Slow down

                    // Use SendLocationParticles to create a visual effect
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x36D4, 9, 32, 1153);
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.Write(m_NextFrostBreath);
            writer.Write(m_NextIcyBarrier);
            writer.Write(m_NextFrostNova);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_NextFrostBreath = reader.ReadDateTime();
                    m_NextIcyBarrier = reader.ReadDateTime();
                    m_NextFrostNova = reader.ReadDateTime();
                    break;
            }

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
