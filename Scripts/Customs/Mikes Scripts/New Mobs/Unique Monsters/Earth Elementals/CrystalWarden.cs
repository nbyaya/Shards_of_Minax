using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a crystal warden corpse")]
    public class CrystalWarden : BaseCreature
    {
        private DateTime m_NextCrystalShield;
        private DateTime m_NextPrismaticBurst;
        private bool m_CrystalShieldActive;
        private bool m_AbilitiesInitialized; // Flag to check if abilities have been initialized

        [Constructable]
        public CrystalWarden()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a crystal warden";
            Body = 14; // Earth Elemental body
            BaseSoundID = 268;
            Hue = 1497; // Unique crystalline hue

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

            PackItem(new PowerCrystal());
            PackItem(new MagicCrystal());

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public CrystalWarden(Serial serial)
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

        public override bool BleedImmune { get { return true; } }
        public override double WeaponAbilityChance { get { return 0.3; } }


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    // Initialize random intervals for abilities
                    Random rand = new Random();
                    m_NextCrystalShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextPrismaticBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCrystalShield)
                {
                    CrystalShield();
                }

                if (DateTime.UtcNow >= m_NextPrismaticBurst)
                {
                    PrismaticBurst();
                }
            }
        }

        private void CrystalShield()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Summons a crystalline barrier *");
            PlaySound(0x1ED);
            FixedEffect(0x375A, 10, 15);

            m_CrystalShieldActive = true;
            Timer.DelayCall(TimeSpan.FromSeconds(15), new TimerCallback(DeactivateCrystalShield));

            m_NextCrystalShield = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown after use
        }

        private void DeactivateCrystalShield()
        {
            m_CrystalShieldActive = false;
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The crystal shield dissipates *");
        }

        private void PrismaticBurst()
        {
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* Emits a dazzling burst of light *");
            PlaySound(0x212);
            FixedEffect(0x3709, 10, 20);

            List<Mobile> targets = new List<Mobile>(); // Declare and initialize the 'targets' list
            IPooledEnumerable eable = GetMobilesInRange(5);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            eable.Free();

            foreach (Mobile m in targets)
            {
                m.SendLocalizedMessage(500898); // A blinding light explodes in front of you!
                m.Freeze(TimeSpan.FromSeconds(5));
                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bleed, 1075643, 1075644, TimeSpan.FromSeconds(5), m, "50"));
            }

            m_NextPrismaticBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown after use
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (m_CrystalShieldActive)
            {
                int reflectedDamage = (int)(damage * 0.3);
                from.Damage(reflectedDamage, this);
                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The crystal shield reflects damage *");
            }

            base.AlterMeleeDamageFrom(from, ref damage);
        }

        public override void AlterSpellDamageFrom(Mobile from, ref int damage)
        {
            if (m_CrystalShieldActive)
            {
                int reflectedDamage = (int)(damage * 0.3);
                from.Damage(reflectedDamage, this);
                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, "* The crystal shield reflects damage *");
            }

            base.AlterSpellDamageFrom(from, ref damage);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_CrystalShieldActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_CrystalShieldActive = reader.ReadBool();

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialize
        }
    }
}
