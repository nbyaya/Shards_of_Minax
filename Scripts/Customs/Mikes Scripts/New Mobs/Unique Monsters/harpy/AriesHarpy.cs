using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an aries harpy corpse")]
    public class AriesHarpy : BaseCreature
    {
        private DateTime m_NextRamCharge;
        private DateTime m_NextFireBurst;
        private DateTime m_NextFieryRoar;
        private DateTime m_NextMagmaEruption;
        private DateTime m_NextBlazingFeathers;
        private bool m_FlamingAuraActive;
        private bool m_AbilitiesActivated; // Flag to track initial ability activation

        [Constructable]
        public AriesHarpy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Aries Harpy";
            Body = 30; // Harpy body
            Hue = 2079; // Fiery red hue
            BaseSoundID = 402;

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

            // Initialize variables
            m_NextRamCharge = DateTime.UtcNow;
            m_NextFireBurst = DateTime.UtcNow;
            m_NextFieryRoar = DateTime.UtcNow;
            m_NextMagmaEruption = DateTime.UtcNow;
            m_NextBlazingFeathers = DateTime.UtcNow;
            m_FlamingAuraActive = false;
            m_AbilitiesActivated = false; // Initialize flag
        }

        public AriesHarpy(Serial serial)
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
                    m_NextRamCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextFireBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFieryRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_NextMagmaEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextBlazingFeathers = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextRamCharge)
                {
                    RamCharge();
                }

                if (DateTime.UtcNow >= m_NextFireBurst)
                {
                    FireBurst();
                }

                if (DateTime.UtcNow >= m_NextFieryRoar)
                {
                    FieryRoar();
                }

                if (DateTime.UtcNow >= m_NextMagmaEruption)
                {
                    MagmaEruption();
                }

                if (DateTime.UtcNow >= m_NextBlazingFeathers)
                {
                    BlazingFeathers();
                }

                if (!m_FlamingAuraActive && Utility.RandomDouble() < 0.05) // 5% chance to activate Flaming Aura
                {
                    ActivateFlamingAura();
                }
            }
        }

        private void RamCharge()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Charges with fiery rage! *");
            Effects.PlaySound(Location, Map, 0x208); // Use Location instead of GetWorldLocation
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(1))
            {
                if (m != this && m != Combatant)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                    m.PlaySound(0x208);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            m_NextRamCharge = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void FireBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Erupts in a burst of flames! *");
            Effects.PlaySound(Location, Map, 0x208); // Use Location instead of GetWorldLocation
            FixedEffect(0x3709, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.PlaySound(0x208);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            m_NextFireBurst = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void FieryRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lets out a fiery roar! *");
            Effects.PlaySound(Location, Map, 0x208); // Use Location instead of GetWorldLocation
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.PlaySound(0x208);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m.Freeze(TimeSpan.FromSeconds(2)); // Fear effect
                }
            }

            m_NextFieryRoar = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void MagmaEruption()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Causes magma to erupt from the ground! *");
            Effects.PlaySound(Location, Map, 0x208); // Use Location instead of GetWorldLocation
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.PlaySound(0x208);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    m.SendMessage("You are burned by the magma!");
                    Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                    {
                        if (m.InRange(Location, 3)) // Use Location instead of GetWorldLocation
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 100, 0);
                        }
                    });
                }
            }

            m_NextMagmaEruption = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void BlazingFeathers()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Shoots blazing feathers! *");
            Effects.PlaySound(Location, Map, 0x208); // Use Location instead of GetWorldLocation
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.PlaySound(0x208);
                    m.FixedParticles(0x373A, 10, 30, 5052, EffectLayer.LeftFoot);
                    m.SendMessage("You are hit by a blazing feather!");
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                    {
                        if (m.InRange(Location, 3)) // Use Location instead of GetWorldLocation
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                            m.SendMessage("You are still burning from the feathers!");
                        }
                    });
                }
            }

            m_NextBlazingFeathers = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void ActivateFlamingAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Aries Harpy's aura flares with intense heat! *");
            Effects.PlaySound(Location, Map, 0x208); // Use Location instead of GetWorldLocation
            FixedEffect(0x373A, 10, 16);

            m_FlamingAuraActive = true;
            SetDamageType(ResistanceType.Fire, 70); // Increase fire damage
            VirtualArmor += 10; // Increase armor

            Timer.DelayCall(TimeSpan.FromSeconds(20), () =>
            {
                m_FlamingAuraActive = false;
                SetDamageType(ResistanceType.Fire, 50); // Reset fire damage
                VirtualArmor -= 10; // Reset armor
            });
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

            m_NextRamCharge = DateTime.UtcNow;
            m_NextFireBurst = DateTime.UtcNow;
            m_NextFieryRoar = DateTime.UtcNow;
            m_NextMagmaEruption = DateTime.UtcNow;
            m_NextBlazingFeathers = DateTime.UtcNow;
            m_FlamingAuraActive = false;
            m_AbilitiesActivated = false; // Reset flag
        }
    }
}
