using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a celestial dragon corpse")]
    public class CelestialDragon : BaseCreature
    {
        private DateTime m_NextCelestialBreath;
        private DateTime m_NextHeavenlyAura;
        private DateTime m_NextDivineStrike;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CelestialDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a celestial dragon";
            Body = Utility.RandomList(12, 59); // Using dragon body
            Hue = 1493; // Celestial blue hue
            BaseSoundID = 362;

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

        public CelestialDragon(Serial serial)
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
                    m_NextCelestialBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextHeavenlyAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDivineStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCelestialBreath)
                {
                    CelestialBreath();
                }

                if (DateTime.UtcNow >= m_NextHeavenlyAura)
                {
                    HeavenlyAura();
                }

                if (DateTime.UtcNow >= m_NextDivineStrike)
                {
                    DivineStrike();
                }
            }
        }

        private void CelestialBreath()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Celestial Breath *");
            PlaySound(0x657); // Celestial sound
            FixedEffect(0x376A, 10, 16);

            // Damage and heal effect
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    if (m == Combatant)
                    {
                        m.Damage(30, this);
                        m.PlaySound(0x1F1);
                    }
                    else if (m.Player)
                    {
                        m.Heal(20);
                        m.SendMessage("You are healed by the Celestial Dragon's breath!");
                    }
                }
            }

            m_NextCelestialBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void HeavenlyAura()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Heavenly Aura *");
            PlaySound(0x65B); // Aura sound
            FixedEffect(0x376A, 10, 20);

            // Visual effect and message to signify the aura
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is BaseCreature creature && creature.Team == Team)
                {
                    // Apply a temporary boost by increasing stats directly
                    creature.Str += 20;
                    creature.Dex += 10;
                    creature.Int += 10;

                    // Remove boost after a duration
                    Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
                    {
                        if (creature != null && !creature.Deleted)
                        {
                            creature.Str -= 20;
                            creature.Dex -= 10;
                            creature.Int -= 10;
                        }
                    });
                }
            }

            m_NextHeavenlyAura = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void DivineStrike()
        {
            if (Combatant == null) return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Divine Strike *");
            PlaySound(0x657); // Strike sound
            FixedEffect(0x376A, 10, 16);

            if (Combatant is Mobile target)
            {
                int damage = Utility.RandomMinMax(40, 50);
                target.Damage(damage, this);
                target.SendMessage("You are struck by divine energy!");
                target.PlaySound(0x1F1);
                target.FixedEffect(0x376A, 10, 16);
                // Chance to blind the target
                if (Utility.RandomBool())
                {
                    target.SendMessage("The divine energy blinds you!");
                    target.Freeze(TimeSpan.FromSeconds(5));
                }
            }

            m_NextDivineStrike = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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
