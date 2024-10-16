using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a radiant slime corpse")]
    public class RadiantSlime : BaseCreature
    {
        private DateTime m_NextRadiantBurst;
        private DateTime m_NextHealingLight;
        private DateTime m_NextHolyAura;
        private DateTime m_NextDivineProtection;
        private DateTime m_NextSmite;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public RadiantSlime()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a radiant slime";
            Body = 51;
            Hue = 2381; // Unique hue for a glowing appearance
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

            m_AbilitiesInitialized = false;
        }

        public RadiantSlime(Serial serial)
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
                    m_NextRadiantBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextHealingLight = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextHolyAura = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Aura starts immediately
                    m_NextDivineProtection = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextSmite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRadiantBurst)
                {
                    RadiantBurst();
                }

                if (DateTime.UtcNow >= m_NextHealingLight)
                {
                    HealingLight();
                }

                if (DateTime.UtcNow >= m_NextHolyAura)
                {
                    HolyAura();
                }

                if (DateTime.UtcNow >= m_NextDivineProtection)
                {
                    DivineProtection();
                }

                if (DateTime.UtcNow >= m_NextSmite)
                {
                    Smite();
                }
            }
        }

        private void RadiantBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Radiant Slime releases a burst of holy energy! *");
            PlaySound(0x1B3); // Holy sound effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is BaseCreature creature && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                    m.SendMessage("You are struck by a burst of holy energy and blinded!");
                    m.Blessed = true; // Optional: Apply a 'blessed' status effect
                    // Chance to cause blindness
                    if (Utility.RandomDouble() < 0.25)
                        m.SendMessage("You are blinded by the holy light!");
                }
            }

            m_NextRadiantBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void HealingLight()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Radiant Slime bathes in healing light! *");
            PlaySound(0x1E0); // Healing sound effect

            if (Utility.RandomDouble() < 0.5)
            {
                Hits += Utility.RandomMinMax(15, 25);
            }
            else
            {
                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m.Alive && !m.IsDeadBondedPet)
                    {
                        m.SendMessage("You feel a soothing healing light!");
                        m.Hits += Utility.RandomMinMax(15, 25);

                        // Chance to cleanse harmful effects
                        if (Utility.RandomDouble() < 0.5)
                            m.SendMessage("You are cleansed of harmful effects!");
                        break;
                    }
                }
            }

            m_NextHealingLight = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void HolyAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Radiant Slime emits a holy aura! *");
            PlaySound(0x1B1); // Holy aura sound effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && (m is BaseCreature creature || (m.Karma < 0 && m is PlayerMobile)))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.SendMessage("You are hurt by the holy aura!");
                    // Chance to stun
                    if (Utility.RandomDouble() < 0.25)
                    {
                        m.SendMessage("You are stunned by the holy aura!");
                        m.Frozen = true;
                    }
                }
            }

            m_NextHolyAura = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void DivineProtection()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Radiant Slime is enveloped in divine protection! *");
            PlaySound(0x1E1); // Protective sound effect

            // Temporarily increase resistances and immunity
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 70, 80);

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                // Revert resistances after 10 seconds
                SetResistance(ResistanceType.Physical, 35, 45);
                SetResistance(ResistanceType.Fire, 20, 30);
                SetResistance(ResistanceType.Cold, 20, 30);
                SetResistance(ResistanceType.Poison, 20, 30);
                SetResistance(ResistanceType.Energy, 50, 60);
            });

            m_NextDivineProtection = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void Smite()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Radiant Slime calls down a powerful smite! *");
                PlaySound(0x1B1); // Smite sound effect

                AOS.Damage(Combatant, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
				Mobile mobile = Combatant as Mobile;
				if (mobile != null)
				{
					mobile.SendMessage("You are struck by a powerful divine smite!");
				}


                m_NextSmite = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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
            m_AbilitiesInitialized = false;
        }
    }
}
