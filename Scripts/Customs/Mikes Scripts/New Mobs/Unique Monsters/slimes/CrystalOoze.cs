using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a crystalline ooze corpse")]
    public class CrystalOoze : BaseCreature
    {
        private DateTime m_NextCrystallineSpikes;
        private DateTime m_NextManaAbsorption;
        private DateTime m_NextDefensiveShield;
        private DateTime m_NextReflectiveShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CrystalOoze()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Crystal Ooze";
            Body = 51; // Slime body
            Hue = 2397; // Unique crystal-like hue
			BaseSoundID = 456;

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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public CrystalOoze(Serial serial)
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
                    m_NextCrystallineSpikes = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextManaAbsorption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDefensiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextReflectiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCrystallineSpikes)
                {
                    CrystallineSpikes();
                }

                if (DateTime.UtcNow >= m_NextManaAbsorption)
                {
                    ManaAbsorption();
                }

                if (DateTime.UtcNow >= m_NextDefensiveShield)
                {
                    DefensiveShield();
                }

                if (DateTime.UtcNow >= m_NextReflectiveShield)
                {
                    ReflectiveShield();
                }
            }
        }

        private void CrystallineSpikes()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crystal Ooze releases sharp crystalline spikes! *");
            PlaySound(0x226); // Shard sound
            Effects.SendLocationEffect(Location, Map, 0x36BD, 20); // Visual effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0); // Physical damage
                    m.SendMessage("You are hit by sharp crystalline spikes!");
                }
            }

            m_NextCrystallineSpikes = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for CrystallineSpikes
        }

        private void ManaAbsorption()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crystal Ooze absorbs magical energy! *");
            PlaySound(0x1E4); // Mana absorb sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m is BaseCreature creature && creature.Mana > 0)
                {
                    int absorbedMana = Utility.RandomMinMax(10, 20);
                    creature.Mana -= absorbedMana;
                    Hits += absorbedMana; // Restore health based on absorbed mana
                    m.SendMessage("Your magical energy is absorbed by the Crystal Ooze!");
                }
            }

            m_NextManaAbsorption = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ManaAbsorption
        }

        private void DefensiveShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crystal Ooze activates a defensive shield! *");
            PlaySound(0x1E3); // Shield sound

            VirtualArmor += 20;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => DeactivateDefensiveShield());

            m_NextDefensiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for DefensiveShield
        }

        private void DeactivateDefensiveShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crystal Ooze's defensive shield fades away. *");
            VirtualArmor -= 20;
        }

        private void ReflectiveShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crystal Ooze's reflective shield absorbs magic! *");
            PlaySound(0x1E4); // Magic absorb sound

            m_NextReflectiveShield = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for ReflectiveShield
        }


        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (willKill)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Crystal Ooze erupts in a final explosion of crystals! *");
                PlaySound(0x226); // Shard sound

                Effects.SendLocationEffect(Location, Map, 0x36BD, 20); // Explosion effect

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive && CanBeHarmful(m))
                    {
                        AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0); // Final explosion damage
                        m.SendMessage("You are caught in the Crystal Ooze's final explosion!");
                    }
                }
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
